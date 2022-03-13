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
    public partial class SelectTransactionToAdd : ContentPage
    {
        public SelectTransactionToAdd()
        {
            InitializeComponent();
        }

        private async void Btn_Clicked(object sender, EventArgs e)
        {
            Button button = sender as Button;

            switch(button.Text)
            {
                case "Przychody":
                    await Navigation.PushAsync(new AddTransactionPage("Przychody"));
                    break;

                case "Wydatki":
                    await Navigation.PushAsync(new AddTransactionPage("Wydatki"));
                    break;

                case "Płatności":
                    await Navigation.PushAsync(new AddPaymentPage());
                    break;
            }
        }
    }
}