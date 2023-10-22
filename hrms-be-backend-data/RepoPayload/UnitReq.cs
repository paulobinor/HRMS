namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessUnitReq
    {
        public long UnitId { get; set; }
        public string UnitName { get; set; }      
        public long? UnitHeadEmployeeId { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
    public class DeleteUnitReq
    {
        public long UnitId { get; set; }
        public string Comment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
