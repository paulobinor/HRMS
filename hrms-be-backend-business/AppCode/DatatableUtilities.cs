using Com.XpressPayments.Common.ViewModels;
using System.Data;

namespace hrms_be_backend_business.AppCode
{
    public static class DatatableUtilities
    {
        public static System.Data.DataTable ConvertSectionListToDataTable(List<InterviewDetailsSection> data)

        {
            System.Data.DataTable table = new System.Data.DataTable();

            table.Columns.Add(new DataColumn()
            {
                ColumnName = "ID",
                DataType = typeof(int)
            });
            table.Columns.Add(new DataColumn()
            {
                ColumnName = "Value",
                DataType = typeof(int)
            });
            table.Columns.Add(new DataColumn()
            {
                ColumnName = "Scale",
                DataType = typeof(string)
            });
            
            foreach (var item in data)
            {
                table.Rows.Add(new object[] { item.ID, item.Value, item.Scale });
            }

            return table;

        }
    }
}
