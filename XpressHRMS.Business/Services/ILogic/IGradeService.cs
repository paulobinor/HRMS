using System;
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
        Task<BaseResponse<CreateGradeDTO>> CreateGrade(CreateGradeDTO createGrade, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<UpdateGradeDTO>> UpdateGrade(UpdateGradeDTO UpdateGrade, string RemoteIpAddress, string RemotePort);
        Task<BaseResponse<DelGradeDTO>> DeleteGrade(DelGradeDTO DelGrade, string RemoteIpAddress, string RemotePort);
        //Task<BaseResponse> DisableGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort);
        //Task<BaseResponse> ActivateGrade(int GradeID, int CompanyID, string RemoteIpAddress, string RemotePort);

        Task<BaseResponse<List<GradeDTO>>>GetAllGrade(int CompanyID);

        Task<BaseResponse<GradeDTO>> GetGradeByID(int CompanyID, int GradeID);
    }
}
