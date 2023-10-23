using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface ILgaRepository
    {
        Task<List<LgaVm>> GetLgas(int StateId);
    }
}
