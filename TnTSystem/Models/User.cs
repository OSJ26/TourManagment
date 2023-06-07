using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnTSystem.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string City { get; set; }

        public string MobNum { get; set; }

        public string Role { get; set; }
    }
}