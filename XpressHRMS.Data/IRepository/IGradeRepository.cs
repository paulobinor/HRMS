using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
   public interface IGradeRepository
    {
        Task<int> CreateGrade(CreateGradeDTO createGrade);
        Task<int> UpdateGrade(UpdateGradeDTO UpdateGrade);
        Task<int> DeleteGrade(DelGradeDTO deleteGrade);
        Task<int> DisableGrade(int GradeID);
        Task<int> ActivateGrade(int GradeID);
        Task<IEnumerable<GradeDTO>> GetAllGrades();
        Task<IEnumerable<GradeDTO>> GetGradeByID(int CompanyID, int GradeID);


    }
}
