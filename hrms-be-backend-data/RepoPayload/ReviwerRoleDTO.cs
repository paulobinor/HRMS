using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.RepoPayload.OnBoardingDTO.DTOs
{
    public class ReviwerRoleDTO
    {
        public long ReviwerRoleID { get; set; }
        public string ReviwerRoleName { get; set; }
        public string ReviwerRoleCode { get; set; }
        public long CompanyID { get; set; }
    }
}
