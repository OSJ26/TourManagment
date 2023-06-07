using System;

namespace TnTSystem.Models
{
    public class OneWay
    {
        public int Id { get; set; }

        public string Source { get; set; }

        public string Destination { get; set; }

        public int Price { get; set; }

        public string stops { get; set; }

        public DateTime Date_Tour { get; set; }

        public string Other_notes { get; set; }

        public string Pickup_Point { get; set; }

        public string Drop_point { get; set; }

        public int Max_Passanger { get; set; }
    }
}