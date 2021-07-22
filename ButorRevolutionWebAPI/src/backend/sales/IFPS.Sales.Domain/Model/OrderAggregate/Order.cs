using ENCO.DDD;
using ENCO.DDD.Domain.Model.Entities.Auditing;
using IFPS.Sales.Domain.Events;
using IFPS.Sales.Domain.Events.OrderEvents;
using IFPS.Sales.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IFPS.Sales.Domain.Model
{
    public class Order : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// Name of the order, wich helps the salesperson to identify it. 
        /// By default, it is the AddressValue field of the installment place
        /// </summary>
        public string OrderName { get; set; }

        /// <summary>
        /// Unique value, to easily identify the exact order by the year and the serial number
        /// </summary>
        private static readonly string WorkingNumberPrefix = "MSZ";
        public int WorkingNumberYear { get; set; }
        public int WorkingNumberSerial { get; set; }
        public string WorkingNumber => WorkingNumberYear > 0 && WorkingNumberSerial > 0 ?
            WorkingNumberPrefix + WorkingNumberSerial.ToString().PadLeft(4, '0') + '/' + WorkingNumberYear :
            String.Empty;

        /// <summary>
        /// Long text of aggrement by contract
        /// </summary>
        public string Aggrement { get; set; }

        /// <summary>
        /// Text under agreement text by contract
        /// </summary>
        public string Additional { get; set; }

        /// <summary>
        /// This is the date, when the customer wants the furniture to be installed
        /// </summary>
        public DateTime Deadline { get; set; }

        public Address ShippingAddress { get; set; }

        public CabinetMaterial TopCabinet { get; set; }
        public int? TopCabinetId { get; set; }

        public CabinetMaterial BaseCabinet { get; set; }
        public int? BaseCabinetId { get; set; }

        public CabinetMaterial TallCabinet { get; set; }
        public int? TallCabinetid { get; set; }

        /// <summary>
        /// Salesperson, who is responsible for the project
        /// Nullable only to avoid cascade delete 
        /// </summary>
        public virtual SalesPerson SalesPerson { get; set; }
        public int? SalesPersonId { get; set; }

        /// <summary>
        /// Customer, who requested the 
        /// </summary>
        public virtual Customer Customer { get; set; }
        public int CustomerId { get; set; }

        /// <summary>
        /// Private person or company by offer
        /// </summary>
        public bool IsPrivatePerson { get; set; }

        /// <summary>
        /// Budget by offer form
        /// </summary>
        public Price Budget { get; set; }


        /// <summary>
        /// Description by requires on offer form
        /// </summary>
        public string DescriptionByOffer { get; set; }

        /// <summary>
        /// Date, when the customer signed the contract
        /// </summary>
        public DateTime? ContractDate { get; set; }

        public OfferInformation  OfferInformation { get; set; }
        public int? OfferInformationId { get; set; }

        public OrderPrice FirstPayment { get; set; }
        public int? FirstPaymentId { get; set; }

        public OrderPrice SecondPayment { get; set; }
        public int? SecondPaymentId { get; set; }

        public int CurrentTicketId { get; set; }
        /// <summary>
        /// Current ticket of the order
        /// </summary>
        public Ticket CurrentTicket { get; set; }

        private List<Ticket> _tickets;

        /// <summary>
        /// All tickets of the order
        /// </summary>
        public IEnumerable<Ticket> Tickets => _tickets.AsReadOnly();

        /// <summary>
        /// Private list of documents
        /// </summary>
        private List<DocumentGroup> _documentGroups;

        /// <summary>
        /// Public readonly collection of documents
        /// </summary>
        public IEnumerable<DocumentGroup> DocumentGroups => _documentGroups.AsReadOnly();

        private List<OrderedFurnitureUnit> _orderedFurnitureUnits;
        public IEnumerable<OrderedFurnitureUnit> OrderedFurnitureUnits => _orderedFurnitureUnits.AsReadOnly();

        /// <summary>
        /// Private list of appliance materials
        /// </summary>
        private List<OrderedApplianceMaterial> _orderedApplianceMaterials;
        /// <summary>
        /// Public readonly collection of appliance materials
        /// </summary>
        public IEnumerable<OrderedApplianceMaterial> OrderedApplianceMaterials => _orderedApplianceMaterials.AsReadOnly();

        /// <summary>
        /// Private list of services
        /// </summary>
        private List<OrderedService> _services;
        /// <summary>
        /// Public readonly collection of services
        /// </summary>
        public IEnumerable<OrderedService> Services => _services.AsReadOnly();

        public string IniContent { get; set; }

        public string JsonContent { get; set; }

        private Order()
        {
            Id = Guid.NewGuid();
            _tickets = new List<Ticket>();
            _orderedFurnitureUnits = new List<OrderedFurnitureUnit>();
            _orderedApplianceMaterials = new List<OrderedApplianceMaterial>();
            _services = new List<OrderedService>();
            this.CreationTime = DateTime.Now;
            _documentGroups = new List<DocumentGroup>();
            //ContractDate = Clock.Now; // a statistics oldalon ez alapján is vizsgáljuk a salespersonokat, nem lehet default értéke
        }


        public Order(string orderName, int customerId, int salesPersonId, DateTime deadline, Price budget) : this()
        {
            this.OrderName = orderName;
            this.CustomerId = customerId;
            this.SalesPersonId = salesPersonId;
            this.Deadline = deadline;
            this.Budget = budget;

            AddDomainEvent(new OrderPlacedDomainEvent(this));
        }

        public Order(string orderName, int customerId, int salesPersonId, DateTime deadline, Price budget,
            Address shippingAddress) : this(orderName, customerId, salesPersonId, deadline, budget)
        {
            this.ShippingAddress = shippingAddress;
        }

        public Order(string orderName, int customerId, int salesPersonId,
             DateTime deadline, Price budget, Address shippingAddress, int workingNumberYear, int workingNumberSerial) : this(orderName, customerId, salesPersonId, deadline, budget, shippingAddress)
        {
            this.WorkingNumberYear = workingNumberYear;
            this.WorkingNumberSerial = workingNumberSerial;
        }

        public void AddTicket(Ticket ticket)
        {
            Ensure.NotNull(ticket);

            if (_tickets.Any(ent => ent.ValidTo == null))
            {
                throw new IFPSDomainException("Wrong tickets are archieved");
            }

            if (this.CurrentTicket != null)
            {
                CurrentTicket.Close(this);
                this._tickets.Add(CurrentTicket);
                this.CurrentTicket = null;
            }

            this.CurrentTicket = ticket;
        }

        public void AddOrderedFurnitureUnit(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Ensure.NotNull(orderedFurnitureUnit);
            _orderedFurnitureUnits.Add(orderedFurnitureUnit);
        }

        public void RemoveOrderedFurnitureUnit(OrderedFurnitureUnit orderedFurnitureUnit)
        {
            Ensure.NotNull(orderedFurnitureUnit);
            _orderedFurnitureUnits.Remove(orderedFurnitureUnit);
        }

        public void AddAppliance(OrderedApplianceMaterial appliance)
        {
            Ensure.NotNull(appliance);
            _orderedApplianceMaterials.Add(appliance);
        }

        public void RemoveAppliance(OrderedApplianceMaterial appliance)
        {
            Ensure.NotNull(appliance);
            _orderedApplianceMaterials.Remove(appliance);
        }

        public void AddService(OrderedService orderedService)
        {
            Ensure.NotNull(orderedService);
            _services.Add(orderedService);
        }

        public void RemoveService(OrderedService orderedService)
        {
            Ensure.NotNull(orderedService);
            _services.Remove(orderedService);
        }

        public void ClearOrderedServicesList()
        {
            _services.Clear();
        }

        public void SetWorkingNumber(int year, int serial)
        {
            if (WorkingNumberYear == 0 && WorkingNumberSerial == 0)
            {
                WorkingNumberYear = year;
                WorkingNumberSerial = serial;
            }
            else
            {
                //Test seed conflict 
                //throw new IFPSDomainException("WorkingNumber has already set!");
            }
        }

        public void AddDocumentGroup(DocumentGroup group)
        {
            Ensure.NotNull(group);

            _documentGroups.Add(group);
        }

        public void AddDocumentGroups(List<DocumentGroup> groups)
        {
            if (groups.Any(x => x == null))
            {
                throw new NullReferenceException($"{nameof(DocumentGroup)} can't be null!");
            }

            _documentGroups.AddRange(groups);
        }

        public void SetWaitingForOfferState()
        {
            AddDomainEvent(new OrderCreatedDomainEvent(this));
        }

        public void SetWaitingForOfferStateAfterDeclined()
        {
            AddDomainEvent(new OfferDeclinedDomainEvent(this));
        }

        public void SetWaitingForOfferFeedbackState()
        {
            AddDomainEvent(new OfferUploadedDomainEvent(this));
        }

        public void SetWaitingForOnSiteSurveyAppointmentReservationState()
        {
            AddDomainEvent(new OfferAcceptedDomainEvent(this));
        }

        public void SetWaitingForContractFeedbackState()
        {
            AddDomainEvent(new ContractUploadedDomainEvent(this));
        }

        public void SetWaitingForContractStateAfterDeclined()
        {
            AddDomainEvent(new ContractDeclinedDomainEvent(this));
        }

        public void SetUnderProductionState()
        {
            AddDomainEvent(new ContractAcceptedDomainEvent(this));
        }

        public void SetWaitingForShippingState(int partnerId)
        {
            AddDomainEvent(new ShippingAppointmentCreatedDomainEvent(this, partnerId));
        }

        public void SetWaitingForInstallationState(int partnerId)
        {
            AddDomainEvent(new InstallationAppointmentCreatedDomainEvent(this, partnerId));
        }

        public void SetWaitingForOnSiteSurveyState(int partnerId)
        {
            AddDomainEvent(new OnSiteSurveyAppointmentCreatedDomainEvent(this, partnerId));
        }

        public void SetWaitingForContractState(int partnerId)
        {
            AddDomainEvent(new OnSiteSurveyDoneDomainEvent(this, partnerId));
        }

        public void SetWaitingForShippingAppointmentReservationState(int partnerId)
        {
            AddDomainEvent(new WaitingForShippingAppointmentReservationDomainEvent(this, partnerId));
        }

        public void SetDeliverdState(int partnerId)
        {
            AddDomainEvent(new DeliveredDomainEvent(this, partnerId));
        }
    }
}
