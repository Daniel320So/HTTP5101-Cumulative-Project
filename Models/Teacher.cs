using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Teacher
    {
        //Varaibles
        public int teacherId;
        public string teacherFname;
        public string teacherLname;
        public string employeeNumber;
        public DateTime hireDate;
        public double salary;

        //Constructor
        public Teacher(int _teacherId, string _teacherFname, string _teacherLname, string _employeeNumber, DateTime _hireDate, double _salary)
        {
            teacherId = _teacherId;
            teacherFname = _teacherFname;
            teacherLname = _teacherLname;
            employeeNumber = _employeeNumber;
            hireDate = _hireDate;
            salary = _salary;
        }

    }
}