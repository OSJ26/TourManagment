using MySql.Data.MySqlClient;
using System;
using TnTSystem.Models;
using System.Configuration;
using System.Data;
using System.Web.Http;

namespace TnTSystem.Controllers
{
    public class ReviewController : ApiController
    {

        MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        [HttpGet]
        [Route("api/Getall")]
        public DataTable GetReview()
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from user_review", connection);
                var render = cmd.ExecuteReader();
                DataTable de = new DataTable();
                de.Load(render);
                return de;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("api/Getsome")]
        public DataTable GetUserReviewBySpecificcol()
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"select name,rating,review from user_review", connection);
                //cmd.CommandText = "select name,rating,review from user_review";
                var render = cmd.ExecuteReader();
                DataTable de = new DataTable();
                de.Load(render);
                return de;
            }
            catch (MySqlException ex)
            {
                throw;
            }
        }

        [HttpPost]
        public string PostReview([FromBody] Review review)
        {

            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Insert into user_review(name,review,rating) value('{review.Name}','{review.Description}','{review.Rating}')", connection);
                var render = cmd.ExecuteNonQuery();

                if (render <= 0)
                {
                    return "Data is not inserted";
                }
                else
                {
                    return render + " Review Added";
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
