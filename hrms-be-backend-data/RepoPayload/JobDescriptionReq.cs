namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessJobDescriptionReq
    {
        public long JobDescriptionId { get; set; }
        public string JobDescriptionName { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
    public class DeleteJobDescriptionReq
    {
        public long JobDescriptionId { get; set; }
        public string Comment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
