namespace hrms_be_backend_common.DTO
{
    public class CreateJobDescriptionDto
    {
        public string JobDescriptionName { get; set; }
    }

    public class UpdateJobDescriptionDto
    {
        public long JobDescriptionId { get; set; }
        public string JobDescriptionName { get; set; }
    }

    public class DeleteJobDescriptionDto
    {
        public long JobDescriptionId { get; set; }
        public string Comment { get; set; }
    }
}
