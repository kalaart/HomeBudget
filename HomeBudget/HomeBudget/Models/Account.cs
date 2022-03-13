using SQLite;

namespace HomeBudget.Models
{
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }

        public float Balance { get; set; }
    }
}
