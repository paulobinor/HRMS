namespace hrms_be_backend_common.DTO
{
    public class CreateGradeDto
    {
        public string GradeName { get; set; }       
    }

    public class UpdateGradeDto
    {
        public long GradeId { get; set; }
        public string GradeName { get; set; }       
    }

    public class DeleteGradeDto
    {
        public long GradeId { get; set; }
        public string Comment { get; set; }
    }
}
