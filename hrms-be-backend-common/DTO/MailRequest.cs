namespace hrms_be_backend_common.DTO
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailTitle { get; set; }
        public string DisplayName { get; set; }
        public Dictionary<string, FileStream> Attachments { get; set; } = new Dictionary<string, FileStream>();
    }
}
