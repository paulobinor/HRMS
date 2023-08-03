using Com.XpressPayments.Data.ExitModuleRepository.Repositories;
using Com.XpressPayments.Data.LeaveModuleRepository.LeaveRequestRepo;
using Com.XpressPayments.Data.LeaveModuleRepository.LeaveType;
using Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.Reviwer;
using Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.ReviwerRole;
using Com.XpressPayments.Data.OnBoardingRepositorie.Repositories.UserAccount.IRepository;
using Com.XpressPayments.Data.Repositories;
using Com.XpressPayments.Data.Repositories.Branch;
using Com.XpressPayments.Data.Repositories.Children;
using Com.XpressPayments.Data.Repositories.Company.IRepository;
using Com.XpressPayments.Data.Repositories.Company.Repository;
using Com.XpressPayments.Data.Repositories.CountryStateLga;
using Com.XpressPayments.Data.Repositories.Departments.IRepository;
using Com.XpressPayments.Data.Repositories.Departments.Repository;
using Com.XpressPayments.Data.Repositories.EmpLocation;
using Com.XpressPayments.Data.Repositories.Employee;
using Com.XpressPayments.Data.Repositories.EmployeeType;
using Com.XpressPayments.Data.Repositories.EmploymentStatus;
using Com.XpressPayments.Data.Repositories.Gender;
using Com.XpressPayments.Data.Repositories.Grade;
using Com.XpressPayments.Data.Repositories.Group;
using Com.XpressPayments.Data.Repositories.HMO;
using Com.XpressPayments.Data.Repositories.HOD;
using Com.XpressPayments.Data.Repositories.HospitalPlan;
using Com.XpressPayments.Data.Repositories.HospitalProviders;
using Com.XpressPayments.Data.Repositories.Institutions;
using Com.XpressPayments.Data.Repositories.JobDescription;
using Com.XpressPayments.Data.Repositories.MaritalStatus;
using Com.XpressPayments.Data.Repositories.Position;
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
            services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
            services.AddScoped<IGradeRepository, GradeRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IGenderRepository, GenderRepository>();
            services.AddScoped<IMaritalStatusReposiorty, MaritalStatusReposiorty>();
            services.AddScoped<IInstitutionsRepository, InstitutionsRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmpLocationRepository, EmpLocationRepository>();
            services.AddScoped<IEmploymentStatusRepository, EmploymentStatusRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IHMORepository, HMORepository>();
            services.AddScoped<IHospitalProvidersRepository, HospitalProvidersRepository>();
            services.AddScoped<IHospitalPlanRepository, HospitalPlanRepository>();
            services.AddScoped<IChildrenRepository, ChildrenRepository>();
            services.AddScoped<IRolesRepo, RolesRepo>();
            services.AddScoped<IReviwerRepository, ReviwerRepository>();
            services.AddScoped<IReviwerRoleRepository, ReviwerRoleRepository>();

            //VacationModul
            services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
            services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
            services.AddScoped<IRescheduleLeaveRepository, RescheduleLeaveRepository>();

            //ExitModule
            services.AddScoped<IResignationRepository, ResignationRepository>();
        }
    }
}
