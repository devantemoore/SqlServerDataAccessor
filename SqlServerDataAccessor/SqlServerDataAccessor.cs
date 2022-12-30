using Microsoft.Data.SqlClient;
using System.Data;

namespace SqlServerDataAccessor
{
    public class SqlServerDataAccessor
    {
        public SqlServerDataAccessor(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public List<T> Get<T>() where T : class, new()
        {
            var tableName = typeof(T).Name;
            var commandText = $"SELECT * FROM dbo.{tableName.ToLower()}";

            return SqlHelper.ExecuteReader<T>(ConnectionString, commandText, CommandType.Text);
            
        }

        

        public object Insert<T>(object task)
        {
            throw new NotImplementedException();
        }

        public object Remove<T>(object guid)
        {
            throw new NotImplementedException();
        }

        public object Update<T>(object task)
        {
            throw new NotImplementedException();
        }

        string ConnectionString;
 }

    internal static class SqlHelper
    {
        public static object[] ExecuteReader(string connectionString, String commandText, CommandType commandType, params SqlParameter[] sqlParams)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    if (sqlParams.Any()) cmd.Parameters.AddRange(sqlParams);

                    conn.Open();
                    var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // closes conn after reader closes
                    while (reader.Read())
                    {



                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var columnName = reader.GetName(i);
                            Console.Write(reader[columnName] + "\t");
                        }
                        Console.WriteLine();
                    }
                }
            }
            throw new NotImplementedException();
        }
        public static List<T> ExecuteReader<T>(string connectionString, String commandText, CommandType commandType, params SqlParameter[] sqlParams) where T : class, new()
        {
            var result = new List<T>();
            Type type = typeof(T);
            var t = Activator.CreateInstance(type);
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    if (sqlParams.Any()) cmd.Parameters.AddRange(sqlParams);

                    conn.Open();
                    var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection); // closes conn after reader closes
                    while (reader.Read())
                    {
                        result.Add(reader.ConvertToObject<T>());
                    }
                }
            }
            return result;
        }
    }
}