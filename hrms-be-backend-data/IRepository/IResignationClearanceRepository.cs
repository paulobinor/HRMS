using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IResignationClearanceRepository
    {
        Task<int> CreateResignationClearance(ResignationClearanceDTO resignation);
    }
}
