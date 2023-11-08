using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public interface IIdentificationTypeRepository
    {
        Task<List<IdenticationTypeVm>> GetIdenticationType(long CompanyId);
    }
}
