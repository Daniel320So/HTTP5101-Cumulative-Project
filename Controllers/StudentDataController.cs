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

        /// <summary>Returns a list of students in the system filtered by name</summary>
        /// <param name="searchKey">The search key of student name</param>
        /// <returns>A list of students</returns>
        /// <example>GET api/StudentData/ListStudent/${searchKey}</example>

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

        /// <summary>Finds a student in the system given an ID</summary>
        /// <param name="id">The student primary key</param>
        /// <returns>A student object</returns>
        /// <example>GET api/StudentData/FindStudent/${id}</example>
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
                ClassDataController classDataController = new ClassDataController();
                newStudent.classes = classDataController.FindClassesByStudentId(id);

                //Add available classes data to the student
                newStudent.availableClasses = new List<Class>();
                List<Class> allClasses = new List<Class>(classDataController.ListClass(""));
                for (int i=0; i < allClasses.Count; i++)
                {
                    if (!newStudent.classes.Exists(v =>  v.classId == allClasses[i].classId))
                    {
                        newStudent.availableClasses.Add(allClasses[i]);
                    }
                }
            }

            Conn.Close();

            return newStudent;
        }

        /// <summary>Find a list of student in the system given a list of ids</summary>
        /// <param name="ids">A list of student primary key</param>
        /// <returns>A list of student object</returns>
        /// <example>GET api/StudentData/FindStudents/${ids}</example>
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

        /// <summary>Return a Student object from the result in the ResulSet</summary> 
        /// <param name="result">The result from the database</param>
        /// <returns>A Student Object</returns>
    

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

        /// <summary>
        ///     Add a class to the student
        /// </summary>
        /// <param name="studentId"> Student Id </param>
        /// <param name="classId"> Class Id </param>
        /// <example> POST : /api/StudentData/AddClassToStudent/3/4 </example>

        [HttpPost]
        public void AddClassToStudent(int studentId, int classId)
        {

            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Insert Into studentsxclasses (studentid, classid) values (@studentId, @classId)";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@classId", classId);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }


        /// <summary>
        ///     Delete a class from the student
        /// </summary>
        /// <param name="studentId"> Student Id </param>
        /// <param name="classId"> Class Id </param>
        /// <example> POST : /api/StudentData/DeleteClassFromStudent/3/4 </example>

        [HttpPost]
        public void DeleteClassFromStudent(int studentId, int classId)
        {

            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Delete from studentsxclasses where studentid = @studentId and classid = @classId";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@studentId", studentId);
            cmd.Parameters.AddWithValue("@classId", classId);
            cmd.Prepare();

            cmd.ExecuteNonQuery();

            Conn.Close();
        }
    }
}
