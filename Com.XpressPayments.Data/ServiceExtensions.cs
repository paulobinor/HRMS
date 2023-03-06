using Com.XpressPayments.Data.Repositories.Branch;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Company.Repository;
using Com.XpressPayments.Data.Repositories.CountryStateLga;
using Com.XpressPayments.Data.Repositories.Departments.IRepository;
using Com.XpressPayments.Data.Repositories.Departments.Repository;
using Com.XpressPayments.Data.Repositories.HOD;
using Com.XpressPayments.Data.Repositories.JobDescription;
using Com.XpressPayments.Data.Repositories.Unit;
using Com.XpressPayments.Data.Repositories.UnitHead;
using Com.XpressPayments.Data.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.Repositories.UserAccount.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Com.XpressPayments.Data
{
    public static class ServiceExtensions
    {
        public static void AddDataLayer(this IServiceCollection services)
        {
            //register repo here
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAuditLog, AuditLogRepository>();
            services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddTransient<IJwtManager, JwtManager>();
            services.AddTransient<ITokenRefresher, TokenRefresher>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IHODRepository, HODRepository>();
            services.AddScoped<ICountryRepository, CountryRepository>();
            services.AddScoped<IStateRepository, StateRepository>();
            services.AddScoped<ILgaRepository, LgaRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IUnitHeadRepository, UnitHeadRepository>();
            services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
        }
    }
}
