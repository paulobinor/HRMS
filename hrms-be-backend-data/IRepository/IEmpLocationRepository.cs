using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
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
