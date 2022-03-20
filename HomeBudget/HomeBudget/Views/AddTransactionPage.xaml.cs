using HomeBudget.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeBudget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTransactionPage : ContentPage
    {
        public List<Account> Accounts { get; set; }

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

        public string Name { get; set; }
        public string Description { get; set; }
       // public float? Price { get; set; } = null;
        public string Price { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        //public Account SelectedAccount { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedTypes { get; set; }

        public AddTransactionPage(string type)
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                Accounts = await Services.DatabaseConnection.GetAccounts();
            }).Wait();

            SelectedTypes = type;

            BindingContext = this;
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if (Name == null || Price == null)
            {
                await DisplayAlert("Dodawanie", "Popraw dane!", "Ok");
                return;
            }

            await Services.DatabaseConnection.AddTransaction(new Models.Transaction
            {
                Name = Name,
                Description = Description,
                //Price = (float)Price,
                Price = float.Parse(Price),
                Category = SelectedCategory,
                Date = Date,
                Type = SelectedTypes,
                //Account = SelectedAccount.Id
            });

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

            await Navigation.PopToRootAsync();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private void TransactionPrice_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            int ala = 1;
            ala += 1;

        }

        private void TransactionPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            int ala = 1;
            ala += 1;
        }
    }
}