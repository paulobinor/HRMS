using FluentValidation;
using hrms_be_backend_common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.Helpers
{
    public class Utilities
    {
        private static IServiceProvider _serviceProvider;

        public Utilities(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public static int CountWeekdays(DateTime startDate, DateTime endDate)
        {
            int count = 0;
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    count++;
                }
                currentDate = currentDate.AddDays(1);
            }

            return count;
        }

        public static T GetServiceProvider<T>() where T : class
        {
            var sp = _serviceProvider.GetService<T>();
            if (sp == null)
            {
                throw new InvalidOperationException($"No service provider found for {typeof(T).Name}");
            }
            return sp;
        }

        public static PagedListModel<T> GetPagedList<T>(List<T> t, int pageNumber = 1, int pageSize = 10) 
        {
            int totalItems = 0; int totalPages = 0;
            List<T> items = null;
            if (t != null && t.Count > 0)
            {

                totalItems = t.Count;
                totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
                items = t
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
            }
            else
            {
                totalItems = 0;
                totalPages = 1;
            }

            var pagedRes = new PagedListModel<T>
            {
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = totalPages,
                Items = items
            };
            return pagedRes;
        }

        //public static object GetPagedList<T>(List<T> finalRes, int pageNumber, int pageSize)
        //{
        //    throw new NotImplementedException();
        //}
    }
    public class ModelValidationService
    {
        private static IServiceProvider _serviceProvider;

        public ModelValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ValidationResultModel Validate<TModel>(TModel model)
        {
            ValidationResultModel validationResultModelDto = new ValidationResultModel();
            var validator = _serviceProvider.GetService<IValidator<TModel>>();
            if (validator == null)
            {
                throw new InvalidOperationException($"No validator found for {typeof(TModel).Name}");
            }

            var res = validator.Validate(model);
            if (!res.IsValid)
            {
                string message = string.Empty;

                foreach (var item in res.Errors)
                {
                    message += string.Join(" | ", $"{item.PropertyName}: {item.ErrorMessage}, ");
                }

                validationResultModelDto.IsValid = false;
                validationResultModelDto.customProblemDetail = new ProblemDetails { Detail = message, Status = (int)HttpStatusCode.BadRequest, Title = "Validation Errors" };

                return validationResultModelDto;
            }
            return validationResultModelDto;
        }

        public static T GetServiceProvider<T>(T model) where T : class
        {
            var sp = _serviceProvider.GetService<T>();
            if (sp == null)
            {
                throw new InvalidOperationException($"No service provider found for {typeof(T).Name}");
            }
            return sp;
        }

        
    }

    public class ValidationResultModel
    {
        public bool IsValid { get; set; } = true;

        public ProblemDetails customProblemDetail { get; set; }
    }

    
}
