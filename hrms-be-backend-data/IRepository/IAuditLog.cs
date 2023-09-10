using hrms_be_backend_data.RepoPayload;

namespace hrms_be_backend_data.IRepository
{
    public interface IAuditLog
    {
        Task<dynamic> LogActivity(AuditLogDto auditLog);
    }
}
