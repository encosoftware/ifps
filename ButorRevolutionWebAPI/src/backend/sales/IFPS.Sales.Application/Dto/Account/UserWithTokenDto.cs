namespace IFPS.Sales.Application.Dto
{
    public class UserWithTokenDto
    {
        //public string UserName { get; set; }
        public string Email { get; set; }

        public string RefreshToken { get; set; }
    }
}