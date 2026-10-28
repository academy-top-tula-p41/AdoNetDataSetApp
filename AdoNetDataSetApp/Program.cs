using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Data.SqlClient;
using System.Data;

var config = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .Build();
string? connectionString = config.GetConnectionString("DefaultConnection");

using(SqlConnection connection = new(connectionString))
{
    //
    string sqlQuery = "SELECT * FROM [cities]";

    // создвет адаптер и заполняем набор данных
    SqlDataAdapter adapter = new SqlDataAdapter(sqlQuery, connection);
    DataSet dataSetCities = new DataSet();

    adapter.Fill(dataSetCities);

    // выводим на консоль
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
    Console.WriteLine();

    // ссылка на таблицу городов
    DataTable tableCities = dataSetCities.Tables[0];
    // создание команд INSERT UPDATE DELETE
    SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adapter);
    

    // INSERT
    // ввод названия города
    //string? city;
    //Console.Write("Input city title: ");
    //city = Console.ReadLine();

    // создаем запись для таблицы
    //DataRow rowCity = tableCities.NewRow();
    //rowCity["title"] = city;

    // добавляем эту запись в таблицу
    //tableCities.Rows.Add(rowCity);

    // DELETE
    int id;
    Console.Write("Input id city for delete: ");
    id = Int32.Parse(Console.ReadLine());

    int indexDelete = -1;
    for(int i = 0; i < tableCities.Rows.Count; i++)
        if ((int)tableCities.Rows[i]["id"] == id)
            indexDelete = i;
    if (indexDelete >= 0)
    {
        tableCities.Rows.RemoveAt(indexDelete);
        //tableCities.Rows[indexDelete].Delete();
        dataSetCities.AcceptChanges();
    }

    // обновление таблицы БД
    adapter.UpdateCommand = commandBuilder.GetUpdateCommand();
    adapter.DeleteCommand = commandBuilder.GetDeleteCommand();
    adapter.Update(dataSetCities);

    // очищаем набор данных и заново заполняем
    dataSetCities.Clear();
    adapter.Fill(dataSetCities);

    // вывод на консоль
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