﻿using Com.XpressPayments.Common.ViewModels;
using hrms_be_backend_data.RepoPayload;
using Microsoft.AspNetCore.Http;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationRepository
    {
        Task<dynamic> CreateResignation(ResignationDTO request);
        Task<dynamic> UpdateResignation(UpdateResignationDTO resignation);
        Task<ResignationDTO> GetResignationByID(long ID);
        Task<ResignationDTO> GetResignationByEmployeeID(long EmployeeID);
        Task<IEnumerable<ResignationDTO>> GetResignationByCompanyID(long companyID, int PageNumber, int RowsOfPage, string SearchVal);
       // Task<IEnumerable<ResignationDTO>> GetAllResignations();

        //Task<dynamic> DeleteResignation(long ID, string deletedBy, string deleteReason);
        Task<IEnumerable<ResignationDTO>> GetPendingResignationByEmployeeID(long userID);
        Task<string> ApprovePendingResignation(long EmployeeID, long ResignationId);
        Task<string> DisapprovePendingResignation(long EmployeeID, long ResignationId, string reason);
        

    }
}
