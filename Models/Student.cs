using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebGrease.Extensions;

namespace SchoolProject.Models
{
    public class Student
    {
        //Varaibles
        public int studentId;
        public string studentFname;
        public string studentLname;
        public string studentNumber;
        public DateTime enrollDate;
        public List<Class> classes;
        public List<Class> availableClasses;

        //Constructor
        public Student(int _studentId = -1, string _studentFname = null, string _studentLname = null, string _studentNumber = null, DateTime _enrollDate = new DateTime())
        {
            studentId = _studentId;
            studentFname = _studentFname;
            studentLname = _studentLname;
            studentNumber = _studentNumber;
            enrollDate = _enrollDate;
        }
    }
}