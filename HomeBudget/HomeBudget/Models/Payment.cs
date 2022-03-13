using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace HomeBudget.Models
{
    public class Payment
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }
    }
}
