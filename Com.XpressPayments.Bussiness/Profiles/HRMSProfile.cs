using System;
using System.Collections.Generic;
using AutoMapper;
using Com.XpressPayments.Data.DTOs.Account;
using Com.XpressPayments.Bussiness.ViewModels;

namespace Com.XpressPayments.Bussiness.Profiles
{
    public class HRMSProfile : Profile
    {
        public HRMSProfile()
        {
            //Data source to Target
            CreateMap<User, UserViewModel>();

        }
    }
}
