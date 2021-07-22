using IFPS.Factory.Domain.Model;
using IFPS.Factory.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace DataGeneration
{
    class Program
    {

        static void Main(string[] args)
        {
            var program = new Program();
            string connectionString = "Server=.\\SQLEXPRESS;Database=IFPSFactoryDb;Trusted_Connection=True;";

            string usecase = "layout";

            switch (usecase)
            {
                // random data generation
                case "generate":
                    program.GenerateLayoutPlanningData(connectionString, 3, 3, 3, 1);
                    break;

                // output data for certain orders to the layout planning module in json
                case "json":
                    List<Guid> orderIds = new List<Guid>() { new Guid("d116f5ca-58af-47a4-92a2-1fb2abc31f51"), new Guid("409f08ee-7fad-43d2-a6cb-e0ed274d9cb9"), new Guid("0d9f83dc-9143-49e5-9dd7-8dc702f130cb") };
                    program.WriteToJsonFile(connectionString, orderIds);
                    break;

                case "layout":

                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.FileName = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/LayoutPlanning/LayoutPlanningCP/x64/Release/LayoutPlanningCP.exe";

                    // input json
                    string a1 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/LayoutPlanning/LayoutPlanningCP/LayoutPlanningCP/data/new/input.json";
                    ///string a1 = "C:/Users/modli.bendeguz/Downloads/input.json";

                    // strategy
                    string a2 = "BY_AREA_DESC";

                    // settings json
                    string a3 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/LayoutPlanning/LayoutPlanningCP/LayoutPlanningCP/settings.json";

                    // output dir
                    string a4 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/LayoutPlanning/LayoutPlanningCP/LayoutPlanningCP/output";

                    startInfo.Arguments = a1 + " " + a2 + " " + a3 + " " + a4;

                    using (Process p = Process.Start(startInfo))
                    {
                        p.WaitForExit();
                        Console.WriteLine("\nRETURNED VALUE: " + p.ExitCode);
                    }
     
                    break;

                case "jobs":

                    ProcessStartInfo jobStartInfo = new ProcessStartInfo();
                    jobStartInfo.FileName = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/JobScheduling/x64/Release/JobScheduling.exe";

                    // input json
                    ///string arg1 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/JobScheduling/data/sample_reschedule_data.json";
                    ///string arg1 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/JobScheduling/data/sample_data.json";
                    string arg1 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/LayoutPlanning/LayoutPlanningCP/LayoutPlanningCP/data/new/input.json";

                    // cuttings json
                    string arg2 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/LayoutPlanning/LayoutPlanningCP/LayoutPlanningCP/output/cuttings.json";
                    ///string arg2 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/JobScheduling/data/sample_cuttings.json";
                    
                    // settings json
                    string arg3 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/JobScheduling/settings.json";
                    
                    // output dir
                    string arg4 = "C:/Users/modli.bendeguz/Documents/enco/butoros/ScheduleOptimisation/JobScheduling/results";
                    
                    // shift number
                    string arg5 = "1";
                    
                    // shift length
                    string arg6 = "8";
                    
                    // mode
                    string arg7 = "MODE_RESCHEDULE";
                    ///string arg7 = "MODE_ADD_ORDERS";

                    jobStartInfo.Arguments = arg1 + " " + arg2 + " " + arg3 + " " + arg4 + " " + arg5 + " " + arg6 + " " + arg7;

                    using (Process p = Process.Start(jobStartInfo))
                    {
                        p.WaitForExit();
                        Console.WriteLine("\nRETURNED VALUE: " + p.ExitCode);
                    }

                    break;

                default:
                    Console.WriteLine("Invalid usecase!");
                    break;
            }
            
            Console.WriteLine("Finished!");
            Console.Read();
        }

        void WriteToJsonFile(string connStr, List<Guid> orderIds)
        {
            Console.WriteLine("Writing json file...");
            
            var optionsBuilder = new DbContextOptionsBuilder<IFPSFactoryContext>();            
            optionsBuilder.UseSqlServer(connStr);

            using (var context = new IFPSFactoryContext(optionsBuilder.Options))
            {
                // collect relevant data for layout planning
                var orders = context.Orders.Where(o => orderIds.Contains(o.Id)).Select(
                        o => new {
                            o.Id,
                            o.OrderName
                        }
                    ).ToList();

                // do we need the unit infos at all?
                var units = context.ConcreteFurnitureUnits.Where(u => orderIds.Contains(u.OrderId)).Select(
                        u => new {
                            u.Id,
                            u.OrderId
                        }
                    ).ToList();
                List<int> unitIds = context.ConcreteFurnitureUnits.Where(u => orderIds.Contains(u.OrderId)).Select(u => u.Id).ToList();

                var components = context.ConcreteFurnitureComponents.Where(c => unitIds.Contains(c.ConcreteFurnitureUnitId)).Include(c => c.FurnitureComponent).Select(
                        c => new {
                            c.Id,
                            c.ConcreteFurnitureUnitId, //consider removing this too
                            c.ConcreteFurnitureUnit.OrderId,
                            c.FurnitureComponent.Name,
                            c.FurnitureComponent.Width,
                            c.FurnitureComponent.Length,
                            c.FurnitureComponent.BoardMaterialId
                        }
                    ).ToList();

				//TODO only thoes boards that are relevant for the orders
                var boards = context.Materials.OfType<BoardMaterial>().Select(
                        b => new {
                            b.Id,
                            b.Dimension.Width,
                            b.Dimension.Length,
                            b.Dimension.Thickness,
                            b.HasFiberDirection
                        }
                    ).ToList();

                using (StreamWriter fileHandle = System.IO.File.CreateText("../../../output.json"))
                {
                    string ordersJson = "{\"orders\":" + JsonConvert.SerializeObject(orders, Formatting.None) + ",";
                    string unitsJson = "\"units\":" + JsonConvert.SerializeObject(units, Formatting.None) + ",";
                    string componentsJson = "\"components\":" + JsonConvert.SerializeObject(components, Formatting.None) + ",";
                    string boardsJson = "\"boards\":" + JsonConvert.SerializeObject(boards, Formatting.None) + "}";

                    fileHandle.Write(ordersJson + unitsJson + componentsJson + boardsJson);
                }
            }
        }

        void GenerateLayoutPlanningData(string connStr, int orderN, int unitN, int compN, int boardN)
        {
            Console.WriteLine("Data generation started..");

            var optionsBuilder = new DbContextOptionsBuilder<IFPSFactoryContext>();
            optionsBuilder.UseSqlServer(connStr);

            using (var context = new IFPSFactoryContext(optionsBuilder.Options))
            {
                // boards
                var boardList = new List<BoardMaterial>();
                for (var b = 0; b < boardN; b++)
                {
                    var board = CreateBoardMaterial();
                    context.Materials.Add(board);
                    boardList.Add(board);
                }
                context.SaveChanges();
                
                // orders
                var orderList = new List<Order>();
                for (var i = 0; i < orderN; i++)
                {
                    var order = CreateOrder(i);
                    context.Orders.Add(order);
                    orderList.Add(order);
                }
                context.SaveChanges();

                // units
                var cfuList = new List<ConcreteFurnitureUnit>();
                var fuList = new List<FurnitureUnit>();
                foreach (var order in orderList)
                {
                    for (var j = 0; j < unitN; j++)
                    {
                        var fu = CreateFU();
                        var cfu = CreateCFU(order);
                        order.AddCFU(cfu);
                        context.ConcreteFurnitureUnits.Add(cfu);
                        context.FurnitureUnits.Add(fu);
                        cfuList.Add(cfu);
                        fuList.Add(fu);
                    }
                }
                context.SaveChanges();

                // components
                foreach (var order in orderList)
                {
                    for (var j = 0; j < cfuList.Count(); j++)
                    {
                        for (var k = 0; k < compN; k++)
                        {
                            var fc = CreateFC(order.Id, fuList[j].Id, k, boardList, fuList[j]);
                            var cfc = CreateCFC(cfuList[j]);
                            cfc.FurnitureComponent = fc;
                            cfuList[j].AddCFC(cfc);
                            context.FurnitureComponents.Add(fc);
                            context.ConcreteFurnitureComponents.Add(cfc);
                        }
                    }
                }
                context.SaveChanges();                
            }
        }

        Order CreateOrder(int num)
        {
            string name = "Order 0" + num;
            DateTime contractDate = DateTime.Now;
            DateTime deadline = DateTime.Now.AddDays(7);

            var order = new Order(name)
            {
                Deadline = deadline,
                ContractDate = contractDate
            };
            
            return order;
        }

        ConcreteFurnitureUnit CreateCFU(Order order)
        {            
            return new ConcreteFurnitureUnit(order);
        }

        FurnitureUnit CreateFU()
        {
            return new FurnitureUnit("CODE", 0, 0, 0);
        }

        ConcreteFurnitureComponent CreateCFC(ConcreteFurnitureUnit cfu)
        {
            return new ConcreteFurnitureComponent(cfu.Id);
        }

        FurnitureComponent CreateFC(Guid orderNum, Guid unitNum, int compNum, List<BoardMaterial> boards, FurnitureUnit fu)
        {
            Random rng = new Random();

            string name = "Component o" + orderNum + " u" + unitNum + " c" + compNum;
            int width = rng.Next(3, 14) * 100;
            int length = rng.Next(2, 13) * 100;

            int boardIdx = rng.Next(boards.Count);

            var fc = new FurnitureComponent(name,width,-1,1)
            {
                Length = length,
                BoardMaterial = boards[boardIdx],
                CenterPoint = new AbsolutePoint(0,0,0),
                FurnitureUnit = fu
            };

            return fc;
        }

        BoardMaterial CreateBoardMaterial()
        {
            Random rng = new Random();

            var dim = new Dimension(2700, 4000, 20);

            int fiberDir = rng.Next(2);

            var board = new BoardMaterial("asd")
            {
                Dimension = dim,
                HasFiberDirection = (0 == fiberDir)
            };
           
            return board;
        }
    }
}
