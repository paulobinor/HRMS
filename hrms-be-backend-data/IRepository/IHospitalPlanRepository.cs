using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IHospitalPlanRepository
    {
        Task<IEnumerable<HospitalPlanDTO>> GetAllHospitalPlan();
        Task<HospitalPlanDTO> GetHospitalPlanByName(string HospitalPlan);
    }
}
