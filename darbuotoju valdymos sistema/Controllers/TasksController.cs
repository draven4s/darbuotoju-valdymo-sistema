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

        public IActionResult Tasks(string sortOrder, string term)
        {
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            term = String.IsNullOrEmpty(term) ? "" : term;
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;

            var tasks = context.GetAllTasks().ToList();
            if (term != "")
            {
                var lTerm = term.ToLower();
                tasks = tasks.Where(n => n.id.ToString().Contains(lTerm) || n.name.ToLower().Contains(lTerm) || n.workers.Any(x => x.name.ToLower().Contains(lTerm)) || n.workers.Any(x => x.lastName.ToLower().Contains(lTerm))).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tasks = tasks.OrderByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    tasks = tasks.OrderByDescending(n => n.id).ToList();
                    break;
                case "name":
                    tasks = tasks.OrderBy(n => n.name).ToList();
                    break;
                case "date":
                    tasks = tasks.OrderByDescending(n => n.dueby).ToList();
                    break;
                case "date_desc":
                    tasks = tasks.OrderBy(n => n.dueby).ToList();
                    break;
                default:
                    tasks = tasks.OrderBy(n => n.id).ToList();
                    break;
            }
            return View(tasks);
        }
        public IActionResult TaskInfo(int id)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var tasks = context.GetTaskById(id);
            return PartialView("_ModalTaskView", tasks);
        }
        public IActionResult AddTask(int userid, int taskid)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var userInfo = context.AssignTaskToWorker(userid, taskid);
            return PartialView("_ModalWorkerView", userInfo);
        }
        public IActionResult RemoveTask(int userid, int taskid)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var userInfo = context.RemoveTaskFromWorker(userid, taskid);
            return PartialView("_ModalWorkerView", userInfo);
        }
        public void DeleteTask(int taskid)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            context.RemoveTask(taskid);
        }
        public IActionResult RemoveTaskAs(int userid, int taskid)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            var userInfo = context.RemoveTaskFromWorker(userid, taskid);
            return PartialView("_ModalWorkerView", userInfo);
        }

        public IActionResult CreateTaskWindow(string name)
        {
            return PartialView("_ModalNewTaskForm");
        }
        public IActionResult UpdateTasksTableSearch(string sortOrder, string term)
        {
            ViewData["IdSortParam"] = String.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["DateSortParam"] = sortOrder == "date" ? "date_desc" : "date";
            term = String.IsNullOrEmpty(term) ? "" : term;
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;

            var tasks = context.GetAllTasks().ToList();
            if (term != "")
            {
                var lTerm = term.ToLower();
                tasks = tasks.Where(n => n.id.ToString().Contains(lTerm) || n.name.ToLower().Contains(lTerm) || n.workers.Any(x => x.name.ToLower().Contains(lTerm)) || n.workers.Any(x => x.lastName.ToLower().Contains(lTerm))).ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    tasks = tasks.OrderByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    tasks = tasks.OrderByDescending(n => n.id).ToList();
                    break;
                case "name":
                    tasks = tasks.OrderBy(n => n.name).ToList();
                    break;
                case "date":
                    tasks = tasks.OrderByDescending(n => n.dueby).ToList();
                    break;
                case "date_desc":
                    tasks = tasks.OrderBy(n => n.dueby).ToList();
                    break;
                default:
                    tasks = tasks.OrderBy(n => n.id).ToList();
                    break;
            }

            return PartialView("_MainTasksView", tasks);
        }
        public void CreateNewTask(string name, string description, DateTime duebydate, long createddate)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            context.CreateNewTask(name, description, duebydate, createddate);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}