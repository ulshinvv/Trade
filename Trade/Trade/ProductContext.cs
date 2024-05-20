using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Trade
{
    public class ProductContext : DbContext
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }

        public string connectionString = @"Data Source=Vova; DataBase=Trade; Integrated Security=True; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true; encrypt=false;";
        private SqlConnection connection;

        public DataTable SqlSelect(string cmd)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlCommand command = new SqlCommand(cmd, conn);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    DataTable dt = new DataTable();
                    dt.Load(reader);
                    reader.Close();
                    return dt;
                }
            }
        }

        public DataTable SqlInsert(string cmd)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand command = new SqlCommand(cmd, conn);
            command.ExecuteNonQuery();
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
        /// <summary>
        /// Метод для открытия соединения с БД
        /// </summary>
        public void OpenConnection()
        {
            // Если состояние строки закрыто, открываем
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        /// <summary>
        /// Метод для закрытия соединения с БД
        /// </summary>
        public void CloseConnection()
        {
            // Если состояние строки открыто, закрываем
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        /// <summary>
        /// Метод для возвращения строки подключения
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetConnection()
        {
            return connection;
        }
    }
}

