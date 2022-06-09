using HomeBudget.Models;
using SQLite;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HomeBudget.Services
{
    public static class DatabaseConnection
    {
        static SQLiteAsyncConnection db;
        static async Task Init()
        {
            if (db != null)
            {
                return;
            }

            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "expencesDb.db");
            db = new SQLiteAsyncConnection(databasePath);

            await db.CreateTableAsync<Transaction>();
            await db.CreateTableAsync<Account>();
            await db.CreateTableAsync<Payment>();
        }

        public static async Task AddTransaction(Transaction transaction)
        {
            await Init();
            await db.InsertAsync(transaction);
        }
        public static async Task UpdateTransaction(Transaction transaction)
        {
            await Init();
            await db.UpdateAsync(transaction);
        }
        public static async Task DeleteTransaction(int id)
        {
            await Init();
            await db.DeleteAsync<Transaction>(id);
        }

        public static async Task AddPayment(Payment payment)
        {
            await Init();
            await db.InsertAsync(payment);
        }

        public static async Task DeletePayment(int id)
        {
            await Init();
            await db.DeleteAsync<Payment>(id);
        }

        public static async Task UpdatePayment(Payment payment)
        {
            await Init();
            await db.UpdateAsync(payment);
        }

        public static async Task<IEnumerable<Payment>> GetGlobalPayments()
        {
            await Init();
            var trans = await db.QueryAsync<Payment>($"SELECT * FROM \"Payment\" ORDER BY Date DESC");
            return trans;
        }

        public static async Task<Payment> GetPaymentById(int id)
        {
            await Init();
            string query = "SELECT * FROM \"Payment\" WHERE Id == " + id.ToString();
            var trans = await db.FindWithQueryAsync<Payment>(query);

            return trans;
        }

        public static async Task AddAccount(Account account)
        {
            await Init();
            await db.InsertAsync(account);
        }

        public static async Task UpdateAccount(Account acc)
        {
            await Init();
            await db.UpdateAsync(acc);
        }

        public static async Task<List<Account>> GetAccountByName(string name)
        {
            await Init();
            string query = $"SELECT * FROM \"Account\" WHERE Name = \"{name}\"";
            var trans = await db.QueryAsync<Account>(query);

            return trans;
        }

        public static async Task<bool> VerifyIfAccExist(string name)
        {
            await Init();
            List<Account> trans = await GetAccountByName(name);

            return trans.Count == 0 ? true : false;
        }

        public static async Task<IEnumerable<Transaction>> GetGlobalTransactions()
        {
            await Init();
            var trans = await db.QueryAsync<Transaction>($"SELECT * FROM \"Transaction\" ORDER BY Date DESC");
            return trans;
        }

        public static async Task<List<Account>> GetAccounts()
        {
            await Init();
            return await db.Table<Account>().ToListAsync();
        }

        public static async Task<IEnumerable<Transaction>> GetIncomeTransactions()
        {
            await Init();
            var trans = await db.QueryAsync<Transaction>($"SELECT * FROM \"Transaction\" WHERE Type = \"Przychody\" ORDER BY Date DESC");
            return trans;
        }

        public static async Task<IEnumerable<Transaction>> GetExpensesTransactions()
        {
            await Init();
            string query = "SELECT * FROM \"Transaction\" WHERE Type = \"Wydatki\" ORDER BY Date DESC";
            var trans = await db.QueryAsync<Transaction>(query);
            return trans;
        }

        public static async Task<Transaction> GetTransactionById(int id)
        {
            await Init();
            string query = "SELECT * FROM \"Transaction\" WHERE Id == " + id.ToString();
            var trans = await db.FindWithQueryAsync<Transaction>(query);

            return trans;
        }

        public static async Task<float> GetFunctionResult(string query)
        {
            await Init();
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\"" get sum from Income
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\"" get sum from Expense
            
            var incomeSum = await db.ExecuteScalarAsync<float>(query);
            return incomeSum;
        }


        public static async Task<List<Account>> GetAccountsWithBalance()
        {
            await Init();
            string query = "SELECT IFNULL(a.ACC ,b.ACC) as Name , (IFNULL(a.Balance,0) - IFNULL(b.Balance,0)) as Balance FROM (SELECT Account as ACC ,SUM(Price) as Balance FROM \"Transaction\"  WHERE Type = \"Income\" GROUP BY Account) a,(SELECT Account as ACC ,SUM(Price) as Balance FROM \"Transaction\" WHERE Type = \"Expense\" GROUP BY Account) b GROUP BY Name";
            var trans = await db.QueryAsync<Account>(query);

            return trans;
        }
    }
}