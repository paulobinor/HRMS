using hrms_be_backend_common.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
            if (t == null || t.Count == 0)
            {
                return new PagedListModel<T>();
            }
           
            var pagedRes = new PagedListModel<T>
            {
                TotalItems = t.Count,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(t.Count / (double)pageSize),
                Items = t.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList()
            };
            return pagedRes;
        }
    }
}
