using System;
using System.CodeDom;
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

        //GET : /Teacher/DeleteFailed/${id}
        public ActionResult DeleteFailed(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = controller.FindTeacher(id);
            return View(teacher);
        }

        [HttpPost]
        //POST : /Teacher/Delete/{id}
        public ActionResult Delete(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            try
            {
                controller.DeleteTeacher(id);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return RedirectToAction("DeleteFailed", new { id = id });
            }

            return RedirectToAction("List");
        }

        //GET : /Teacher/New
        public ActionResult New()
        {
            return View();
        }

        //POST : /Teacher/Create
        /// <summary>
        ///     Insert a new teacher in the database
        /// </summary>
        /// <param name="teacherFname"> Teacher First Name</param>
        /// <param name="teacherLname"> Teacher Last NAme</param>
        /// <param name="employeeNumber"> Teacher Employee Number</param>
        /// <param name="salary"> Salary</param>
        /// <returns> A Dynamic Webpage that provide the information of the teacher</returns>
        [HttpPost]
        public ActionResult Create(string teacherFname, string teacherLname, string employeeNumber, string salary)
        {
            // Server side data validation
            try
            {
                if (teacherFname == "" || teacherFname == null)
                {
                    throw new Exception("Teacher First Name Is Empty");
                }
                else if (teacherLname == "" || teacherLname == null)
                {
                    throw new Exception("Teacher Last Name Is Empty");
                }
                else if (employeeNumber == "" || employeeNumber == null)
                {
                    throw new Exception("Employee Number Is Empty");
                }
                else if (salary == "" || salary == null)
                {
                    throw new Exception("Salary Is Empty");
                }
            } catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return RedirectToAction("List");
            }

            
            DateTime hireDate = DateTime.Now;
            float _salary = float.Parse(salary);
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = new Teacher(-1, teacherFname, teacherLname, employeeNumber, hireDate, _salary); // -1 is used as default teacher id
            controller.AddTeacher(teacher);
            int id = controller.getLatestTeacherId();
            return RedirectToAction("Show", new { id = id });
        }

        //GET : /Teacher/Update
        public ActionResult Update(int id)
        {
            TeacherDataController controller = new TeacherDataController();
            Teacher selectedTeacher = controller.FindTeacher(id);
            return View(selectedTeacher);
        }

        //POST : /Teacher/Update/${id}
        /// <summary>
        ///     Update a teacher in the database
        /// </summary>
        /// <param name="teacherFname"> Teacher First Name</param>
        /// <param name="teacherLname"> Teacher Last Name</param>
        /// <param name="employeeNumber"> Teacher Employee Number</param>
        /// <param name="hireDate"> Hire Date</param>
        /// <param name="salary"> Salary</param>
        /// <returns> A Dynamic Webpage that provide the information of the teacher</returns>
        /// 
        [HttpPost]
        public ActionResult Update(int id, string teacherFname, string teacherLname, string employeeNumber, string hireDate, string salary)
        {
            Debug.WriteLine(id);
            // Server side data validation
            try
            {
                if (id < 0)
                {
                    throw new Exception("Teacher Id is not Found");
                } 
                else if (teacherFname == "" || teacherFname == null)
                {
                    throw new Exception("Teacher First Name Is Empty");
                }
                else if (teacherLname == "" || teacherLname == null)
                {
                    throw new Exception("Teacher Last Name Is Empty");
                }
                else if (employeeNumber == "" || employeeNumber == null)
                {
                    throw new Exception("Employee Number Is Empty");
                }
                else if (hireDate == "" || hireDate == null)
                {
                    throw new Exception("Hire Date Is Empty");
                }
                else if (salary == "" || salary == null)
                {
                    throw new Exception("Salary Is Empty");
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return RedirectToAction("List");
            }


            float _salary = float.Parse(salary);
            DateTime _hireDate = DateTime.Parse(hireDate);
            TeacherDataController controller = new TeacherDataController();
            Teacher teacher = new Teacher(id, teacherFname, teacherLname, employeeNumber, _hireDate, _salary);
            controller.UpdateTeacher(teacher);

            return RedirectToAction("Show", new { id = id });
        }
    }
}