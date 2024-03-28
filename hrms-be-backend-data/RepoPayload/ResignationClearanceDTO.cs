namespace hrms_be_backend_data.RepoPayload
{
    public class ResignationClearanceDTO
    {
        public long ResignationClearanceID { get; set; }
        public long ResignationID { get; set; }
        public long EmployeeID { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string MiddleName { get; set; }
        //public string PreferredName { get; set; }
        public long DepartmentID { get; set; }
        public long CompanyID { get; set; }
        public string ReasonForExit { get; set; }
        public string ItemsReturnedToDepartment { get; set; }
        public string ItemsReturnedToAdmin { get; set; }
        public string ItemsReturnedToHr { get; set; }
        public string LoansOutstanding { get; set; }
        public DateTime ExitDate { get; set; }
        public string Signature { get; set; }
        public long CreatedByUserID { get; set; }
        public DateTime DateCreated { get; set; }


    }
}

