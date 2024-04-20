using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.CompilerServices;

namespace ConsoleApp01.SQL
{
    public class ExecuteStoredProcedure
    {
        public static void RunQuery()
        {
            string connectionString = "Data Source=34.106.203.228;Initial Catalog=flowdatabase-01;User ID=sqlserver;Password=FlowsqlDatabase@1234";

            string createTableQuery = @"
                CREATE TABLE YourTableName (
                    ID INT PRIMARY KEY,
                    Name NVARCHAR(50),
                    Age INT
                )";
            try
            {
                // Create a SqlConnection object
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand object with the create table query
                    using (SqlCommand command = new SqlCommand(createTableQuery, connection))
                    {
                        // Execute the SQL command to create the table
                        command.ExecuteNonQuery();
                        Console.WriteLine("Table created successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        private static void WriteDatasetToTextFile()
        {
            DataSet ds = new DataSet();
            using (SqlConnection connection = new SqlConnection("Server=xxx.database.windows.net,1433;Database=MyDatabase;User ID=xxx;Password=xxx;Encrypt=False;TrustServerCertificate=False;Connection Timeout=30;"))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("[UspGetGuidanceSearchResults]", connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@memberFirmCategoryElementId", 39272);
                    command.Parameters.AddWithValue("@languageid", 1033);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);

                    adapter.Fill(ds);
                    WriteDatasetToTextFile(ds);
                }
            }
        }

        private static void WriteDatasetToTextFile(DataSet ds)
        {
            StringBuilder content = new StringBuilder();

            foreach (DataTable tbl in ds.Tables)
            {
                content.AppendLine(tbl.TableName);
                foreach (DataColumn col in tbl.Columns)
                {
                    content.Append($"{col.ColumnName}|");
                }
                content.AppendLine();
                foreach (DataRow row in tbl.Rows)
                {
                    foreach (DataColumn col in tbl.Columns)
                    {
                        content.Append($"{row[col]}|");
                    }
                    content.AppendLine();
                }
            }
            System.IO.File.WriteAllText("D:\\Temp\\QueryOutput.txt", content.ToString());
        }
    }
}
