using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_business.Helpers
{
    public class Utilities
    {
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
    }
}
