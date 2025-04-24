using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace View
{
    public static class DataTableHelper
    {
        public static DataTable ToDataTable<T>(List<T> data)
        {
            DataTable table = new DataTable();

            if (data == null || !data.Any())
                return table;

            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyInfo prop in properties)
                {
                    object value = prop.GetValue(item, null);
                    row[prop.Name] = value ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}