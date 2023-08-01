using Com.XpressPayments.Data.OnBoardingDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.ReviwerRole
{
    public interface IReviwerRoleRepository
    {
        Task<ReviwerRoleDTO> GetReviwerRoleById(long ReviwerRoleID);
        Task<IEnumerable<ReviwerRoleDTO>> GetAllReviwerRoleCompanyId(long companyId);
        
    }
}
