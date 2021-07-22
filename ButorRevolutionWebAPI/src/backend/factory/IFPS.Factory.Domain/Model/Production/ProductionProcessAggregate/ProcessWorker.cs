using ENCO.DDD.Domain.Model.Entities;

namespace IFPS.Factory.Domain.Model
{
    public class ProcessWorker : Entity
    {
        public virtual ProductionProcess Process { get; set; }
        public int ProcessId { get; set; }

        public virtual User Worker { get; set; }
        public int WorkerId { get; set; }

        public ProcessWorker()
        {

        }
    }
}
