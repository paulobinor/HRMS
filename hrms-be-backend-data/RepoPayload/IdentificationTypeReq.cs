namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessIdentificationTypeReq
    {
        public long IdentificationTypeId { get; set; }
        public string IdentificationTypeName { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
}
