using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface IHODRepository
    {
        Task<dynamic> CreateHOD(CreateHodDTO hod, string createdbyUserEmail);
        Task<dynamic> UpdateHOD(UpdateHodDTO hod, string updatedbyUserEmail);
        Task<dynamic> DeleteHOD(DeleteHodDTO hod, string deletedbyUserEmail);
        Task<IEnumerable<HodDTO>> GetAllActiveHODs();
        Task<IEnumerable<HodDTO>> GetAllHOD();
        Task<HodDTO> GetHODById(long HodID);
        Task<HodDTO> GetHODByUserId(long UserId);
        Task<HodDTO> GetHODByName(string HODName);
        Task<HodDTO> GetHODByCompany(long UserId, long companyId);
        Task<IEnumerable<HodDTO>> GetAllHODCompanyId(long companyId);

    }
}
