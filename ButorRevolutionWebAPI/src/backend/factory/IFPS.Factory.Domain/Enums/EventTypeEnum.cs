namespace IFPS.Factory.Domain.Enums
{
    public enum EventTypeEnum
    {
        None = 0,

        NewAppointment = 1,
        AppointmentReminder = 2,
        NewFilesUploaded = 3,
        NewMessages = 4,
        ChangedOrderState = 5,
        OrderEvaluation = 6,

        Other = 1000
    }
}
