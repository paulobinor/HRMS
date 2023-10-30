namespace hrms_be_backend_data.RepoPayload
{
    public class UserAppModulePrivilegesReq
    {
        public long UserId { get; set; }
        public long AppModulePrivilegeId { get; set; }
        public long CreatedByUserId { get; set;}
        public DateTime DateCreated { get;set;}
    }
}
