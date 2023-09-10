using AutoMapper;
using hrms_be_backend_data.RepoPayload;
using hrms_be_backend_data.ViewModel;

namespace hrms_be_backend_business.Profiles
{
    public class HRMSProfile : Profile
    {
        public HRMSProfile()
        {
            //Data source to Target
            CreateMap<User, UserViewModel>();

        }
    }
}
