using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.OnBoardingDTO.DTOs
{
    public class ReviwerDTO
    {
        public long ReviwerID { get; set; }
        public long UserId { get; set; }
        public long ReviwerRoleID { get; set; }
        public long CompanyID { get; set; }
       

        public DateTime Created_Date { get; set; }
        public string Created_By_User_Email { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime? Deleted_Date { get; set; }
        public string Deleted_By_User_Email { get; set; }
        public string Reasons_For_Delete { get; set; }
    }

    public class CreateReviwerDTO
    {
        public long UserId { get; set; }
        public long ReviwerRoleID { get; set; }
        public long CompanyID { get; set; }

        public string Created_By_User_Email { get; set; }
    }

    public class DeleteReviwerDTO
    {
        public long ReviwerID { get; set; }
        public string Reasons_For_Delete { get; set; }
    }
}
