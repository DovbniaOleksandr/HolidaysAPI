﻿using System.Text.Json.Serialization;

namespace HolidaysAPI.Models.Entities
{
    public class Country
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }
    }
}
