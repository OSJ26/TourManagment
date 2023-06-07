using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TnTSystem.Models
{
    public class Tour
    {
        public int Id { get; set; }

        public string TourName { get; set; }

        public string Tour_date { get; set; }

        public int pack_days { get; set; }

        public int pack_price { get; set; }

        public string pack_pickpoint { get; set; }

        public string pack_DropPoint { get; set; }

        public string other_note { get; set; }

        public int Max_pass { get; set; }

        public string Main_place { get; set; }

        public DateTime Creation_Date { get; }

    }
}