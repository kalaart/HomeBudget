using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeBudget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddPaymentPage : ContentPage
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
        public float? Price { get; set; }
        public string SelectedType { get; set; }

        NavigationPage navPage;
        public AddPaymentPage()
        {
            InitializeComponent();

            SelectedType = "Płatności";
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

            await Services.DatabaseConnection.AddPayment(new Models.Payment
            {
                Name = Name,
                Description = Description,
                Price = (float)Price,
                Date = DateTime,
                Type = SelectedType
            });

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