using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace darbuotoju_valdymos_sistema.Models
{
    public class DBContext
    {
        public string ConnectionString { get; set; }

        public DBContext(string connectionString)
        {
            this.ConnectionString = connectionString;
        }

        private MySqlConnection GetConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        public List<Workers> GetAllWorkers()
        {
            List<Workers> list = new List<Workers>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM workers", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Workers()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            //tasks = reader.GetString("description"),
                        });
                    }
                }
            }

            return list;
        }
        

    }
}
