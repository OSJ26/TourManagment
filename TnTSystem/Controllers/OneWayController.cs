using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Net;
using System.Runtime.Caching;
using System.Web.Http;
using TnTSystem.Models;
using System;
using Newtonsoft.Json;
using TnTSystem.Filter;

namespace TnTSystem.Controllers
{
    public class OneWayController : ApiController
    {
        MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        [HttpGet]
        public DataTable GetData()
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("select * from oneway", connection);
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

        [HttpGet]
        public DataTable GetDataById(int id)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Select * from oneway where  id= {id}", connection);
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

        [HttpGet]   
        [Route("api/SearchResult")]
        public DataTable GetData([FromUri]string Source)
        {
            try { 
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from oneway where source = '{Source}'", connection);
                var reader = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;
            } catch(MySqlException ex) {
                throw;
            }
        }

        [HttpGet]
        [Route("api/GetSearchData")]
        public HttpResponseMessage GetSearchData([FromUri] string source, [FromUri] string destination)
        {
            try {
                var cache = MemoryCache.Default;
                string cacheKey = "this is key";
                            
                if (cache.Contains(cacheKey))
                {
                    var cacheProduct = cache.Get(cacheKey);
                    //string jsonProduct = JsonConvert.SerializeObject(cacheProduct);
                    return Request.CreateResponse(HttpStatusCode.OK, "Data Come from cache \n " + cacheProduct);
                }

                else
                {
                    connection.Open();
                    CacheItemPolicy chaceitemPoilcy = new CacheItemPolicy();
                    chaceitemPoilcy.AbsoluteExpiration = DateTime.Now.AddMinutes(3);
                    string query = $"select source, destination, price,stops,date,notes," +
                        $"pickup_point,drop_address from oneway where source = '{source}' And destination = '{destination}' ";

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        string jsonTour = JsonConvert.SerializeObject(dt);
                        cache.Add(cacheKey, jsonTour, chaceitemPoilcy);
                        return Request.CreateResponse(HttpStatusCode.OK,jsonTour);
                    }
                }

            } catch {
                throw;
            }
        }

        [CustomAuth]
        [HttpPost]
        public string PostData([FromBody] OneWay oneway)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Insert into oneway(source,destination,price,stops,notes,pickup_point,drop_address,max_passanger) value('{oneway.Source}','{oneway.Destination}',{oneway.Price},'{oneway.stops}','{oneway.Other_notes}','{oneway.Pickup_Point}','{oneway.Drop_point}',{oneway.Max_Passanger})", connection);

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
            catch (MySqlException)
            {

                throw;
            }

        }

        [CustomAuth]
        [HttpPut]
        public string PutData(int id, [FromBody] OneWay oneWay)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"Update oneway set source = '{oneWay.Source}', destination = '{oneWay.Destination}',price = {oneWay.Price},notes = '{oneWay.Other_notes}',pickup_point = '{oneWay.Pickup_Point}',drop_address = '{oneWay.Drop_point}', max_passanger = {oneWay.Max_Passanger} where  id= {id}", connection);
                int count = cmd.ExecuteNonQuery();

                if (count < 0)
                {
                    return "Data not updated";
                }
                else
                {
                    return count + " Rows Affected";
                }
            }
            catch (MySqlException)
            {
                throw;
            }
        }
        
        [CustomAuth]
        [HttpGet]
        [Route("api/DeleteOneWayTour/{id}")]
        public string DeleteData([FromUri]int id)
        {
            try
            {
                connection.Open();
                MySqlCommand cmd = new MySqlCommand($"delete from oneway where id = {id}", connection);
                int count = cmd.ExecuteNonQuery();

                if (count <= 0)
                {
                    return "Data is not deleted";
                }
                else
                {
                    return count + " Row Affected";
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
