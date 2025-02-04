using System.Data;
using System.Data.SqlClient;

namespace Dashboard.Database
{
    [Obsolete]
    public class Executer
    {
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
                using (SqlConnection _Connection = new SqlConnection(GetConnectionString()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, _Connection))
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
                using (SqlConnection _Connection = new SqlConnection(GetConnectionString()))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(query, _Connection))
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

        public DataTable GetDataTable(string query, params SqlParameter[] sqlParameters)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(GetConnectionString()))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(sqlParameters);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            return dt;
        }



        public bool ExecuteNonQuery(string query)
        {
            bool res = false;
            try
            {
                using (SqlConnection con = new SqlConnection(GetConnectionString()))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        int x = cmd.ExecuteNonQuery();
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


        public bool ExecuteNonQuery(string query, params SqlParameter[] sqlParameters)
        {
            bool res = false;
            try
            {
                using (SqlConnection _Connection = new SqlConnection(GetConnectionString()))
                {
                    _Connection.Open();
                    using (SqlCommand _Command = new SqlCommand(query, _Connection))
                    {
                        if (sqlParameters != null)
                        {
                            _Command.Parameters.AddRange(sqlParameters);
                        }

                        int x = _Command.ExecuteNonQuery();
                        res = x > 0;  // Ensure at least 1 row is affected
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);  // Log for debugging
                return false;
            }
            return res;
        }


        public object ExecuteScalar(string query)
        {
            object x = null;
            try
            {
                using (SqlConnection _Connection = new SqlConnection(GetConnectionString()))
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

        public object ExecuteScalar(string query, params SqlParameter[] sqlParameters)
        {
            object x = null;
            try
            {
                using (SqlConnection _Connection = new SqlConnection(GetConnectionString()))
                {
                    _Connection.Open();
                    using (SqlCommand cmd = new SqlCommand(query, _Connection))
                    {
                        if (sqlParameters != null)
                        {
                            cmd.Parameters.AddRange(sqlParameters);
                        }

                        x = cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return 0;
            }
            return x ?? 0;  // Return 0 if null
        }



    }
}
