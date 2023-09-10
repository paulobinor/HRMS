using hrms_be_backend_data.RepoPayload;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationInterviewRepository
    {
        Task<int> CreateResignationInterview(ResignationInterviewDTO resignation, DataTable sectionOne, DataTable sectionTwo);
        Task<List<InterviewScaleDetailsDTO>> GetInterviewScaleDetails();
        Task<ResignationInterviewDTO> GetResignationInterview(long SRFID);
        Task<List<InterviewScaleValue>> GetResignationInterviewDetails(long InterviewID);
        Task<int> ApprovePendingResignationInterview(long userID, long InterviewID, bool isApproved);
        Task<List<ResignationInterviewDTO>> GetAllApprovedResignationInterview(long UserID, bool isApproved);
        Task<int> DisapprovePendingResignationInterview(long userID, long InterviewID, bool isDisapproved, string DisapprovedComment);
    }
}
