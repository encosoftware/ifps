namespace IFPS.Sales.Domain.Enums
{
    public enum DocumentStateEnum
    {
        None = 0,

        Uploaded = 1,
        WaitingForApproval = 2,
        Approved = 3,
        Declined = 4,
        Empty = 5
    }
}
