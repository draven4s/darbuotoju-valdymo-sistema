using darbuotoju_valdymos_sistema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace darbuotoju_valdymos_sistema.Controllers
{
    public class TasksController : Controller
    {
        private readonly ILogger<TasksController> _logger;

        public TasksController(ILogger<TasksController> logger)
        {
            _logger = logger;
        }

        public IActionResult Tasks(string Sort_Order)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(Sort_Order) ? "name_desc" : "";
            ViewData["IdSortParam"] = Sort_Order == "id" ? "id_desc" : "id";
            ViewData["DateSortParam"] = Sort_Order == "date" ? "date_desc" : "date";
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var tasks = context.GetAllTasks().ToList();
            switch (Sort_Order)
            {
                case "name_desc":
                    tasks = tasks.OrderByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    tasks = tasks.OrderByDescending(n => n.id).ToList();
                    break;
                case "id":
                    tasks = tasks.OrderBy(n => n.id).ToList();
                    break;
                default:
                    tasks = tasks.OrderBy(n => n.id).ToList();
                    break;
            }
            return View(tasks);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}