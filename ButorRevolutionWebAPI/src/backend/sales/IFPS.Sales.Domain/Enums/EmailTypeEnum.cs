namespace IFPS.Sales.Domain.Enums
{
    public enum EmailTypeEnum
    {
        None = 0,

        ConfirmEmail = 1,
        ResetPassword = 2,
        ConfirmPurchase = 3,
        ConfirmNewUser = 4,
        DocumentUploadSuccess = 5,
        OfferUploaded = 6,
        ContractSignedState = 7,
        WaitingForContract = 8,
        WaitingForOffer = 9,
        ContractUploaded = 10,

        Other = 1000
    }
}
