﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnTSystem.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Rating { get; set; }
        public DateTime date { get; }
    }
}