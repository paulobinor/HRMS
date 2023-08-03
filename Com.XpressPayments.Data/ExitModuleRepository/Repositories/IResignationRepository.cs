using Com.XpressPayments.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.ExitModuleRepository.Repositories
{
    public interface IResignationRepository
    {
        Task<int> CreateResignation(ResignationDTO request);
        Task<ResignationDTO> GetResignationByID(long ID);
        Task<ResignationDTO> GetResignationByUserID(long UserID);
        Task<List<ResignationDTO>> GetResignationByCompanyID(long companyID, bool isApproved);
        Task<int> DeleteResignation(long ID, string deletedBy, string deleteReason);
        Task<List<ResignationDTO>> GetPendingResignationByUserID(long userID);
        Task<int> ApprovePendingResignation(long userID, long SRFID);
        Task<int> DisapprovePendingResignation(long userID, long SRFID, string reason);

    }
}
