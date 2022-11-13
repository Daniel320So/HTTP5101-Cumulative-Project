using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class ClassController : Controller
    {
        // GET: Class
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Class/List
        public ActionResult List()
        {
            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> classes = controller.ListClass();
            return View(classes);
        }

        //GET : /Class/Show/${id}
        public ActionResult Show(int id)
        {
            ClassDataController controller = new ClassDataController();
            Class classObj = controller.FindClass(id);
            return View(classObj);
        }
    }
}