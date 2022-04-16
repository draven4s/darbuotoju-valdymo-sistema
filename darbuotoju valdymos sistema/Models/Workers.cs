using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace darbuotoju_valdymos_sistema.Models
{
    public class Workers
    {
        private DBContext context;
        public int id { get; set; }
        public string name { get; set; }
        public List<Task> tasks { get; set; }
        public List<Task> excludedTasks { get; set; }
        //more things needed in here, will add more when I think of what we need more :D

    }
}
