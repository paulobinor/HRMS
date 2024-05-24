namespace hrms_be_backend_data.RepoPayload
{
    public class ResignationClearanceDTO
    {
        public long ResignationClearanceID { get; set; }
        public long ResignationID { get; set; }
        public long EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Grade { get; set; }
        public string Department { get; set; }
        public long CompanyID { get; set; }
        //public string ReasonForExit { get; set; }
        public string ItemsReturnedToDepartment { get; set; }
        public string ItemsReturnedToAdmin { get; set; }
        public string ItemsReturnedToHr { get; set; }
        public string LoansOutstanding { get; set; }
        public DateTime ExitDate { get; set; }
        public string Signature { get; set; }
        public long CreatedByUserID { get; set; }
        public DateTime DateCreated { get; set; }
        public long HodEmployeeID { get; set; }
        public bool IsHodApproved { get; set; }
        public bool IsHodDisapproved { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public DateTime DateApproved { get; set; }
        public DateTime HodDateApproved { get; set; }
        public DateTime DateDisapproved { get; set; }
        public long DisapproveByUserID { get; set; }
        public string ReasonForDisapproval { get; set; }
        public string PendingOn { get; set; }


    }
}

