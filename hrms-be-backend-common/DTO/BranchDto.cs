namespace hrms_be_backend_common.DTO
{
    public class CreateBranchDto
    {       
        public string BranchName { get; set; }
        public string Address { get; set; }
        public bool IsHeadQuater { get; set; }
        public int LgaId { get; set; }
    }
    public class UpdateBranchDto
    {
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public bool IsHeadQuater { get; set; }
        public int LgaId { get; set; }
    }
    public class DeleteBranchDto
    {
        public long BranchId { get; set; }
        public string Comment { get; set; }      
    }
}
