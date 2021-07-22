using IFPS.Factory.Domain.Model;
using System.Linq;

namespace IFPS.Factory.Application.Dto
{
    public class BaseWorkStationTypeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }

        public BaseWorkStationTypeListDto(WorkStation workStation)
        {
            
        }

        protected Plan GetLastPlan(WorkStation ws)
        {
            var resultPlan = new Plan();
            for (int i = 0; i < ws.Plans.Count(); i++)
            {
                var current = ws.Plans.ElementAt(i);
                var maxDate = current.ScheduledEndTime;
                resultPlan = current;
                if ((maxDate.CompareTo(current.ScheduledEndTime)) < 0)
                {
                    maxDate = current.ScheduledEndTime;
                    resultPlan = current;
                }
            }

            return resultPlan;
        }
    }
}
