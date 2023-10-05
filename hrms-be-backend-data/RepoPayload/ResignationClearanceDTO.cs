namespace hrms_be_backend_data.RepoPayload
{
    public class ResignationClearanceDTO
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long SRFID { get; set; }
        public long InterviewID { get; set; }
        public long CompanyID { get; set; }
        public string Created_By_User_Email { get; set; }
        public int ApprovalStatus { get; set; }
        public bool IsApproved { get; set; }
        public long ApprovedByUserId { get; set; }
        public DateTime DateApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public string DisApprovedReason { get; set; }
        public long DisapprovedByUserId { get; set; }
        public DateTime DateDisapproved { get; set; }
        public bool IsDeleted { get; set; }
        public string ItemsReturnedToDepartment { get; set; }
        public long HodUserID { get; set; }
        public bool IsHodApproved { get; set; }
        public DateTime HodDateApproved { get; set; }
        public bool IsHodDeclined { get; set; }
        public string HodDisapprovedComment { get; set; }
        public DateTime HodDateDisapproved { get; set; }
        public string ItemsReturnedToAdmin { get; set; }
        public long ApprovedByAdminUserID { get; set; }
        public bool IsAdminApproved { get; set; }
        public DateTime AdminDateApproved { get; set; }
        public bool IsAdminDeclined { get; set; }
        public string Loans { get; set; }
        public string Collateral { get; set; }
        public long ApprovedByFinanceUserID { get; set; }
        public bool IsFinanceApproved { get; set; }
        public DateTime FinanceDateApproved { get; set; }
        public bool IsFinanceDeclined { get; set; }
        public long ApprovedByRiskUserID { get; set; }
        public bool IsRiskApproved { get; set; }
        public DateTime RiskDateApproved { get; set; }
        public bool IsRiskDeclined { get; set; }
        public long ApprovedByCIOUserID { get; set; }
        public bool IsCIOApproved { get; set; }
        public DateTime CIODateApproved { get; set; }
        public string CIODisapprovedComment { get; set; }
        public string ItemsReturnedToHR { get; set; }
        public long ApprovedByHrUserID { get; set; }
        public bool IsHrApproved { get; set; }
        public DateTime HrDateApproved { get; set; }
        public bool IsHrDeclined { get; set; }
        public string HrDisapprovedComment { get; set; }
        public DateTime LastDayOfWork { get; set; }
        public string Signature { get; set; }
        public bool IsNotified { get; set; }

    }
}

