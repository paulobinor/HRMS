using AutoMapper;
using Com.XpressPayments.Data.Repositories.UserAccount.Repository;
using hrms_be_backend_business.AppCode;
using hrms_be_backend_business.ILogic;
using hrms_be_backend_business.Logic;
using hrms_be_backend_common.Configuration;
using hrms_be_backend_data.IRepository;
using hrms_be_backend_data.IRepositoryRole;
using hrms_be_backend_data.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var logger = new LoggerConfiguration().WriteTo.Console()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
// Add services to the container.


builder.Services.AddControllers();
builder.Services.AddHttpClient();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{

    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HRMS WebApi",
        Description = "HRMS WebApi Version 1",
        Version = "v1"
    });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme (Example: 'bearer 12345abcdef')",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });



    option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });


    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Auto Mapper Configurations
var mappingConfig = new MapperConfiguration(mc =>
{
    //mc.AddProfile(new HRMSProfile());
});
IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddHttpContextAccessor();

builder.Services.Configure<JwtConfig>(options => builder.Configuration.GetSection("Jwt").Bind(options));
builder.Services.Configure<PassowordConfig>(options => builder.Configuration.GetSection("PassowordConfig").Bind(options));
builder.Services.Configure<DocumentConfig>(options => builder.Configuration.GetSection("DocumentConfig").Bind(options));
builder.Services.Configure<ImageDocumentConfig>(options => builder.Configuration.GetSection("ImageDocumentConfig").Bind(options));
builder.Services.Configure<FrontendConfig>(options => builder.Configuration.GetSection("FrontendConfig").Bind(options));
builder.Services.Configure<ResignationFileConfig>(options => builder.Configuration.GetSection("ResignationFileConfig").Bind(options));
builder.Services.Configure<SmtpConfig>(options => builder.Configuration.GetSection("SmtpConfig").Bind(options));


builder.Services.AddScoped<IDapperGenericRepository, DapperGenericRepository>();

//register repo here
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IAuditLog, AuditLogRepository>();
builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
builder.Services.AddTransient<IJwtManager, JwtManager>();
builder.Services.AddTransient<ITokenRefresher, TokenRefresher>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<ICompanyAppModuleRepository, CompanyAppModuleRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<IDepartmentalModulesRepository, DepartmentalModulesRepository>();
builder.Services.AddScoped<IUserAppModulePrivilegeRepository, UserAppModulePrivilegesRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<ILgaRepository, LgaRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IJobDescriptionRepository, JobDescriptionRepository>();
builder.Services.AddScoped<IEmployeeTypeRepository, EmployeeTypeRepository>();
builder.Services.AddScoped<IGradeRepository, GradeRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();
builder.Services.AddScoped<IGenderRepository, GenderRepository>();
builder.Services.AddScoped<IMaritalStatusReposiorty, MaritalStatusReposiorty>();
builder.Services.AddScoped<IInstitutionsRepository, InstitutionsRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IEmpLocationRepository, EmpLocationRepository>();
builder.Services.AddScoped<IEmploymentStatusRepository, EmploymentStatusRepository>();
builder.Services.AddScoped<IIdentificationTypeRepository, IdentificationTypeRepository>();

builder.Services.AddScoped<IHMORepository, HMORepository>();
builder.Services.AddScoped<IHospitalProvidersRepository, HospitalProvidersRepository>();
builder.Services.AddScoped<IHospitalPlanRepository, HospitalPlanRepository>();
builder.Services.AddScoped<IChildrenRepository, ChildrenRepository>();
builder.Services.AddScoped<IRolesRepo, RolesRepo>();
builder.Services.AddScoped<IReviwerRepository, ReviwerRepository>();
builder.Services.AddScoped<IReviwerRoleRepository, ReviwerRoleRepository>();
builder.Services.AddScoped<ICurrencyRepository, CurrencyRepository>();
builder.Services.AddScoped<IEarningsRepository, EarningsRepository>();
builder.Services.AddScoped<IDeductionsRepository, DeductionsRepository>();
builder.Services.AddScoped<ITaxRepository, TaxRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();
builder.Services.AddScoped<ITrainingFeedbackFormRepository, TrainingFeedbackFormRepository>();
builder.Services.AddScoped<ITrainingInductionRepository, TrainingInductionRepository>();
builder.Services.AddScoped<ITrainingPlanRepository, TrainingPlanRepository>();
builder.Services.AddScoped<IAppModulesRepository, AppModulesRepository>();

builder.Services.AddScoped<ICurrencyService, CurrencyService>();
builder.Services.AddScoped<IEarningsService, EarningsService>();
builder.Services.AddScoped<IDeductionsService, DeductionsService>();
builder.Services.AddScoped<ITaxService, TaxService>();
builder.Services.AddScoped<IPayrollService, PayrollService>();
builder.Services.AddScoped<IIdentificationTypeService, IdentificationTypeService>();
builder.Services.AddSingleton<IUriService>(o =>
{
    var accessor = o.GetRequiredService<IHttpContextAccessor>();
    var request = accessor.HttpContext.Request;
    var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
    return new UriService(uri);
});

//VacationModul
builder.Services.AddScoped<IGradeLeaveRepo, GradeLeaveRepo>();
builder.Services.AddScoped<ILeaveTypeRepository, LeaveTypeRepository>();
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepository>();
builder.Services.AddScoped<IRescheduleLeaveRepository, RescheduleLeaveRepository>();

//ExitModule
builder.Services.AddScoped<IResignationRepository, ResignationRepository>();
builder.Services.AddScoped<IResignationInterviewRepository, ResignationInterviewRepository>();
builder.Services.AddScoped<IResignationClearanceRepository, ResignationClearanceRepository>();

//OnboardingModule
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ICompanyAppModuleService, CompanyAppModuleService>();
builder.Services.AddScoped<IDepartmentService, DepartmentService>();
builder.Services.AddScoped<IDepartmentalModulesService, DepartmentalModulesService>();
builder.Services.AddScoped<IUserAppModulePrivilegeService, UserAppModulePrivilegesService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<IStateService, StateService>();
builder.Services.AddScoped<ILgaService, LgaService>();
builder.Services.AddScoped<IUnitService, UnitService>();
//builder.Services.AddScoped<IUnitHeadService, UnitHeadService>();
builder.Services.AddScoped<IJobDescriptionService, JobDescriptionService>();
builder.Services.AddScoped<IEmployeeTypeService, EmployeeTypeService>();
builder.Services.AddScoped<IGradeService, GradeService>();
builder.Services.AddScoped<IPositionService, PositionService>();
builder.Services.AddScoped<IGenderService, GenderService>();
builder.Services.AddScoped<IMaritalStatusService, MaritalStatusService>();
builder.Services.AddScoped<IInstitutionsService, InstitutionsService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmpLocationService, EmpLocationService>();
builder.Services.AddScoped<IEmploymentStatusService, EmploymentStatusService>();
builder.Services.AddScoped<IHMOService, HMOService>();
builder.Services.AddScoped<IHospitalProvidersService, HospitalProvidersService>();
builder.Services.AddScoped<IChildrenService, ChildrenService>();
builder.Services.AddScoped<IReviwerRoleService, ReviwerRoleService>();
builder.Services.AddScoped<IReviwerService, ReviwerService>();
builder.Services.AddScoped<ILearningAndDevelopmentMailService, LearningAndDevelopmentMailService>();
builder.Services.AddScoped<ITrainingFeedbackFormService, TrainingFeedbackFormService>();
builder.Services.AddScoped<ITrainingInductionService, TrainingInductionService>();
builder.Services.AddScoped<ITrainingPlanService, TrainingPlanService>();
builder.Services.AddScoped<IUserService, UserService>();

//VacationModul
builder.Services.AddScoped<IGradeLeaveService, GradeLeaveService>();
builder.Services.AddScoped<ILeaveTypeService, LeaveTypeService>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();



//ExitModule
builder.Services.AddScoped<IResignationService, ResignationService>();
builder.Services.AddScoped<IResignationInterviewService, ResignationInterviewService>();
builder.Services.AddScoped<IResignationClearanceService, ResignationClearanceService>();


//Mail Service
builder.Services.AddScoped<IMailService, MailService>();


var app = builder.Build();
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Content-Security-Policy", new StringValues(
                                                            "base-uri 'self';" +
                                                            "block-all-mixed-content;" +
                                                            "child-src 'self';" +
                                                            "connect-src 'self';" +
                                                            "font-src 'self';" +
                                                            "form-action 'none';" +
                                                            "manifest-src 'self';" +
                                                            "media-src 'self';" +
                                                            "object-src 'none';" +
                                                            "allow-scripts 'self';" +
                                                            "upgrade-insecure-requests;" +
                                                            "worker-src 'self';"));
    context.Response.Headers.Add("Referrer-Policy", "no-referrer");
    context.Response.Headers.Add("Permissions-Policy", new StringValues(
                                                        "accelerometer 'none';" +
                                                        "ambient-light-sensor 'none';" +
                                                        "autoplay 'none';" +
                                                        "battery 'none';" +
                                                        "camera 'none';" +
                                                        "display-capture 'none';" +
                                                        "document-domain 'none';" +
                                                        "encrypted-media 'none';" +
                                                        "execution-while-not-rendered 'none';" +
                                                        "execution-while-out-of-viewport 'none';" +
                                                        "gyroscope 'none';" +
                                                        "magnetometer 'none';" +
                                                        "microphone 'none';" +
                                                        "midi 'none';" +
                                                        "navigation-override 'none';" +
                                                        "payment 'none';" +
                                                        "picture-in-picture 'none';" +
                                                        "publickey-credentials-get 'none';" +
                                                        "sync-xhr 'none';" +
                                                        "usb 'none';" +
                                                        "wake-lock 'none';" +
                                                        "xr-spatial-tracking 'none';"));
    await next();

});
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HRMS WebApi");
    c.RoutePrefix = string.Empty;
    c.DocExpansion(DocExpansion.None);
});
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
app.UseCors(b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();



app.Run();
