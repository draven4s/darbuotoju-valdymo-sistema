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
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["DateSortParam"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
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
                    tasks = tasks.OrderBy(n => n.status == true).ThenByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    tasks = tasks.OrderBy(n => n.status == true).ThenByDescending(n => n.id).ToList();
                    break;
                case "name":
                    tasks = tasks.OrderBy(n => n.status == true).ThenBy(n => n.name).ToList();
                    break;
                case "id":
                    tasks = tasks.OrderBy(n => n.status == true).ThenBy(n => n.id).ToList();
                    break;
                case "date_desc":
                    tasks = tasks.OrderBy(n => n.status == true).ThenByDescending(n => n.dueby).ToList();
                    break;
                default:
                    tasks = tasks.OrderBy(n => n.status == true).ThenBy(n => n.dueby).ToList();
                    break;
            }
            return View(tasks);
        }
        public IActionResult UpdateTasksExcludedSearch(string term, int userid)
        {
            term = String.IsNullOrEmpty(term) ? "" : term;
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;

            List<darbuotoju_valdymos_sistema.Models.Task> tasks = context.GetTasksAssignedToWorker(userid);
            List<darbuotoju_valdymos_sistema.Models.Task> excluded = context.GetAllTasks(tasks);

            if (term != "")
            {
                var lTerm = term.ToLower();
                excluded = excluded.Where(n => n.id.ToString().Contains(lTerm) || n.name.ToLower().Contains(lTerm)).ToList();
            }
            return PartialView("_ExcludedTasks", excluded);
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
            ViewData["IdSortParam"] = sortOrder == "id" ? "id_desc" : "id";
            ViewData["NameSortParam"] = sortOrder == "name" ? "name_desc" : "name";
            ViewData["DateSortParam"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
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
                    tasks = tasks.OrderBy(n => n.status == true).ThenByDescending(n => n.name).ToList();
                    break;
                case "id_desc":
                    tasks = tasks.OrderBy(n => n.status == true).ThenByDescending(n => n.id).ToList();
                    break;
                case "name":
                    tasks = tasks.OrderBy(n => n.status == true).ThenBy(n => n.name).ToList();
                    break;
                case "id":
                    tasks = tasks.OrderBy(n => n.status == true).ThenBy(n => n.id).ToList();
                    break;
                case "date_desc":
                    tasks = tasks.OrderBy(n => n.status == true).ThenByDescending(n => n.dueby).ToList();
                    break;
                default:
                    tasks = tasks.OrderBy(n => n.status == true).ThenBy(n => n.dueby).ToList();
                    break;
            }

            return PartialView("_MainTasksView", tasks);
        }
        public void MarkTaskAsDone(int id)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            context.MarkTaskAsDone(id);
        }
        public void CreateNewTask(string name, string description, DateTime duebydate, long createddate)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            context.CreateNewTask(name, description, duebydate, createddate);
        }
        public void UpdateTask(int id,string name, string description, DateTime duebydate, bool status)
        {
            DBContext context = HttpContext.RequestServices.GetService(typeof(DBContext)) as DBContext;
            context.UpdateTask(id, name, description, duebydate, status);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}