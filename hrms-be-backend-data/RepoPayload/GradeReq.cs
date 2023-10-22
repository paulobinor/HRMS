namespace hrms_be_backend_data.RepoPayload
{
    public class ProcessGradeReq
    {
        public long GradeId { get; set; }
        public string GradeName { get; set; }       
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsModifield { get; set; }
    }
    public class DeleteGradeReq
    {
        public long GradeId { get; set; }
        public string Comment { get; set; }
        public long CreatedByUserId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
