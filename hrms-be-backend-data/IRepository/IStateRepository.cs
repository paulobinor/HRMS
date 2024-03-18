using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_data.IRepository
{
    public  interface IStateRepository
    {
        Task<List<StateVm>> GetStates(int CountryId);
    }
}
