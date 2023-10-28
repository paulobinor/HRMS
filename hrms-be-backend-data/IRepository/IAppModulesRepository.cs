using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IAppModulesRepository
    {
        Task<List<AppModulesVm>> GetAppModules();
    }
}
