using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hrms_be_backend_common.Models
{
    public class PagedListModel<T> 
    {
        public int TotalItems { get; set; } = 0; 
        public int TotalPages { get; set; } = 0;
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public List<T> Items { get; set; } 
    }
}
