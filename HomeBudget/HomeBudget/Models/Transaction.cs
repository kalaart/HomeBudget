using SQLite;
using System;

namespace HomeBudget.Models
{
    public class Transaction
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }

        public string Category { get; set; }

        public DateTime Date { get; set; }

        public string Type { get; set; }

        //public int Account { get; set; }
    }
}
