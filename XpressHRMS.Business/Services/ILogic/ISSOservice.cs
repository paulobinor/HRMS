﻿using System.Threading.Tasks;
using XpressHRMS.Business.GenericResponse;
using XpressHRMS.Data.DTO;

namespace XpressHRMS.Business.Services.ILogic
{
    public interface ISSOservice
    {
        Task<BaseResponse> Login(UserLoginDTO user);
        Task<BaseResponse> LogOut(UserLoginDTO user);
    }
}