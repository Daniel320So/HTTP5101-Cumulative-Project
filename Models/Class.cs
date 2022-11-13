using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolProject.Models
{
    public class Class
    {
        //Varaibles
        public int classId;
        public string classCode;
        public int teacherId;
        public DateTime startDate;
        public DateTime finishDate;
        public string className;
        public Teacher teacher;

        //Constructor
        public Class(int _classId = -1, string _classCode = null, int _teacherId = -1, DateTime _startDate = new DateTime(), DateTime _finishDate = new DateTime(), string _className = null)
        {
            classId = _classId;
            classCode = _classCode;
            teacherId = _teacherId;
            startDate = _startDate;
            finishDate = _finishDate;
            className = _className;
        }

    }
}