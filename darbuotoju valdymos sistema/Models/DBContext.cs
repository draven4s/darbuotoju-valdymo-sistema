using MySql.Data.MySqlClient;
using System;
using System.Data;
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
                            tasks = GetTasksAssignedToWorker(reader.GetInt32("id"))
                        });
                    }
                }
            }

            return list;
        }

        public List<Task> GetAllTasks()
        {
            List<Task> list = new List<Task>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tasks", conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Task()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            description = reader.GetString("description"),
                            status = reader.GetBoolean("status"),
                            workers = GetWorkersAssignedToTask(reader.GetInt32("id"))
                        });
                    }
                }
            }

            return list;
        }

        // getting the 
        public List<Workers> GetWorkersAssignedToTask(int taskid)
        {
            List<Workers> list = new List<Workers>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM assignedtasks v INNER JOIN workers c on v.worker_id = c.id where task_id = " + taskid, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Workers()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                        });
                    }
                }
            }

            return list;
        }

        public List<Task> GetTasksAssignedToWorker(int workerid)
        {
            List<Task> list = new List<Task>();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * FROM assignedtasks v INNER JOIN tasks c on v.task_id = c.id where worker_id = " + workerid, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Task()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            description = reader.GetString("description"),
                            status = reader.GetBoolean("status"),
                        });
                    }
                    reader.Close();
                }
                
                

            }

            return list;
        }

    }
}
