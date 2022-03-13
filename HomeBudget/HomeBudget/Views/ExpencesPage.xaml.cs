using System;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HomeBudget.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ExpencesPage : ContentPage
    {
        public ObservableCollection<string> BillListFilters { get; set; } = new ObservableCollection<string>
        {
            "Przychody",
            "Wydatki",
            "Płatności",
            "Wszystkie"
        };

        public ObservableCollection<Models.Transaction> TransactionsList { get; set; }

        public ObservableCollection<Models.Payment> PaymentsList { get; set; }
        public Models.Transaction CurrentTransactionItem { get; set; }
        public Models.Payment CurrentPaymentItem { get; set; }
        public Button CurrentCheck { get; set; } = new Button { Text = "-" };

        public ExpencesPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void updateTransactionList()
        {
            switch (CurrentCheck.Text)
            {
                case "Przychody":
                    TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetIncomeTransactions());
                    break;
                case "Wydatki":
                    TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetExpensesTransactions());
                    break;
                case "Płatności":
                    PaymentsList = new ObservableCollection<Models.Payment>(await Services.DatabaseConnection.GetGlobalPayments());
                    break;
                default:
                    TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());
                    break;
            }

            expensesList.ItemsSource = TransactionsList;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            updateTransactionList();
        }

        private async void Filter_Clicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            if (btn != null && btn.Text != CurrentCheck.Text)
            {
                btn.Style = (Style)Application.Current.Resources["MainButtonUnChecked"];
                CurrentCheck.Style = (Style)Application.Current.Resources["MainButtonChecked"];
                CurrentCheck = btn;

                switch (btn.Text)
                {
                    case "Przychody":
                        TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetIncomeTransactions());
                        expensesList.ItemsSource = TransactionsList;
                        break;
                    case "Wydatki":
                        TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetExpensesTransactions());
                        expensesList.ItemsSource = TransactionsList;
                        break;
                    case "Płatności":
                        PaymentsList = new ObservableCollection<Models.Payment>(await Services.DatabaseConnection.GetGlobalPayments());
                        expensesList.ItemsSource = PaymentsList;
                        break;
                    default:
                        TransactionsList = new ObservableCollection<Models.Transaction>(await Services.DatabaseConnection.GetGlobalTransactions());
                        expensesList.ItemsSource = TransactionsList;
                        break;
                }

                
                selectedFilter.Text = btn.Text;

                CurrentCheck = btn;
            }
        }

        private async void addTransaction_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new AddTransactionPage());
            await Navigation.PushAsync(new SelectTransactionToAdd());
        }

        private async void editTransaction_Clicked(Object sender, EventArgs e)
        {
            if (CurrentTransactionItem != null)
            {
                await Navigation.PushAsync(new UpdateTransactionPage(CurrentTransactionItem.Id));
            }
            else if(CurrentPaymentItem != null)
            {
                await Navigation.PushAsync(new UpdatePaymentPage(CurrentPaymentItem.Id));
            }    
            else
            {
                await DisplayAlert("Edycja", "Nie zaznaczono elementu do edycji!", "Ok");
            }
        }

        private async void deleteTransaction_Clicked(object sender, EventArgs e)
        {
            if (CurrentTransactionItem != null)
            {
                bool answer = await DisplayAlert("Usuwanie", "Czy na pewno chcesz usunąć " + CurrentTransactionItem.Name, "Tak", "Nie");

                if (answer)
                {
                    await Services.DatabaseConnection.DeleteTransaction(CurrentTransactionItem.Id);
                    updateTransactionList();
                }
            }
            else if(CurrentPaymentItem != null)
            {
                bool answer = await DisplayAlert("Usuwanie", "Czy na pewno chcesz usunąć " + CurrentPaymentItem.Name, "Tak", "Nie");
                if (answer)
                {
                    await Services.DatabaseConnection.DeletePayment(CurrentPaymentItem.Id);
                    updateTransactionList();
                }
            }
            else
            {
                await DisplayAlert("Usuwanie", "Nie zaznaczono elementu do usunięcia!", "Ok");
            }
        }
        private void expemcesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GroupableItemsView item = sender as GroupableItemsView;
            Models.Transaction tr = null;
            Models.Payment pay = null;
            if (item != null)
            {
                tr = item.SelectedItem as Models.Transaction;
                pay = item.SelectedItem as Models.Payment;
            }
            if (tr != null)
            {
                CurrentTransactionItem = tr;
                return;
            }
            else if(pay != null)
            {
                CurrentPaymentItem = pay;
            }
            


        }

        private void hideFilters_Clicked(object sender, EventArgs e)
        {
            if (filtersList.IsVisible)
            {
                filtersList.IsVisible = false;
                selectedFilter.IsVisible = true;
            }
            else
            {
                filtersList.IsVisible = true;
                selectedFilter.IsVisible = false;
            }
        }
    }
}