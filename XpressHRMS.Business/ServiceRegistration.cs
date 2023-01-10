
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
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUnitService, UnitService>();
        }
    }
}
