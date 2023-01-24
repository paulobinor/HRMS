using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IEmployeeRepository
    {
        Task<dynamic> CreateEmployee(CreateEmployeeDTO payload);
    }
}