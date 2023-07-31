using Com.XpressPayments.Data.OnBoardingDTO.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.Reviwer
{
    public  interface IReviwerRepository
    {
        Task<dynamic> CreateReviwer(CreateReviwerDTO CreateReviwer, string createdbyUserEmail);
        Task<dynamic> DeleteReviwer(DeleteReviwerDTO del, string deletedbyUserEmail);
        Task<ReviwerDTO> GetReviwerById(long ReviwerID);
        Task<ReviwerDTO> GetReviwerByName(long UserId);
        Task<IEnumerable<ReviwerDTO>> GetAllReviwerCompanyId(long companyId);
        Task<ReviwerDTO> GetReviwerByCompany(long UserId, long companyId);
    }
}
