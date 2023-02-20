
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using XpressHRMS.Business.Services.ILogic;
using XpressHRMS.Business.Services.Logic;

namespace XpressHRMS.Business
{
    public static class ServiceRegistration
    {
        public static void AddBussinessLayer(this IServiceCollection services, IConfiguration _config)
        {
           // services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<ISSOservice, SSOservice>();
            services.AddScoped<IBranchService, BranchService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IGradeService, GradeService>();
            services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IBankService, BankService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<ILgaService, LgaService>();
            services.AddScoped<IHodService, HodService>();
        }
    }
}
