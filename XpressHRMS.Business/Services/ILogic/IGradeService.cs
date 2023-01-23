﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IGradeService
    {
        Task<BaseResponse> CreateGrade(CreateGradeDTO createGrade, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> UpdateGrade(UpdateGradeDTO UpdateGrade, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DeleteGrade(DelGradeDTO DelGrade, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> DisableGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> ActivateGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse> GetAllGrade(int CompanyID);
        Task<BaseResponse> GetGradeByID(int CompanyID, int GradeID);
    }
}
