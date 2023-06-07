using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Windows;
using TnTSystem.Models;

namespace TnTSystem.Controllers
{
    public class UserController : ApiController
    {
        //creating connection objecet
        MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        //select user details from table
        [HttpGet]
        public DataTable GetAllUser()
        {
            try
            {
                //int a = 10, b = 0;
                //var res = a / b;

                connection.Open();
                MySqlCommand cmd = new MySqlCommand("select * from user", connection);

                /*cmd.Connection = connection;
                               cmd.CommandText = "select * from user";*/
                var render = cmd.ExecuteReader();
                DataTable de = new DataTable();
                de.Load(render);
                return de;

            }
            catch (MySqlException)
            {
                throw;
            }

        }

        //gives user information using id
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetAllUserByI")]
        public DataTable GetAllUserById(int id)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = connection;
                cmd.CommandText = "Select * from user where  id= " + id;
                var reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                return dt;

            }
            catch (MySqlException)
            {
                throw;
            }
        }

        //used to insert data in user table
        [HttpPost]
        [Route("user/register")]
        public string PostUser(User user)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Insert into user(name,email,mobNum,city,pwd) value('{user.Name}','{user.Email}','{user.MobNum}','{user.City}','{user.Password}')", connection);
                /* cmd.Connection = connection;
                cmd.CommandText = "Insert into user(name,email,number,city,pwd,role) value(@name,@email,@number,@city,@pwd,@role)";
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@number", user.Num);
                cmd.Parameters.AddWithValue("@city", user.City);
                cmd.Parameters.AddWithValue("@pwd", user.Password);*/

                cmd.ExecuteNonQuery();

                return "Data Insert Successfull";
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("api/Login")]
        public HttpResponseMessage Authenticate([FromBody] User user)
        {
            try
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand($"select * from user where email =  '" +user.Email+ "'  AND pwd = '" +user.Password+ "'", connection);
 /*               cmd.Parameters.AddWithValue("@Email",user.Email);
                cmd.Parameters.AddWithValue("@Password",user.Password);*/
                var reader = cmd.ExecuteReader();

                DataTable dt = new DataTable(); 
                dt.Load(reader);
               return  Request.CreateResponse(HttpStatusCode.OK, dt);
                //return user.Id
            }
            catch (MySqlException)
            {
                throw;
            }
        }

        /*        [HttpGet]
                [Route("api/login")]
                public int login([FromUri] User userLogin)
                {
                    int userId = 0;
                    try
                    {
                        connection.Open();
                        MySqlCommand cmd = new MySqlCommand("select * from user where email = '" + userLogin.Email + "' AND pwd = '" + userLogin.Password + "'", connection);
                        var render = cmd.ExecuteReader();
                        if (render.HasRows)
                        {
                            while (render.Read())   
                            {
                                userId = render.GetInt32(0);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                    return userId;
                }*/

        //update user using it id
        [HttpPut]
        public string PutUser(int id, [FromBody] User user)
        {
            string msg = "";
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"UPDATE user SET name = '{user.Name}', email = '{user.Email}', mobNum = '{user.MobNum}', pwd = '{user.Password}', city = '{user.City}'   WHERE id ={id}", connection);

                /* cmd.Connection = connection;
                cmd.CommandText = "UPDATE user SET name = @name, email = @email, number = @number, password = @pwd, city = @city   WHERE id = " + id;
                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@number", user.Num);
                cmd.Parameters.AddWithValue("@pwd", user.Password); 
                cmd.Parameters.AddWithValue("@city", user.City);*/
                int count = cmd.ExecuteNonQuery();
                if (count > 0)
                {
                    msg = "Sucess";
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                msg = "fail";
            }

            return msg;
        }

        //Delete user from the user table using user id 
        [HttpDelete]
        public string DeleteUser(int id)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = connection;
                cmd.CommandText = "Delete from user where id = " + id;

                cmd.ExecuteNonQuery();

                return "Data Delete Successfull";
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
