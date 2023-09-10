using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public  interface IGenderRepository
    {
        Task<IEnumerable<GenderDTO>> GetAllGender();
    }
}
