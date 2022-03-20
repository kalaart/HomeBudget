using HomeBudget.Models;
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
    public partial class UpdatePaymentPage : ContentPage
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public float? Price { get; set; }
        public string SelectedType { get; set; }

        public Payment Payment { get; set; }

        public UpdatePaymentPage(int id)
        {
            InitializeComponent();

            ID = id;

            Task.Run(async () =>
            {
                Payment = await Services.DatabaseConnection.GetPaymentById(ID);
            }).Wait();

            if (Payment != null)
            {
                Name = Payment.Name;
                Description = Payment.Description;
                Price = Payment.Price;
                Date = Payment.Date;
                SelectedType = Payment.Type;
            }

            BindingContext = this;
        }

        private async void SaveBtn_Clicked(object sender, EventArgs e)
        {
            if (Name == null || Price == null)
            {
                await DisplayAlert("Edycja", "Popraw dane!", "Ok");
                return;
            }

            Payment.Id = ID;
            Payment.Name = Name;
            Payment.Description = Description;
            Payment.Price = (float)Price;
            Payment.Date = Date;
            Payment.Type = SelectedType;

            await Services.DatabaseConnection.UpdatePayment(Payment);

            await Navigation.PopToRootAsync();
        }

        private async void CancelBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}