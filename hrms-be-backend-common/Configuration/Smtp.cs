namespace hrms_be_backend_common.Configuration
{
    public class Smtp
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Sender { get; set; }
        public string DisplayName { get; set; }
        public bool SSL { get; set; }
    }
}
