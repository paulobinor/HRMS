using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface IEmployeeService
    {
        Task<BaseResponse<CreateEmployeeDTO>> CreateEmployee(CreateEmployeeDTO payload);
    }
}