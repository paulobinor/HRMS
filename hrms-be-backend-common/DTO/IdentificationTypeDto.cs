namespace hrms_be_backend_common.DTO
{
    public class CreateIdentificationTypeDto
    {
        public string IdentificationTypeName { get; set; }
    }

    public class UpdateIdentificationTypeDto
    {
        public long IdentificationTypeId { get; set; }
        public string IdentificationTypeName { get; set; }
    }
}
