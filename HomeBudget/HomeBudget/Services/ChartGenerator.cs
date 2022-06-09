using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Entry = Microcharts.ChartEntry;

namespace HomeBudget.Services
{
    public static class ChartGenerator
    {

        public static List<string> Categorys { get; set; } = new List<string>
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

        public static List<string> ChartColors { get; set; } = new List<string>
        {
            "ChartColor1",
            "ChartColor2",
            "ChartColor7",
            "ChartColor8",
            "ChartColor9",
            "ChartColor10",
            "ChartColor3",
            "ChartColor4",
            "ChartColor5",
            "ChartColor6",
        };


        public static async Task<Chart> GetOverView(DateTime fromPeriod)
        {
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Income\"" get sum from Income
            // "SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Expense\"" get sum from Expense

            //var incomeSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Przychody\" ");
            //var expenseSum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Type = \"Wydatki\" ");
            Color color1 = (Color)Application.Current.Resources["ChartColor2"];
            Color color2 = (Color)Application.Current.Resources["ChartColor1"];

            DateTime dtStart = new DateTime(fromPeriod.Year, fromPeriod.Month, 1);
            DateTime dtStop = new DateTime(fromPeriod.Year, fromPeriod.Month, 1);
            dtStop = dtStop.AddMonths(1);

            var listOfIncom = await Services.DatabaseConnection.GetIncomeTransactions();
            float incomeSum = 0;

            foreach (var income in listOfIncom)
            {
                if (income.Date.CompareTo(dtStart) >= 0 && income.Date.CompareTo(dtStop) <= 0)
                    incomeSum += income.Price;
            }

            var listOfExpense = await Services.DatabaseConnection.GetExpensesTransactions();
            float expenseSum = 0;

            foreach (var expense in listOfExpense)
            {
                if (expense.Date.CompareTo(dtStart) >= 0 && expense.Date.CompareTo(dtStop) <= 0)
                    expenseSum += expense.Price;
            }

            List<Entry> entrys = new List<Entry>
            {
                new Entry(incomeSum)
                {
                    Color = SKColor.Parse(color1.ToHex()),
                    ValueLabelColor = SKColor.Parse(color1.ToHex()),
                    Label = "Przychody",
                    ValueLabel = incomeSum.ToString() + " PLN"
                },
                new Entry(expenseSum)
                {
                    Color = SKColor.Parse(color2.ToHex()),
                    ValueLabelColor = SKColor.Parse(color2.ToHex()),
                    Label = "Wydatki",
                    ValueLabel = expenseSum.ToString() + " PLN"
                }
            };

            return new BarChart
            {
                Entries = entrys,
                LabelTextSize = 40f,
                MaxValue = 800,
                Margin = 50,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex())
            };
        }


        public static async Task<Chart> GetIncomesGraf(DateTime fromPeriod)
        {
            var listOfIncom = await Services.DatabaseConnection.GetIncomeTransactions();
            Entry entry;
            List<Entry> entrys = new List<Entry>();
            Color color;
            DateTime dtStart = new DateTime(fromPeriod.Year, fromPeriod.Month, 1);
            DateTime dtStop = new DateTime(fromPeriod.Year, fromPeriod.Month, 1);
            dtStop = dtStop.AddMonths(1);


            int i = 0;
            foreach (var income in listOfIncom)
            {
                if (income.Date.CompareTo(dtStart) >= 0 && income.Date.CompareTo(dtStop) <= 0)
                {
                    color = (Color)Application.Current.Resources[ChartColors[i]];
                    entry = new Entry(income.Price)
                    {
                        Color = SKColor.Parse(color.ToHex()),
                        ValueLabelColor = SKColor.Parse(color.ToHex()),
                        Label = $"{income.Name}",
                        ValueLabel = income.Price.ToString() + " PLN"
                    };

                    entrys.Add(entry);
                    i++;
                }
            }

            return new BarChart { Entries = entrys, LabelTextSize = 40f, BackgroundColor = SKColor.Parse(Color.Transparent.ToHex()) };
        }

        public static async Task<Chart> GetExpencesCategory(DateTime fromPeriod)
        {
            Entry entry;
            List<Entry> entrys = new List<Entry>();
            Color color;
           
            int i = 0;

            var listOfExpenses = await Services.DatabaseConnection.GetExpensesTransactions();

            DateTime dtStart = new DateTime(fromPeriod.Year, fromPeriod.Month, 1);
            DateTime dtStop = new DateTime(fromPeriod.Year, fromPeriod.Month, 1);
            dtStop = dtStop.AddMonths(1);

            foreach (string category in Categorys)
            {
                //var categorySum = await Services.DatabaseConnection.GetFunctionResult($"SELECT SUM(Price) FROM \"Transaction\" WHERE Category = \"{cateory}\" AND Type = \"Wydatki\"");

                float categorySum = 0;

                foreach (var expense in listOfExpenses)
                {
                    if (expense.Category == category && expense.Date.CompareTo(dtStart) >= 0 && expense.Date.CompareTo(dtStop) <= 0)
                        categorySum += expense.Price;
                }

                if (categorySum > 0)
                {
                    color = (Color)Application.Current.Resources[ChartColors[i]];
                    entry = new Entry(categorySum)
                    {
                        Color = SKColor.Parse(color.ToHex()),
                        ValueLabelColor = SKColor.Parse(color.ToHex()),
                        Label = $"{category}",
                        ValueLabel = categorySum.ToString() + " PLN"
                    };


                    entrys.Add(entry);
                    i++;
                }
            }

            return new BarChart
            {
                Entries = entrys,
                LabelTextSize = 40f,
                Margin = 50,
                LabelOrientation = Orientation.Horizontal,
                ValueLabelOrientation = Orientation.Horizontal,
                BackgroundColor = SKColor.Parse(Color.Transparent.ToHex())
            };
        }
    }
}
