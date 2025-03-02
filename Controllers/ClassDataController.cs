﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace SchoolProject.Controllers
{
    public class ClassDataController : ApiController
    {

        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the classes table.

        /// <summary> Returns a list of classes in the system by filtering the Class Name </summary>
        /// <param name="searchKey"> search key for class name</param>
        /// <returns> A list of classes </returns>
        /// <example>GET api/ClassData/ListClass/${searchKey}</example>

        [HttpGet]
        public IEnumerable<Class> ListClass(string searchKey)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from Classes where classname like @key";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Class> classes = new List<Class> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Class newClass = ConvertDataToClassObject(ResultSet);

                //Add the Class Name to the List
                classes.Add(newClass);
            }

            Conn.Close();

            return classes;
        }

        /// <summary> Finds a class in the system given an ID</summary>
        /// <param name="id">The class primary key</param>
        /// <returns>A class object</returns>
        /// <example>GET api/ClassData/FindClass/${id}</example>
        [HttpGet]
        public Class FindClass(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from Classes where classId = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            Class newClass = new Class();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                newClass = ConvertDataToClassObject(ResultSet);
                TeacherDataController teacherController = new TeacherDataController();
                newClass.teacher = teacherController.FindTeacher(int.Parse(ResultSet["teacherid"].ToString()));

                List<int> studentIds = FindStudentEnrolledInClass(id);
                StudentDataController studentController = new StudentDataController();
                newClass.students = studentController.FindStudents(studentIds);
                newClass.studentNumber = newClass.students.Count;
            }

            Conn.Close();

            return newClass;
        }

        /// <summary> Finds a list of class in the system given an teacherId </summary>
        /// <param name="id">The teacherId foreign key</param>
        /// <returns>A list of class object</returns>
        /// <example>GET api/ClassData/FindClassByTeacherId/${id}</example>
        [HttpGet]
        public List<Class> FindClassByTeacherId(int id)
        { 
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from Classes where teacherId = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Class> classes = new List<Class> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Class newClass = ConvertDataToClassObject(ResultSet);
                classes.Add(newClass);
            }

            Conn.Close();

            return classes;
        }

        /// <summary> Finds a list of class in the system given an studentId </summary>
        /// <param name="id">The studentId foreign key</param>
        /// <returns>A list of class object</returns>
        /// <example>GET api/ClassData/FindClassesByStudentId/${id}</example>

        [HttpGet]
        public List<Class> FindClassesByStudentId(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from Classes c join studentsxclasses sxc on c.classId = sxc.classId where studentId = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Class> classes = new List<Class> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Class newClass = ConvertDataToClassObject(ResultSet);
                classes.Add(newClass);
            }

            Conn.Close();

            return classes;
        }


        /// <summary> Finds a list of studentId in the system given an classId </summary>
        /// <param name="id">The classId</param>
        /// <returns>A list of studentId</returns>
        /// <example>GET api/ClassData/FindStudentEnrolledInClass/${id}</example>
        [HttpGet]
        public List<int> FindStudentEnrolledInClass(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from studentsxclasses where classId = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<int> studentIds = new List<int> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                studentIds.Add(int.Parse(ResultSet["studentId"].ToString()));
            }

            Conn.Close();

            return studentIds;
        }

        /// <summary>Return a Class object from the result in the ResulSet</summary> 
        /// <param name="result">The result from the database</param>
        /// <returns>A Class Object</returns>

        public Class ConvertDataToClassObject(MySqlDataReader result)
        {
            int classId = int.Parse(result["classid"].ToString());
            string classCode = result["classcode"].ToString();
            int teacherId = int.Parse(result["teacherid"].ToString());
            DateTime startDate = DateTime.Parse(result["startdate"].ToString());
            DateTime finishDate = DateTime.Parse(result["finishdate"].ToString());
            string className = result["classname"].ToString();
            return new Class(classId, classCode, teacherId, startDate, finishDate, className);
        }
    }
}
