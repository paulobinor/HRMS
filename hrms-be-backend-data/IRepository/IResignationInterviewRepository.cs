using Com.XpressPayments.Common.ViewModels;
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
        Task<dynamic> CreateResignationInterview(ResignationInterviewDTO resignation, DataTable sectionOne, DataTable sectionTwo);
        Task<ResignationInterviewDTO> GetResignationInterviewById(long ResignationInterviewId);
        Task<ResignationInterviewDTO> GetResignationInterviewByEmployeeID(long EmployeeId);
        Task<List<InterviewScaleValue>> GetResignationInterviewDetails(long InterviewID);
        Task<IEnumerable<ResignationInterviewDTO>> GetAllResignationInterviewsByCompany(long companyID, int PageNumber, int RowsOfPage, string SearchVal);


        //Task<List<InterviewScaleDetailsDTO>> GetInterviewScaleDetails();
        //Task<dynamic> ApprovePendingResignationInterview(long userID, long InterviewID, bool isApproved);
        //Task<dynamic> DisapprovePendingResignationInterview(long userID, long InterviewID, bool isDisapproved, string DisapprovedComment);
    }
}
