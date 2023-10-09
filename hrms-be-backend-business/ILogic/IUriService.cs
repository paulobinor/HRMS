using hrms_be_backend_common.Communication;

namespace hrms_be_backend_business.ILogic
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
