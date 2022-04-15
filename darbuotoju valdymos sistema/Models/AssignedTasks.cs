using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace darbuotoju_valdymos_sistema.Models
{
    public class AssignedTasks
    {
        private DBContext context;
        public int worker_id { get; set; }
        public int task_id { get; set; }

        
    }
}
