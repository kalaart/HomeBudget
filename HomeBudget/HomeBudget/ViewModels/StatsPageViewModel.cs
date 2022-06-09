using Microcharts;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;


namespace HomeBudget.ViewModels
{
    class StatsPageViewModel : BaseViewModel
    {

        // Top Part 
        private DateTime _currentShowDate = DateTime.Now;
        public DateTime CurrentShowDate
        {
            get { return _currentShowDate; }
            set
            {
                if (_currentShowDate != value)
                {
                    _currentShowDate = value;
                    OnPropertyChanged(nameof(CurrentShowDate));
                }
            }
        }

        private Style _colorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
        public Style ColorNextButton
        {
            get { return _colorNextButton; }
            set
            {
                if (_colorNextButton != value)
                {
                    _colorNextButton = value;
                    OnPropertyChanged(nameof(ColorNextButton));
                }
            }
        }

        private Style _colorPrevButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
        public Style ColorPrevButton
        {
            get { return _colorPrevButton; }
            set
            {
                if (_colorPrevButton != value)
                {
                    _colorPrevButton = value;
                    OnPropertyChanged(nameof(ColorPrevButton));
                }
            }
        }

        // Chart Part 
        private string _currentAppliedFilter = "Wszystko";
        public string CurrentAppliedFilter
        {
            get { return _currentAppliedFilter; }
            set
            {
                if (_currentAppliedFilter != value)
                {
                    _currentAppliedFilter = value;
                    ApplyeOverView.Execute(value);
                    OnPropertyChanged(nameof(CurrentAppliedFilter));
                }
            }
        }

        public Chart GrafData { get; set; }

        // Info Part 
        private float _balance;
        public float Balance
        {
            get { return _balance; }
            set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged(nameof(Balance));
                }
            }
        }

        private string _totalMessage;
        public string TotalMessage
        {
            get { return _totalMessage; }
            set
            {
                if (_totalMessage != value)
                {
                    _totalMessage = value;
                    OnPropertyChanged(nameof(TotalMessage));
                }
            }
        }

        private Color _balanceColor;
        public Color BalanceColor
        {
            get { return _balanceColor; }
            set
            {
                if (_balanceColor != value)
                    _balanceColor = value;
                OnPropertyChanged(nameof(BalanceColor));
            }
        }

        public float IncomeSum { get; set; }
        public float ExpencesSum { get; set; }

        // Commands 
        public ICommand PrevMonth { get; private set; }
        public ICommand NextMonth { get; private set; }
        public ICommand ApplyeOverView { get; private set; }

        public async void ValueChangeMethod(string grafType)
        {

            DateTime dtStart = new DateTime(CurrentShowDate.Year, CurrentShowDate.Month, 1);
            DateTime dtStop = new DateTime(CurrentShowDate.Year, CurrentShowDate.Month, 1);
            dtStop = dtStop.AddMonths(1);

            var listOfIncome = await Services.DatabaseConnection.GetIncomeTransactions();

            IncomeSum = 0;

            foreach (var income in listOfIncome)
            {
                if (income.Date.CompareTo(dtStart) >= 0 && income.Date.CompareTo(dtStop) <= 0)
                {
                    IncomeSum += income.Price;
                }
            }

            var listOfExpenses = await Services.DatabaseConnection.GetExpensesTransactions();

            ExpencesSum = 0;

            foreach (var expense in listOfExpenses)
            {
                if (expense.Date.CompareTo(dtStart) >= 0 && expense.Date.CompareTo(dtStop) <= 0)
                {
                    ExpencesSum += expense.Price;
                }
            }

            //Balance = IncomeSum - ExpencesSum;

            if (grafType == "Wszystko")
            {
                GrafData = await Services.ChartGenerator.GetOverView(CurrentShowDate);
                //DateTime dt = new DateTime(CurrentShowDate.Year,CurrentShowDate.Month,1);
                //IncomeSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Przychody\"");// AND (Date BETWEEN " + dt.ToString("O") + " AND " + dt.AddMonths(1).ToString("o") + "))");
                //ExpencesSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Wydatki\"");// AND (Date BETWEEN " + dt.ToString("yyyy-MM-dd") + " AND " + dt.AddMonths(1).ToString("yyyy-MM-dd") + "))");
                Balance = IncomeSum - ExpencesSum;
                if (Balance < 0)
                {
                    BalanceColor = Color.Red;
                }
                else
                {
                    BalanceColor = Color.Green;
                }
                TotalMessage = "Bilans: ";
            }
            else if (grafType == "Przychody")
            {
                GrafData = await Services.ChartGenerator.GetIncomesGraf(CurrentShowDate);
                Balance = IncomeSum;
                BalanceColor = (Color)Application.Current.Resources["BackgroundDark"];
                TotalMessage = "Przychody: ";
            }
            else if (grafType == "Wydatki")
            {
                GrafData = await Services.ChartGenerator.GetExpencesCategory(CurrentShowDate);
                Balance = ExpencesSum;
                BalanceColor = (Color)Application.Current.Resources["BackgroundDark"];

                TotalMessage = "Wydatki: ";
            }


            OnPropertyChanged(nameof(GrafData));
        }

        public StatsPageViewModel()
        {
            Task.Run(async () =>
            {
                GrafData = await Services.ChartGenerator.GetOverView(CurrentShowDate);
                DateTime dtStart = new DateTime(CurrentShowDate.Year,CurrentShowDate.Month,1);
                DateTime dtStop = new DateTime(CurrentShowDate.Year, CurrentShowDate.Month, 1);

                dtStop.AddMonths(1);


                var listOfIncome = await Services.DatabaseConnection.GetIncomeTransactions();

                IncomeSum = 0;

                foreach (var income in listOfIncome)
                {
                    if(income.Date.CompareTo(dtStart) >= 0 && income.Date.CompareTo(dtStop) <= 0)
                    {
                        IncomeSum += income.Price;
                    }
                }

                var listOfExpenses = await Services.DatabaseConnection.GetExpensesTransactions();

                ExpencesSum = 0;

                foreach (var expense in listOfExpenses)
                {
                    if (expense.Date.CompareTo(dtStart) >= 0 && expense.Date.CompareTo(dtStop) <= 0)
                    {
                        ExpencesSum += expense.Price;
                    }
                }

                Balance = IncomeSum - ExpencesSum;

                if (Balance < 0)
                {
                    BalanceColor = Color.Red;
                }
                else
                {
                    BalanceColor = Color.Green;
                }

                TotalMessage = "Bilans: " + Balance.ToString();
            }).Wait();

            NextMonth = new Command(async =>
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 12, 1);

                if (CurrentShowDate.Month != dt.Month)
                {
                    CurrentShowDate = CurrentShowDate.AddMonths(1);

                    ColorPrevButton = (Style)Application.Current.Resources["MainButtonUnChecked"];

                   // if (CurrentShowDate.Month == dt.Month)
                     //   ColorNextButton = (Style)Application.Current.Resources["MainButtonChecked"];

                    ValueChangeMethod(CurrentAppliedFilter);
                }
                else
                {
                    dt = new DateTime(CurrentShowDate.Year + 1, 1, CurrentShowDate.Day);
                    CurrentShowDate = dt;
                    ColorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    ValueChangeMethod(CurrentAppliedFilter);
                }
            });

            PrevMonth = new Command(async =>
            {
                DateTime dt = new DateTime(DateTime.Now.Year, 1, 1);

                if (CurrentShowDate.Month != dt.Month)
                {
                    CurrentShowDate = CurrentShowDate.AddMonths(-1);
                    ColorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];

                  //  if (CurrentShowDate.Month == dt.Month)
                    //    ColorPrevButton = (Style)Application.Current.Resources["MainButtonChecked"];
                    ValueChangeMethod(CurrentAppliedFilter);
                }
                else
                {
                    dt = new DateTime(CurrentShowDate.Year - 1, 12, CurrentShowDate.Day);

                    CurrentShowDate = dt;
                    ColorNextButton = (Style)Application.Current.Resources["MainButtonUnChecked"];
                    ValueChangeMethod(CurrentAppliedFilter);
                }
            });

            ApplyeOverView = new Command<string>(value =>
                {
                    ValueChangeMethod(value);
                });
        }
    }
}