namespace hrms_be_backend_data.RepoPayload
{
    public class DepartmentalModulesReq
    {
        public long DepartmentId { get; set; }
        public long AppModuleId { get; set;}
        public long CreatedByUserId { get; set;}
        public DateTime DateCreated { get;set;}
    }
}
