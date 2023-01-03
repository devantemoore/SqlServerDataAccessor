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

        public List<T> Get<T>(Guid guid) where T : class, new()
        {
            var tableName = typeof(T).Name.ToLower();
            var commandText = $"SELECT * FROM dbo.{tableName} WHERE id = @Id";
            var param = new SqlParameter("@Id", guid.ToString());

            return SqlHelper.ExecuteReader<T>(ConnectionString, commandText, CommandType.Text, param);
        }
        

        public object Insert<T>(T item)
        {
            var type = typeof(T);
            List<SqlParameter> sqlParams;
            string commandText;
            SqlHelper.SqlCommandStringBuilder(item, type, out sqlParams, out commandText);
            return SqlHelper.ExecuteNonQuery(ConnectionString, commandText, CommandType.Text, sqlParams.ToArray());
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

        public static int ExecuteNonQuery(string connectionString, String commandText, CommandType commandType, params SqlParameter[] sqlParams)
        {
            int result = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = commandText;
                    cmd.CommandType = commandType;
                    if (sqlParams.Any()) cmd.Parameters.AddRange(sqlParams);

                    conn.Open();
                    result = cmd.ExecuteNonQuery(); // closes conn after reader closes                    
                }
            }
            return result;
        }
        public static void SqlCommandStringBuilder<T>(T item, Type type, out List<SqlParameter> sqlParams, out string commandText)
        {
            var tableName = type.Name.ToLower();
            sqlParams = new List<SqlParameter>();
            commandText = $"INSERT INTO {tableName} ";
            var columns = "(";
            var values = "VALUES (";
            foreach (var prop in type.GetProperties())
            {
                columns += $"{prop.Name.FromPascalToSnakeCase()}, ";
                values += $"@{prop.Name.FromPascalToSnakeCase()}, ";
                sqlParams.Add(new SqlParameter($"@{prop.Name.FromPascalToSnakeCase()}", prop.GetValue(item)));
            }
            columns = columns.Remove(columns.Length - 2, 2); // remove last comma+space
            columns += ") ";
            values = values.Remove(values.Length - 2, 2); // remove last comma+space
            values += ")";
            commandText += columns + values;
        }
    }
}