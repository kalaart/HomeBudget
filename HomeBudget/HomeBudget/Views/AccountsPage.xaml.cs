using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeBudget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountsPage : ContentPage
    {
        public ObservableCollection<Models.Account> AccountsList { get; set; }
        public AccountsPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            AccountsList = new ObservableCollection<Models.Account>(await Services.DatabaseConnection.GetAccounts());
            accountsList.ItemsSource = AccountsList;
            float sum = await Services.DatabaseConnection.GetFunctionResult("SELECT SUM(Balance) FROM \"Account\"");
            totalMoney.Text = sum.ToString("Total: 0 PLN");
        }

        private void accountsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}