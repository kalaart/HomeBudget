using HomeBudget.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeBudget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateTransactionPage : ContentPage
    {
        public List<Account> Accounts { get; set; }

        public Transaction Transaction { get; set; }

        public List<string> Categorys { get; set; } = new List<string>
        {
            "Dom",
            "Rachunki",
            "Jedzenie",
            "Edukacja",
            "Zdrowie",
            "Zakupy",
            "Rozrywka",
            "Pozostałe"
        };

        public List<string> Types { get; set; } = new List<string>
        {
            "Przychody",
            "Wydatki",
        };

        public int ID { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public float? Price { get; set; } = null;
        public DateTime Date { get; set; } = DateTime.Now;

        //public Account SelectedAccount { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedTypes { get; set; }

        public UpdateTransactionPage(int id)
        {
            ID = id;
            InitializeComponent();
            Task.Run(async () =>
            {
                Accounts = await Services.DatabaseConnection.GetAccounts();
            }).Wait();

            Task.Run(async () =>
            {
                Transaction = await Services.DatabaseConnection.GetTransactionById(ID);
            }).Wait();

            if (Transaction != null)
            {
                Name = Transaction.Name;
                Description = Transaction.Description;
                Price = Transaction.Price;
                Date = Transaction.Date;
                SelectedCategory = Transaction.Category;
                SelectedTypes = Transaction.Type;
            }

            BindingContext = this;
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            Transaction.Id = ID;
            Transaction.Name = Name;
            Transaction.Description = Description;
            Transaction.Price = (float)Price;
            Transaction.Date = Date;
            Transaction.Category = SelectedCategory;
            Transaction.Type = SelectedTypes;

            await Services.DatabaseConnection.UpdateTransaction(Transaction);

            /*if (SelectedTypes == "Przychód")
            {
                SelectedAccount.Balance += (float)Price;
            }
            else
            {
                SelectedAccount.Balance -= (float)Price;
            }
            */
            //await Services.DatabaseConnection.UpdateAccount(SelectedAccount);
            await Navigation.PopAsync();
        }

        private async void DeleteBtn_Clicked(object sender, EventArgs e)
        {
            await Services.DatabaseConnection.DeleteTransaction(ID);

            await Navigation.PopAsync();
        }
        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}