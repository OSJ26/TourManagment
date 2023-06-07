using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Web.Http;
using TnTSystem.Models;
using System.Text;

namespace TnTSystem.Controllers
{
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("GenerateToken")]
        public IHttpActionResult GenerateToken([FromBody] Login login)
        {
            if (login != null)
            {
                if (IsValidate(login.Email, login.Password))
                {
                    string token = GenerateToken(login.Email, login.Password);
                    return Json(new
                    {
                        Token = token,
                        IncludeIn = "Headers"
                    });
                }
                else
                {
                    return ResponseMessage(new System.Net.Http.HttpResponseMessage(HttpStatusCode.Unauthorized));
                }
            }
            else
            {
                return ResponseMessage(new System.Net.Http.HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        private string GenerateToken(string email, string password)
        {
            DateTime generationDate = DateTime.Now;

            DateTime Expires = DateTime.Now.AddMinutes(30);

            var jwtSecurity = new JwtSecurityTokenHandler();

            var claimsIdentity = new ClaimsIdentity(
                new Claim[]
                     {
                         new Claim("name", email),
                         new Claim("password", password)
                     }
                );

            string secretKey = ConfigurationManager.AppSettings["Secret"].ToString();
            string Auidence = ConfigurationManager.AppSettings["Audience"].ToString();
            string Issuer = ConfigurationManager.AppSettings["Issuer"].ToString();

            var securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(secretKey));
            var signinCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = jwtSecurity.CreateJwtSecurityToken(
                    issuer: Issuer,
                    audience: Auidence,
                    subject: claimsIdentity,
                    issuedAt: generationDate,
                    expires: Expires,
                    signingCredentials: signinCredential
                );
            var tokenWrite = jwtSecurity.WriteToken(token);
            return tokenWrite;
        }

        private bool IsValidate(string email, string password)
        {
            MySqlConnection connection = new MySqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            connection.Open();
            MySqlCommand cmd = new MySqlCommand($"select Id,Name,Email,City,Role from user where email = @e1 and pwd =  @p1 and role = @r1", connection);
            cmd.Parameters.AddWithValue("e1", email);
            cmd.Parameters.AddWithValue("p1", password);
            cmd.Parameters.AddWithValue("r1", "a");

            var data = cmd.ExecuteReader();

            if (data.HasRows)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
