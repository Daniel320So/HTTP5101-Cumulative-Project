using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SchoolProject.Models;
using MySql.Data.MySqlClient;
using System.Diagnostics;
using System.Globalization;

namespace SchoolProject.Controllers
{
    public class StudentDataController : ApiController
    {

        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the students table.

        /// <summary>
        /// Returns a list of students in the system
        /// </summary>
        /// <example>GET api/StudentData/ListStudent</example>
        /// <returns>
        /// A list of students
        /// </returns>

        [HttpGet]
        public IEnumerable<Student> ListStudent(string searchKey)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            string query = "Select * from Students where studentfname like @key or studentlname like @key";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Student> students = new List<Student> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Student newStudent = ConvertDataToStudentObject(ResultSet);

                //Add the Student Name to the List
                students.Add(newStudent);
            }

            Conn.Close();

            return students;
        }

        /// <summary>
        /// Finds a student in the system given an ID
        /// </summary>
        /// <param name="id">The student primary key</param>
        /// <returns>A student object</returns>
        [HttpGet]
        public Student FindStudent(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from Students where studentId = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            Student newStudent = new Student();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                newStudent = ConvertDataToStudentObject(ResultSet);

                //Add classes data to the student
            }

            Conn.Close();

            return newStudent;
        }

        /// <summary>
        /// Finds a student in the system given an ID
        /// </summary>
        /// <param name="id">The student primary key</param>
        /// <returns>A student object</returns>
        [HttpGet]
        public List<Student> FindStudents(List<int> ids)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from Students where studentId in (";
            cmd.CommandText = query;

            if (ids.Count == 0) return new List<Student> { };

            for (int i = 0; i < ids.Count; i++)
            {
                cmd.CommandText = cmd.CommandText + "@id" + i + ",";
                cmd.Parameters.AddWithValue("@id" + i, ids[i]);
            }
            cmd.CommandText = cmd.CommandText.Substring(0, cmd.CommandText.Length - 1) + ")";
            Debug.WriteLine(cmd.CommandText);

            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Student> students = new List<Student> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Student newStudent = ConvertDataToStudentObject(ResultSet);

                students.Add(newStudent);
            }

            Conn.Close();

            return students;
        }

        /// <summary>
        /// Return a Student object from the result in the ResulSet
        /// </summary> 
        /// <param name="result">The result from the database</param>
        /// <returns>
        /// A Student Object
        /// </returns>
        /// 

        public Student ConvertDataToStudentObject(MySqlDataReader result)
        {
            int studentId = int.Parse(result["studentId"].ToString());
            string studentFname = result["studentfname"].ToString();
            string studentLname = result["studentlname"].ToString();
            string studentNumber = result["studentnumber"].ToString();
            DateTime enrollDate = DateTime.Parse(result["enroldate"].ToString());
            Student newStudent = new Student(studentId, studentFname, studentLname, studentNumber, enrollDate);

            return newStudent;
        }
    }
}
