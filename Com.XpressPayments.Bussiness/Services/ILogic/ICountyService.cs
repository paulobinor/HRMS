﻿using Com.XpressPayments.Data.GenericResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.XpressPayments.Bussiness.Services.ILogic
{
    public  interface ICountyService
    {
        Task<BaseResponse> GetAllCountry(RequesterInfo requester);
    }
}
