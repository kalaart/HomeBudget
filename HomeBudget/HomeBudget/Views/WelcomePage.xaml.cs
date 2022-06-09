using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeBudget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WelcomePage : ContentPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private async void getStarted_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new AddAccountPage());
        }
    }
}