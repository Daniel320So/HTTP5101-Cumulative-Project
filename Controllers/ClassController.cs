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

        //GET : /Class/List/${searchKey}
        public ActionResult List(string searchKey)
        {
            ClassDataController controller = new ClassDataController();
            IEnumerable<Class> classes = controller.ListClass(searchKey);
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