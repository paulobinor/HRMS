using Com.XpressPayments.Data.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Data.Repositories.EmpLocation
{
    public interface IEmpLocationRepository
    {
        Task<dynamic> CreateEmpLocation(CreateEmpLocationDTO emplocation, string createdbyUserEmail);
        Task<dynamic> UpdateEmpLocation(UpdateEmpLocationDTO Update, string updatedbyUserEmail);
        Task<dynamic> DeleteEmLocation(DeleteEmpLocationDTO DelEmpLocation, string deletedbyUserEmail);
        Task<IEnumerable<EmpLocationDTO>> GetAllActiveEmLocation();
        Task<IEnumerable<EmpLocationDTO>> GetAllEmpLocation();
        Task<EmpLocationDTO> GetEmpLocationById(long EmpLocationID);
        Task<EmpLocationDTO> GetEmpLocationByName(string LocationAddress);
        Task<IEnumerable<EmpLocationDTO>> GetAllEmpLocationCompanyId(long companyId);

    }
}
