using ENCO.DDD.Application.Dto;
using ENCO.DDD.Application.Extensions;
using ENCO.DDD.Service;
using IFPS.Factory.Application.Dto;
using IFPS.Factory.Application.Exceptions;
using IFPS.Factory.Application.Interfaces;
using IFPS.Factory.Domain.Enums;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using LinqKit;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IFPS.Factory.Application.Services
{
    public class OptimizationAppService : ApplicationService, IOptimizationAppService
    {
        private readonly IOptimizationRepository optimizationRepository;
        private readonly IDrillRepository drillRepository;
        private readonly IOrderRepository orderRepository;
        private readonly IBoardMaterialRepository boardMaterialRepository;
        private readonly IWorkStationRepository workStationRepository;
        private readonly ISortingStrategyTypeRepository sortingStrategyTypeRepository;
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly ISchedulingOptimizationService schedulingOptimizationService;
        private readonly IPlanRepository planRepository;
        private readonly IProductionProcessRepository productionProcessRepository;
        private readonly IFurnitureComponentRepository furnitureComponentRepository;
        private readonly ICncCodeGenerationService cncCodeGenerationService;
        private readonly IConcreteFurnitureComponentRepository concreteFurnitureComponentRepository;
        private readonly ICncMachineRepository cncMachineRepository;
        private readonly IFileHandlerService fileHandlerService;

        public OptimizationAppService(
            IApplicationServiceDependencyAggregate aggregate,
            IDrillRepository drillRepository,
            IOptimizationRepository optimizationRepository,
            IOrderRepository orderRepository,
            IBoardMaterialRepository boardMaterialRepository,
            IWorkStationRepository workStationRepository,
            ISortingStrategyTypeRepository sortingStrategyTypeRepository,
            IHostingEnvironment hostingEnvironment,
            ISchedulingOptimizationService schedulingOptimizationService,
            IPlanRepository planRepository,
            IProductionProcessRepository productionProcessRepository,
            IFurnitureComponentRepository furnitureComponentRepository,
            ICncCodeGenerationService cncCodeGenerationService,
            IConcreteFurnitureComponentRepository concreteFurnitureComponentRepository,
            ICncMachineRepository cncMachineRepository,
            IFileHandlerService fileHandlerService
        ) : base(aggregate)
        {
            this.drillRepository = drillRepository;
            this.optimizationRepository = optimizationRepository;
            this.orderRepository = orderRepository;
            this.boardMaterialRepository = boardMaterialRepository;
            this.workStationRepository = workStationRepository;
            this.sortingStrategyTypeRepository = sortingStrategyTypeRepository;
            this.hostingEnvironment = hostingEnvironment;
            this.schedulingOptimizationService = schedulingOptimizationService;
            this.planRepository = planRepository;
            this.productionProcessRepository = productionProcessRepository;
            this.furnitureComponentRepository = furnitureComponentRepository;
            this.cncCodeGenerationService = cncCodeGenerationService;
            this.concreteFurnitureComponentRepository = concreteFurnitureComponentRepository;
            this.cncMachineRepository = cncMachineRepository;
            this.fileHandlerService = fileHandlerService;
        }

        public async Task<PagedListDto<OptimizationListDto>> GetOptimizationsAsync(OptimizationFilterDto optimizationFilterDto)
        {
            Expression<Func<Optimization, bool>> filter = (Optimization o) => true;

            if (optimizationFilterDto.ShiftNumber.HasValue)
            {
                filter = filter.And(ent => ent.ShiftNumber == optimizationFilterDto.ShiftNumber);
            }
            if (optimizationFilterDto.ShiftLength.HasValue)
            {
                filter = filter.And(ent => ent.ShiftLength == optimizationFilterDto.ShiftLength);
            }
            if (optimizationFilterDto.StartedAtFrom.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime >= optimizationFilterDto.StartedAtFrom);
            }
            if (optimizationFilterDto.StartedAtTo.HasValue)
            {
                filter = filter.And(ent => ent.CreationTime <= optimizationFilterDto.StartedAtTo);
            }

            var orderingQuery = optimizationFilterDto.Orderings.ToOrderingExpression<Optimization>(OptimizationFilterDto.GetOrderingMapping(), nameof(Optimization.Id));

            var optimizations = await optimizationRepository.GetPagedListAsync(filter, orderingQuery, optimizationFilterDto.PageIndex, optimizationFilterDto.PageSize);
            return optimizations.ToPagedList(OptimizationListDto.FromEntity);
        }

        public async Task StartOrdersOptimization(OrdersForOptimizationDto ordersDto, int userId)
        {
            var optimization = new Optimization(ordersDto.ShiftNumber, ordersDto.ShiftLengthHours);

            var concreteFurnitureUnitIds = new List<int>();
            var concreteFurnitureComponentIds = new List<int>();
            var drills = await drillRepository.GetAllListIncludingAsync(e => true, ent => ent.Holes);

            var scheduleMode = "MODE_ADD_ORDERS";
            var result = new OrderForCuttingListDto();
            foreach (var orderId in ordersDto.OrderIds)
            {
                var order = await orderRepository.GetOrderByIdForCutting(orderId);
                if (order.CurrentTicket.OrderState != null && order.CurrentTicket.OrderState.State == OrderStateEnum.UnderProduction && !string.Equals(scheduleMode, "MODE_RESCHEDULE"))
                {
                    scheduleMode = "MODE_RESCHEDULE";
                }
                result.SetProperties(order, concreteFurnitureUnitIds, concreteFurnitureComponentIds, drills);

                optimization.AddOrder(order);
            }

            foreach (var cfc in result.ConcreteFurnitureComponents)
            {
                var board = await boardMaterialRepository.SingleAsync(ent => ent.Id == cfc.BoardGuid);
                var newBoard = new BoardMaterialForCuttingListDto(board);
                if (!result.Boards.Exists(ent => ent.Guid == newBoard.Guid))
                {
                    result.Boards.Add(newBoard);
                }
            }

            var workStationDto = new WorkStationsForCuttingListDto();
            var workstations = await workStationRepository.GetAllListIncludingAsync(ent => true, ent => ent.WorkStationType, ent => ent.Plans);
            foreach (var workstation in workstations)
            {
                workStationDto.SetWorkStationProperties(workstation);
            }

            result.Workstations = workStationDto;

            var sortingStrategyType = await sortingStrategyTypeRepository.GetSortingStrategyTypeById(ordersDto.SortingStrategyTypeId);

            string path = Path.Combine(hostingEnvironment.ContentRootPath, "AppData", "Optimizer");
            var inputFilePath = Path.Combine(path, "input.json");

            // create layout and schedule output folders
            var layoutDir = Directory.CreateDirectory(Path.Combine(path, "output", optimization.Id.ToString(), "layout"));
            System.IO.File.Copy(Path.Combine(path, "layout_style.css"), Path.Combine(layoutDir.FullName, "style.css"));
            var scheduleDir = Directory.CreateDirectory(Path.Combine(path, "output", optimization.Id.ToString(), "schedule"));
            System.IO.File.Copy(Path.Combine(path, "schedule_style.css"), Path.Combine(scheduleDir.FullName, "style.css"));

            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            System.IO.File.WriteAllText(inputFilePath, JsonConvert.SerializeObject(result, serializerSettings));

            // run layout planning C++
            var cuttingsJsonFilePath = schedulingOptimizationService.RunLayoutPlanning(inputFilePath, layoutDir.FullName, sortingStrategyType.StrategyType.ToString());
            // run job scheduling C++
            var resultJsonFilePath = schedulingOptimizationService.RunJobScheduling(inputFilePath, scheduleDir.FullName, cuttingsJsonFilePath, ordersDto.ShiftNumber, ordersDto.ShiftLengthHours, scheduleMode);

            // generate and save layoutplan zip
            var zipPath = Path.Combine(path, "output", optimization.Id.ToString(), "layoutplans.zip");
            ZipFile.CreateFromDirectory(layoutDir.FullName, zipPath);
            if (System.IO.File.Exists(zipPath))
            {
                var layoutOptFile = new LayoutPlanZipFile(Path.Combine("output", optimization.Id.ToString(), "layoutplans.zip"), "Optimizer") { OptimizationId = optimization.Id };
                optimization.LayoutPlanZip = layoutOptFile;
                optimization.LayoutPlanZipId = layoutOptFile.Id;
            }

            // generate and save schedule zip
            var scheduleZipPath = Path.Combine(path, "output", optimization.Id.ToString(), "schedule.zip");
            ZipFile.CreateFromDirectory(scheduleDir.FullName, scheduleZipPath);            
            if (System.IO.File.Exists(scheduleZipPath))
            {
                var scheduleOptFile = new ScheduleZipFile(Path.Combine("output", optimization.Id.ToString(), "schedule.zip"), "Optimizer") { OptimizationId = optimization.Id };
                optimization.ScheduleZip = scheduleOptFile;
                optimization.ScheduleZipId = scheduleOptFile.Id;
            }

            // create and save plans and production processes, set order's status
            var resultFile = Path.GetFileName(resultJsonFilePath);
            var planCreateDto = ReadFromJson(resultJsonFilePath);

            var planDictionary = planCreateDto.SetProperties();
            var orderIds = new List<Guid>();

            var cncMachines = await cncMachineRepository.GetAllListAsync();

            foreach (var planKvp in planDictionary)
            {
                Plan plan = planKvp.Key;
                // if cnc plan, generate code for plan
                if (plan.GetType() == typeof(CncPlan))
                {
                    var serializedPlan = JsonConvert.SerializeObject(plan);
                    CncPlan cncPlan = JsonConvert.DeserializeObject<CncPlan>(serializedPlan);
                    int cfcId = cncPlan.ConcreteFurnitureComponentId ?? default(int);

                    var cncCodeAsText = await GenerateCncCodeForPlan(cfcId, cncMachines);
                    cncPlan.CncCode = cncCodeAsText;

                    await planRepository.InsertAsync(cncPlan);
                    optimization.AddPlan(cncPlan);

                    var productionProcess = new ProductionProcess(planKvp.Value, cncPlan.Id);
                    await productionProcessRepository.InsertAsync(productionProcess);

                    if (!orderIds.Contains(planKvp.Value))
                    {
                        orderIds.Add(planKvp.Value);
                    }
                }
                else
                {
                    await planRepository.InsertAsync(plan);
                    optimization.AddPlan(plan);

                    var productionProcess = new ProductionProcess(planKvp.Value, plan.Id);
                    await productionProcessRepository.InsertAsync(productionProcess);

                    if (!orderIds.Contains(planKvp.Value))
                    {
                        orderIds.Add(planKvp.Value);
                    }
                }
            }

            foreach (var orderId in orderIds)
            {
                var order = await orderRepository.SingleAsync(ent => ent.Id == orderId);
                order.SetScheduledState(userId);
            }

            await optimizationRepository.InsertAsync(optimization);

            await unitOfWork.SaveChangesAsync();
        }

        private PlanCreateDto ReadFromJson(string file)
        {
            var planCreateDto = new PlanCreateDto();
            using (StreamReader reader = new StreamReader(file))
            {
                string json = reader.ReadToEnd();
                planCreateDto = JsonConvert.DeserializeObject<PlanCreateDto>(json);
            }
            return planCreateDto;
        }

        private FileInfo GetNewestHtmlFile(DirectoryInfo directory)
        {
            return directory.GetFiles()
                .Where(ent => ent.Name.EndsWith(".html"))
                .OrderByDescending(ent => (ent == null ? DateTime.MinValue : ent.LastWriteTime))
                .First();
        }

        private async Task<string> GenerateCncCodeForPlan(int cfcId, List<CncMachine> machines)
        {
            var cfc = await concreteFurnitureComponentRepository.SingleAsync(ent => ent.Id == cfcId);
            var fc = await furnitureComponentRepository.SingleIncludingAsync(ent => ent.Id == cfc.FurnitureComponentId, ent => ent.Sequences);

            // here you can modify the type of CNC code generation
            // the default is ISO G code
            var cncCode = await cncCodeGenerationService.GenerateIsoGCode(cfc.FurnitureComponent);

            await SendCodeToMachines(machines, cncCode, fc.Name);

            return cncCode;
        }

        public async Task<byte[]> GetLayoutZipAsBytesAsync(Guid optimizationId)
        {
            var opt = await optimizationRepository.SingleIncludingAsync(ent => ent.Id == optimizationId, ent => ent.LayoutPlanZip);
            string fullZipPath = fileHandlerService.GetFileUrl(opt.LayoutPlanZip.ContainerName, opt.LayoutPlanZip.FileName);
            var zipAsBytes = System.IO.File.ReadAllBytes(fullZipPath);
            return zipAsBytes;
        }

        public async Task<byte[]> GetScheduleZipAsBytesAsync(Guid optimizationid)
        {
            var opt = await optimizationRepository.SingleIncludingAsync(ent => ent.Id == optimizationid, ent => ent.ScheduleZip);
            string fullZipPath = fileHandlerService.GetFileUrl(opt.ScheduleZip.ContainerName, opt.ScheduleZip.FileName);
            var zipAsBytes = System.IO.File.ReadAllBytes(fullZipPath);
            return zipAsBytes;
        }

        private async Task SendCodeToMachines(List<CncMachine> machines, string code, string componentName)
        {
            foreach (var machine in machines)
            {
                if(Directory.Exists(machine.SharedFolderPath))
                {
                    var fileName = Path.Combine(machine.SharedFolderPath, componentName + ".txt");
                    await System.IO.File.WriteAllTextAsync(fileName, code);
                }
                else
                {
                    throw new IFPSAppException("Could not find shared folder!");
                }
            }
        }
    }
}
