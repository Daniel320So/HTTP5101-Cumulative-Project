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

        //GET : /Teacher/DeleteConfirm/${id}
        public ActionResult DeleteConfirm(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.FindTeacher(id);
            return View(teacher);
        }

        //POST : /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            controller.DeleteTeacher(id);
            return RedirectToAction("List");
        }

        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Teacher/Create
        public ActionResult Create(string teacherLname, string teacherFname, string employeeNumber, string salary)
        {
            DateTime hireDate = DateTime.Now;
            float _salary = float.Parse(salary);
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = new Teacher(-1, teacherLname, teacherFname, employeeNumber, hireDate, _salary); // -1 is used as default teacher id
            controller.AddTeacher(teacher);
            int id = controller.getLatestTeacherId();
            return RedirectToAction("Show", new { id = id});
        }
    }
}