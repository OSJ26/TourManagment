using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Web.Http;
using TnTSystem.Models;

namespace TnTSystem.Controllers
{
    public class QueryController : ApiController
    {
        MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        [HttpGet]
        public DataTable GetQuery()
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from query", connection);
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
        public DataTable GetQueryById(int id)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from query where id = {id}", connection);
                var render = cmd.ExecuteReader();
                DataTable de = new DataTable();
                de.Load(render);
                return de;
            }
            catch (MySqlException ex) { throw; }
        }

        [HttpPost]
        public string PostQuery([FromBody] Query query)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Insert into query(query,email) value('{query.QueryDes}','{query.Email}')", connection);
                var render = cmd.ExecuteNonQuery();

                if (render <= 0)
                {
                    return "Data Is Not Inserted Some Error Occurs";
                }
                else
                {
                    return render + " Row Affected";
                }

            }
            catch (MySqlException ex)
            {
                throw;
            }
        }
    }
}
