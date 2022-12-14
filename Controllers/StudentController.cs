using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        //POST : /Student/DeleteClassFromStudent/${studentId},${classId}
        /// <summary>
        ///     Delete a class from the student
        /// </summary>
        /// <param name="id"> A string of two id separated by ","  The first one is student id and the second one is class id</param>
        /// <returns> A Dynamic Webpage that provide the information of the student</returns>
        /// 

        [HttpPost]
        public ActionResult DeleteClassFromStudent(string id)
        {
            int studentId = int.Parse(id.Split(',')[0]);
            int classId = int.Parse(id.Split(',')[1]);
            StudentDataController controller = new StudentDataController();
            controller.DeleteClassFromStudent(studentId, classId);
            return RedirectToAction("Show", new { id = studentId });
        }
    }
}