using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace darbuotoju_valdymos_sistema.Models
{
    public class Task
    {
        private DBContext context;
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; } 
        public bool status { get; set; }
        //[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime dueby { get; set; }
        //[BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime created { get; set; }
        public List<Workers> workers { get; set; }

    }
}
