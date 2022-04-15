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
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var workers = context.GetAllWorkers().ToList();
            switch (Sort_Order)
            {
                case "name_desc":
                    workers = workers.OrderByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    workers = workers.OrderByDescending(n => n.id).ToList();
                    break;
                case "id":
                    workers = workers.OrderBy(n => n.id).ToList();
                    break;
                default:
                    workers = workers.OrderBy(n => n.id).ToList();
                    break;
            }
            return View(workers);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}