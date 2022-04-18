using darbuotoju_valdymos_sistema.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace darbuotoju_valdymos_sistema.Controllers
{
    public class WorkersController : Controller
    {
        private readonly ILogger<WorkersController> _logger;

        public WorkersController(ILogger<WorkersController> logger)
        {
            _logger = logger;
        }

        public IActionResult Workers(string sortOrder)
        {
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";

            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var workers = context.GetAllWorkers().ToList();
            switch (sortOrder)
            {
                case "name_desc":
                    workers = workers.OrderByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    workers = workers.OrderByDescending(n => n.id).ToList();
                    break;
                case "name":
                    workers = workers.OrderBy(n => n.name).ToList();
                    break;
                default:
                    workers = workers.OrderBy(n => n.id).ToList();
                    break;
            }
            return View(workers);
        }
        public IActionResult WorkerInfo(int id)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var worker = context.GetWorkerById(id);
            return PartialView("_ModalWorkerView", worker);
        }

        public void CreateNewWorker(string name, string lastname)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            context.CreateNewWorker(name, lastname);
            //return PartialView("_ModalWorkerView");
        }

        public void DeleteWorker(int id)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            context.RemoveWorker(id);
            //return PartialView("_ModalWorkerView");
        }

        public IActionResult CreateWorkerWindow(string name)
        {
            return PartialView("_ModalNewWorkerForm");
        }
        public IActionResult UpdateWorkersTable(string sortOrder)
        {
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";

            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var workers = context.GetAllWorkers().ToList();
            switch (sortOrder)
            {
                case "name_desc":
                    workers = workers.OrderByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    workers = workers.OrderByDescending(n => n.id).ToList();
                    break;
                case "name":
                    workers = workers.OrderBy(n => n.name).ToList();
                    break;
                default:
                    workers = workers.OrderBy(n => n.id).ToList();
                    break;
            }
            return PartialView("_MainWorkersWindow", workers);
        }
        public IActionResult UpdateWorkersTableSearch(string sortOrder, string term)
        {
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["LastnameSortParam"] = sortOrder == "lastname" ? "lastname_desc" : "lastname";
            term = String.IsNullOrEmpty(term) ? "" : term;
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;

            var workers = context.GetAllWorkers().ToList();
            if (term != "")
            {
                var lTerm = term.ToLower();
                workers = workers.Where(n => n.id.ToString().Contains(lTerm) || n.name.ToLower().Contains(lTerm) || n.lastName.ToLower().Contains(lTerm) || n.tasks.Any(x => x.name.ToLower().Contains(lTerm))).ToList();
            }

                switch (sortOrder)
                {
                    case "name_desc":
                        workers = workers.OrderByDescending(n => n.name).ToList();
                        break;
                    case "id_desc":
                        workers = workers.OrderByDescending(n => n.id).ToList();
                        break;
                    case "name":
                        workers = workers.OrderBy(n => n.name).ToList();
                        break;
                    case "lastname":
                        workers = workers.OrderByDescending(n => n.lastName).ToList();
                        break;
                    case "lastname_desc":
                        workers = workers.OrderBy(n => n.lastName).ToList();
                        break;
                    default:
                        workers = workers.OrderBy(n => n.id).ToList();
                        break;
                }
            
            return PartialView("_MainWorkersWindow", workers);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}