namespace hrms_be_backend_data.RepoPayload
{
    public class CurrencyReq
    {
        public long CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyLogo { get; set; }
        public bool IsActive { get; set; }
        public bool IsModifield { get; set; }
    }
}
