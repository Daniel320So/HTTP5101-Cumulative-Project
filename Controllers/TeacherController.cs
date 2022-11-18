using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class TeacherController : Controller
    {
        // GET: Teacher
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Teacher/List/${searchKey}/${minSalary}/${maxSalary}
        public ActionResult List(string searchKey, float minSalary = 0, float maxSalary = 9999999999) //9999999999 is the max value for data type decimal(10,2)
        {
            TeacherDataController controller = new TeacherDataController();
            IEnumerable<Teacher> teachers = controller.ListTeacher(searchKey, minSalary, maxSalary);
            return View(teachers);
        }

        //GET : /Teacher/Show/${id}
        public ActionResult Show(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.FindTeacher(id);
            return View(teacher);
        }
    }
}