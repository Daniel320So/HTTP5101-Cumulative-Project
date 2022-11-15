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
    public class TeacherDataController : ApiController
    {

        private SchoolDbContext School = new SchoolDbContext();

        //This Controller Will access the teachers table.

        /// <summary>
        /// Returns a list of teachers in the system
        /// </summary>
        /// <example>GET api/TeacherData/ListTeacher</example>
        /// <returns>
        /// A list of teachers
        /// </returns>

        [HttpGet]
        public IEnumerable<Teacher> ListTeacher(string searchKey)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();
            string query = "Select * from Teachers where teacherfname like @key or teacherlname like @key";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@key", "%" + searchKey + "%");
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            List<Teacher> teachers = new List<Teacher> { };

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                Teacher newTeacher = ConvertDataToTeacherObject(ResultSet);

                //Add the Author Name to the List
                teachers.Add(newTeacher);
            }

            Conn.Close();

            return teachers;
        }

        /// <summary>
        /// Finds a teacher in the system given an ID
        /// </summary>
        /// <param name="id">The teacher primary key</param>
        /// <returns>A teacher object</returns>
        [HttpGet]
        public Teacher FindTeacher(int id)
        {
            MySqlConnection Conn = School.AccessDatabase();
            Conn.Open();
            MySqlCommand cmd = Conn.CreateCommand();

            string query = "Select * from Teachers where teacherId = @id";
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@id", id);
            cmd.Prepare();

            MySqlDataReader ResultSet = cmd.ExecuteReader();
            Teacher newTeacher = new Teacher();

            while (ResultSet.Read())
            {
                //Access Column information by the DB column name as an index
                newTeacher = ConvertDataToTeacherObject(ResultSet);

                //Add classes data to the teacher
                ClassDataController controller = new ClassDataController();
                List<Class> classes = controller.FindClassByTeacherId(newTeacher.teacherId);
                newTeacher.classes = classes;
            }

            Conn.Close();

            return newTeacher;
        }

        /// <summary>
        /// Return a Teacher object from the result in the ResulSet
        /// </summary> 
        /// <param name="result">The result from the database</param>
        /// <returns>
        /// A Teacher Object
        /// </returns>
        /// 

        public Teacher ConvertDataToTeacherObject(MySqlDataReader result)
        {
            int teacherId = int.Parse(result["teacherId"].ToString());
            string teacherFname = result["teacherfname"].ToString();
            string teacherLname = result["teacherlname"].ToString();
            string employeeNumber = result["employeenumber"].ToString();
            DateTime hireDate = DateTime.Parse(result["hireDate"].ToString());
            float salary = float.Parse(result["salary"].ToString());
            Teacher newTeacher = new Teacher(teacherId, teacherFname, teacherLname, employeeNumber, hireDate, salary);

            return newTeacher;
        }
    }
}
