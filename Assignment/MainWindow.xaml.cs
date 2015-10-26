using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Save_Customer_Details(object sender, RoutedEventArgs e)
        {
            customerDetails.IsEnabled = false;
        }

        private void Close_Application(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Calculate_Totals(object sender, RoutedEventArgs e)
        {
            const decimal GST = 0.1m;
            decimal price = 0, trade = 0, subTotal = 0, gstTotal = 0, total = 0;
            try
            {
                price = decimal.Parse(vehiclePrice.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Error! Vehicle price required as a decimal", "Error!", MessageBoxButton.OK);
                vehiclePrice.Clear();
                vehiclePrice.Focus();
            }

            try
            {
                if (!string.IsNullOrWhiteSpace(tradeInValue.Text))
                {
                    trade = decimal.Parse(tradeInValue.Text);
                }
            }
            catch
            {
                MessageBox.Show("Error! Trade-in price required as a decimal", "Error!", MessageBoxButton.OK);
                vehiclePrice.Clear();
                vehiclePrice.Focus();
            }

            subTotal = price - trade;
            gstTotal = price * GST;
            total = subTotal + gstTotal;

            this.subTotal.Content = subTotal.ToString("C");
            this.gstTotal.Content = gstTotal.ToString("C");
            this.total.Content = total.ToString("C");
        }
    }
}
