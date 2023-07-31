﻿
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using XpressHRMS.Data.IRepository;
using XpressHRMS.Data.Repository;
using XpressHRMS.IRepository;

namespace XpressHRMS.Data
{
    public static class ServiceExtensions
    {
        public static void AddDataLayer(this IServiceCollection services)
        {
            //register services here
            //services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IAuditTrailRepository, AuditTrailRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddSingleton<IDapperGeneric, DapperGenericRepository>();
            services.AddSingleton<IPositionRepository, PositionRepository>();
            services.AddSingleton<IGradeRepository, GradeRepository>();
            services.AddSingleton<IEmployeeTypeRepository, EmployeeTypeRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IBankRepository, BankRepository>();
            services.AddScoped<IAdminUserRepo, AdminUserRepo>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();




        }
    }
}