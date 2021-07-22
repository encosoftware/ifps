using ENCO.DDD.Domain.Model.Entities.Auditing;

namespace IFPS.Sales.Domain.Model
{
    public class AnonymousUserData : FullAuditedAggregateRoot
    {
        /// <summary>
        /// Email of the customer
        /// </summary>
        public string Email { get; private set; }
        /// <summary>
        /// Name of the customer
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Phonenumber of the customer
        /// </summary>
        public string Phone { get; private set; }

        private AnonymousUserData()
        {
        }

        public AnonymousUserData(string email, string name, string phone)
        {
            this.Email = email;
            this.Name = name;
            this.Phone = phone;
        }
    }
}
