using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchoolProject.Models;

namespace SchoolProject.Controllers
{
    public class StudentController : Controller
    {
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        //GET : /Student/List/${searchKey}
        public ActionResult List(string searchKey)
        {
            StudentDataController controller = new StudentDataController();
            IEnumerable<Student> students = controller.ListStudent(searchKey);
            return View(students);
        }

        //GET : /Student/Show/${id}
        public ActionResult Show(int id)
        {
            StudentDataController controller = new StudentDataController();
            Student student = controller.FindStudent(id);
            return View(student);
        }
    }
}