using System.Data;
using System.Data.SqlClient;

namespace Dashboard.Database
{
    public class Executer
    {
        private SqlConnection? _Connection;
        private SqlCommand? _Command;

        private static IConfiguration? _Configuration;

        private string GetConnectionString()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            _Configuration = builder.Build();
            return _Configuration.GetConnectionString("DbConnection");
        }

        public DataSet GetDataset(string query)
        {
            DataSet ds = new DataSet();
            try
            {
                using (_Connection = new SqlConnection(GetConnectionString()))
                {
                    using (var da = new SqlDataAdapter(query, _Connection))
                    {
                        da.Fill(ds);
                    }

                }
            }
            catch (Exception ex)
            {
                return ds = new DataSet();
            }
            return ds;
        }

        public DataTable GetDataTable(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (_Connection = new SqlConnection(GetConnectionString()))
                {
                    using (var da = new SqlDataAdapter(query, _Connection))
                    {
                        da.Fill(dt);
                    }

                }

            }
            catch (Exception ex)
            {
                return dt = new DataTable();
            }

            return dt;
        }


        public bool ExecuteNonQuery(string query)
        {
            bool res = false;
            try
            {
                using (_Connection = new SqlConnection(GetConnectionString()))
                {
                    _Connection.Open();
                    using (var da = new SqlDataAdapter(query, _Connection))
                    {
                        int x = _Command.ExecuteNonQuery();
                        res = x == 1;
                    }

                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return res;
        }

        public object ExecuteScalar(string query)
        {
            object x = null;
            try
            {
                using (_Connection = new SqlConnection(GetConnectionString()))
                {
                    _Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, _Connection))
                    {
                        x = cmd.ExecuteScalar();
                    }

                }

            }
            catch (Exception ex)
            {
                return 0;
            }
            return x;
        }
    }
}
