﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Data.IRepository
{
    public interface IEmployeeTypeRepository
    {
        Task<int> CreateEmployeeType(CreateEmployeeTypeDTO createEmployeeType);
        Task<int> UpdateEmployeeType(UpdateEmployeeTypeDTO UpdateEmployeeType);
        Task<int> DeleteEmployeeType(DelEmployeeTypeDTO deleteEmployeeType);
        Task<int> DisableEmployeeType(int EmployeeTypeID, int CompanyIDDis);
        Task<int> ActivateEmployeeType(int EmployeeTypeID, int CompanyIDEna);
        Task<IEnumerable<EmployeeTypeDTO>> GetAllEmployeeType(int CompanyID);
        Task<IEnumerable<EmployeeTypeDTO>> GetEmployeeTypeByID(int CompanyID, int EmployeeTypeID);
    }
}
