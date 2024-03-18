namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessBranchReq
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public bool IsHeadQuater { get; set; }
        public int LgaId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; } 
    }
    public class DeleteBranchReq
    {
        public long BranchId { get; set; }
        public string Comment { get; set; }       
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }       
    }
}
