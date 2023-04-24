
using AutoMapper;
using Com.XpressPayments.Bussiness.Services.ILogic;
using Com.XpressPayments.Bussiness.Services.Logic;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Com.XpressPayments.Bussiness
{
    public static class ServiceRegistration
    {
        public static void AddBussinessLayer(this IServiceCollection services, IConfiguration _config)
        {
            //register services here

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddHttpClient();

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IHODService, HODService>();
            services.AddScoped<ICountyService, CountyService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ILgaService, LgaService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IUnitHeadService, UnitHeadService>();
            services.AddScoped<IJobDescriptionService, JobDescriptionService>();
            services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IGenderService, GenderService>();
            services.AddScoped<IMaritalStatusService, MaritalStatusService>();
            services.AddScoped<IInstitutionsService, InstitutionsService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IEmpLocationService, EmpLocationService>();
            services.AddScoped<IEmploymentStatusService, EmploymentStatusService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<IHMOService, HMOService>();
        }
    }
}
