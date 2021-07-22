namespace IFPS.Factory.Domain.Model
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SenderName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public string RegisterSubject { get; set; }
        public string ResetSubject { get; set; }
        public int MaximumTrials { get; set; }
        public int HangfireTimeInterval { get; set; }
        public bool StoreEmailsInDatabase { get; set; }
    }
}
