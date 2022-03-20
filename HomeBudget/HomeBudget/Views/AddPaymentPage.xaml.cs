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
        public DateTime Date { get; set; } = DateTime.Now;
        public float? Price { get; set; }
        public string SelectedType { get; set; }
        public AddPaymentPage()
        {
            InitializeComponent();

            SelectedType = "Płatności";

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
                Date = Date,
                Type = SelectedType
            });

            await Navigation.PopToRootAsync();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}