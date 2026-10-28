using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdoNetDataSetApp
{
    public static class Examples
    {
        public static void DataSetExample(SqlConnection connection)
        {
            string sqlQuery = @"SELECT 
		[e].[name],
		[e].[birth_date],
		[p].[title] AS [position],
		[com].[name] AS [company],
		[c].[title] AS [city],
		[e].[salary]
	FROM [employees] AS [e]
	JOIN [positions] AS [p]
		ON [e].[position_id] = [p].[id]
	JOIN [companies] AS [com]
		ON [e].[company_id] = [com].[id]
	JOIN [cities] AS [c]
		ON [com].[city_id] = [c].[id]";

            SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);
            DataSet dataSetCities = new DataSet();
            adapter.Fill(dataSetCities);

            foreach (DataTable table in dataSetCities.Tables)
            {
                foreach (DataColumn column in table.Columns)
                    Console.Write($"{column.ColumnName}\t");
                Console.WriteLine();

                foreach (DataRow row in table.Rows)
                {
                    var cells = row.ItemArray;
                    foreach (object? cell in cells)
                        Console.Write($"{cell}\t");
                    Console.WriteLine();
                }
            }
        }
    }
}
