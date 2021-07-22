﻿using IFPS.Factory.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class CNCListDto
    {
        public int Id { get; set; }
        public string ComponentName { get; set; }
        public string MaterialCode { get; set; }
        public string OrderName { get; set; }
        public string WorkingNumber { get; set; }
        public DateTime EstimatedStartTime { get; set; }
        public int EstimatedProcessTime { get; set; }
        public int ActualProcessTime { get; set; }
        public List<string> WorkerNames { get; set; }
        public bool IsStarted { get; set; }
        public bool IsShowedButton { get; set; }

        public CNCListDto(ProductionProcess productionProcess)
        {
            Id = productionProcess.Id;
            ComponentName = productionProcess.Plan.ConcreteFurnitureComponent.FurnitureComponent.Name;
            MaterialCode = productionProcess.Plan.ConcreteFurnitureComponent.FurnitureComponent.BoardMaterial.Code;
            OrderName = productionProcess.Order.OrderName;
            WorkingNumber = productionProcess.Order.WorkingNumber;
            EstimatedStartTime = productionProcess.Plan.ScheduledStartTime;
            EstimatedProcessTime = (int)(productionProcess.Plan.ScheduledEndTime - productionProcess.Plan.ScheduledStartTime).TotalSeconds;
            ActualProcessTime = (int)(productionProcess.EndTime.GetValueOrDefault() - productionProcess.StartTime.GetValueOrDefault()).TotalSeconds;
            WorkerNames = productionProcess.Workers.Where(ent => ent.ProcessId == productionProcess.Id).Select(worker => worker.Worker.CurrentVersion.Name).ToList();
            IsStarted = productionProcess.StartTime == null ? IsStarted = false : IsStarted = true;
            IsShowedButton = productionProcess.EndTime == null ? IsShowedButton = true : IsShowedButton = false;
        }

        public static Func<ProductionProcess, CNCListDto> FromEntity = entity => new CNCListDto(entity) { };
    }
}
