using ENCO.DDD.Domain.Model.Entities.Auditing;
using System;

namespace IFPS.Factory.Domain.Model
{
  public class Email : FullAuditedAggregateRoot
    {
        public User User { get; set; }
        public int? UserId { get; set; }

        public bool IsSuccess { get; set; }

        public EmailData EmailData { get; set; }
        public int EmailDataId { get; set; }

        public int SendCount { get; set; }

        public DateTime TimeOfSent { get; set; }

        public Email(int? userId, int emailDataId, DateTime timeOfSent, bool isSuccess = true)
        {
            UserId = userId;
            EmailDataId = emailDataId;
            IsSuccess = isSuccess;
            SendCount = 1;
            TimeOfSent = timeOfSent;
            CreationTime = Clock.Now;
        }
    }
}