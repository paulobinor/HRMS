namespace hrms_be_backend_common.DTO
{
    public class CreateUnitDto
    {
        public string UnitName { get; set; }
        public long? UnitHeadEmployeeId { get; set; }
    }

    public class UpdateUnitDto
    {
        public long UnitId { get; set; }
        public string UnitName { get; set; }
        public long? UnitHeadEmployeeId { get; set; }
    }

    public class DeleteUnitDto
    {
        public long UnitId { get; set; }
        public string Comment { get; set; }
    }
}
