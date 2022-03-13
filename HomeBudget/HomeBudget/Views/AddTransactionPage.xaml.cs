using HomeBudget.Models;
using System;
using System.Collections.Generic;
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
        public float? Price { get; set; } = null;
        public DateTime Date { get; set; } = DateTime.Now;

        //public Account SelectedAccount { get; set; }
        public string SelectedCategory { get; set; }
        public string SelectedTypes { get; set; }

        private NavigationPage navPage;
        public AddTransactionPage(string type)
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                Accounts = await Services.DatabaseConnection.GetAccounts();
            }).Wait();

            SelectedTypes = type;
            navPage = new NavigationPage(new MainTabbedPage());

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
                Price = (float)Price,
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

            Application.Current.MainPage = navPage;
            await navPage.PopAsync();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            Application.Current.MainPage = navPage;
            await navPage.PopAsync();
        }
    }
}