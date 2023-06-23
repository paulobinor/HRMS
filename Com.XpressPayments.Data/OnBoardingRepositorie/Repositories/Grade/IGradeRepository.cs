using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.Grade
{
    public interface IGradeRepository
    {
        Task<dynamic> CreateGrade(CreateGradeDTO create, string createdbyUserEmail);
        Task<dynamic> UpdateGrade(UpdateGradeDTO update, string updatedbyUserEmail);
        Task<dynamic> DeleteGrade(DeleteGradeDTO delete, string deletedbyUserEmail);
        Task<IEnumerable<GradeDTO>> GetAllActiveGrade();
        Task<IEnumerable<GradeDTO>> GetAllGrade();
        Task<GradeDTO> GetGradeById(long GradeID);
        Task<GradeDTO> GetGradeByName(string GradeName);
        Task<GradeDTO> GetGradeByCompany(string GradeName, int companyId);
        Task<IEnumerable<GradeDTO>> GetAllGradeCompanyId(long CompanyId);




    }
}
