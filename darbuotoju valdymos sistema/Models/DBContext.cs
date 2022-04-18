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
                            workers = GetWorkersAssignedToTask(reader.GetInt32("id")),
                            dueby = reader.GetDateTime("dueby"),
                            created = reader.GetDateTime("created")
                        });
                    }
                }
            }

            return list;
        }
        public List<Task> GetAllTasks(List<Task> excluded)
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
                        if (!excluded.Exists(n => n.id == reader.GetInt32("id")))
                        {
                            list.Add(new Task()
                            {
                                id = reader.GetInt32("id"),
                                name = reader.GetString("name"),
                                description = reader.GetString("description"),
                                status = reader.GetBoolean("status"),
                                workers = GetWorkersAssignedToTask(reader.GetInt32("id")),
                                dueby = reader.GetDateTime("dueby"),
                                created = reader.GetDateTime("created")
                            });
                        }
                    }
                }
            }

            return list;
        }
        public Task GetTaskById(int taskid)
        {
            Task taskas = new Task();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM tasks where id = " + taskid, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        taskas = new Task()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            description = reader.GetString("description"),
                            status = reader.GetBoolean("status"),
                            workers = GetWorkersAssignedToTask(reader.GetInt32("id")),
                            dueby = reader.GetDateTime("dueby"),
                            created = reader.GetDateTime("created")
                        };
                    }
                }
            }

            return taskas;
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
                            lastName = reader.GetString("lastname"),
                            tasks = GetTasksAssignedToWorker(reader.GetInt32("id"))
                        });
                    }
                }
            }

            return list;
        }
        public Workers GetWorkerById(int workerid)
        {
            Workers worker = new Workers();

            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("SELECT * FROM workers where id = " + workerid, conn);
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        List<Task> tasks = GetTasksAssignedToWorker(reader.GetInt32("id"));
                        List<Task> excluded = GetAllTasks(tasks);
                        worker = new Workers()
                        {
                            id = reader.GetInt32("id"),
                            name = reader.GetString("name"),
                            lastName = reader.GetString("lastname"),
                            tasks = tasks,
                            excludedTasks = excluded
                        };
                    }
                }
            }

            return worker;
        }
        public Workers AssignTaskToWorker(int workerid, int taskid)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO assignedtasks (worker_id, task_id) VALUES  (" + workerid + "," + taskid + ")", conn);
                var result = cmd.ExecuteNonQuery();
                
            }
            Workers worker = GetWorkerById(workerid);
            return worker;
        }

        public Workers RemoveTaskFromWorker(int workerid, int taskid)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM assignedtasks WHERE (worker_id, task_id) IN ((" + workerid + "," + taskid + "))", conn);
                var result = cmd.ExecuteNonQuery();

            }
            Workers worker = GetWorkerById(workerid);
            return worker;
        }
        public void CreateNewWorker(string name, string lastname)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO workers (name, lastname) VALUES ('" + name + "', '" + lastname + "')", conn);
                var result = cmd.ExecuteNonQuery();
            }
            
        }
        public void CreateNewTask(string name, string description, DateTime duebydate, long createddate)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO tasks (name, description, dueby, created) VALUES ('" + name + "', '" + description + "', '"+duebydate.ToString("yyyy-MM-dd HH:mm:ss") +"', '"+ DateTimeOffset.FromUnixTimeMilliseconds(createddate).DateTime.ToString("yyyy-MM-dd HH:mm:ss") + "' )", conn);
                var result = cmd.ExecuteNonQuery();
            }

        }
        public void UpdateTask(int id, string name, string description, DateTime duebydate, bool status)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE tasks SET name='" + name + "', status = " + status + ", description = '" + description + "', dueby = '" + duebydate.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE id=" + id + ";", conn);
                var result = cmd.ExecuteNonQuery();
            }

        }
        public void MarkTaskAsDone(int id)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("UPDATE tasks SET status = true WHERE id=" + id + ";", conn);
                var result = cmd.ExecuteNonQuery();
            }
        }
        public void RemoveTask(int id)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM tasks WHERE id=" + id, conn);
                var result = cmd.ExecuteNonQuery();
            }

        }
        public void RemoveWorker(int id)
        {
            using (MySqlConnection conn = GetConnection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("DELETE FROM workers WHERE id=" + id, conn);
                var result = cmd.ExecuteNonQuery();
            }

        }
        public void CreateNewWorkerWindow(string Name)
        {
            

        }
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
                            lastName = reader.GetString("lastname"),
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
                            dueby = reader.GetDateTime("dueby"),
                            created = reader.GetDateTime("created")
                        });
                    }
                    reader.Close();
                }
                
                

            }

            return list;
        }

    }
}
