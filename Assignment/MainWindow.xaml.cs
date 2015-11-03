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
        public int reportCount = 0;
        public decimal totalSales = 0m;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Save_Customer_Details(object sender, RoutedEventArgs e)
        {
            // Disable the customer details groupbox
            customerDetails.IsEnabled = false;
            // Set focus
            vehiclePrice.Focus();
        }

        private void Close_Application(object sender, RoutedEventArgs e)
        {
            // Close application
            this.Close();
        }

        private void Calculate_Totals(object sender, RoutedEventArgs e)
        {
            // Set constant GST to decimal 0.1
            const decimal GST = 0.1m;
            // Set vehicle price, trade in, sub total, gst total and total total to 0
            decimal price = 0, trade = 0, subTotal = 0, gstTotal = 0, total = 0;
            try
            {
                // Try to parse decimal
                price = decimal.Parse(vehiclePrice.Text);
            }
            catch (FormatException)
            {
                // Prompt user with error
                MessageBox.Show("Error! Vehicle price required as a decimal", "Error!", MessageBoxButton.OK);
                // Clear textbox
                vehiclePrice.Clear();
                // Set focus to textbox
                vehiclePrice.Focus();
                // Exit
                return;
            }

            try
            {
                // If trade in isn't null or whitespace
                if (!string.IsNullOrWhiteSpace(tradeInValue.Text))
                {
                    // Try to parse decimal
                    trade = decimal.Parse(tradeInValue.Text);
                }
            }
            catch
            {
                // Prompt user with error
                MessageBox.Show("Error! Trade-in price required as a decimal", "Error!", MessageBoxButton.OK);
                // Clear textbox
                vehiclePrice.Clear();
                // Set focus to textbox
                vehiclePrice.Focus();
                // Exit
                return;
            }

            // If 3 year warrenty is checked
            if ((bool)war3.IsChecked)
            {
                price += price*0.1m;
            }
            // If 5 year warrenty is checked
            else if ((bool)war5.IsChecked)
            {
                price += price * 0.2m;
            }

            // Call getExtras
            price += getExtras();

            // Business 'Logic'
            if (trade > price)
            {
                subTotal = 0;
            }
            else
            {
                // Subtotal =  vehicle price - trade in price
                subTotal = price - trade;
            }
            // GST = vehicle price * 0.1
            gstTotal = subTotal * GST;
            // Total = Subtotal + GST
            total = subTotal + gstTotal;

            // Set labels to calculated values
            this.subTotal.Content = subTotal.ToString("C");
            this.gstTotal.Content = gstTotal.ToString("C");
            this.total.Content = total.ToString("C");

            reportCount++;
            dailyReport.IsEnabled = true;
            totalSales += total;
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            // Prompt user if they want to reset
            if (MessageBox.Show("Are you sure you want to reset?", "Reset form", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // Clear all values
                customerName.Clear();
                customerPhone.Clear();
                vehiclePrice.Clear();
                tradeInValue.Clear();
                subTotal.Content = null;
                gstTotal.Content = null;
                total.Content = null;
                // Enable Customer Details
                customerDetails.IsEnabled = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Set focus to Customer Name Textbox
            customerName.Focus();
        }

        private void View_Report(object sender, RoutedEventArgs e)
        {
            // Show sales report
            MessageBox.Show("Sale Count: " + reportCount.ToString() + "\n\nTotal Sales: " + totalSales.ToString("C"));
        }

        private decimal getExtras()
        {
            decimal total = 0;
            // If all are unchecked
            if (!(bool)windowTinting.IsChecked && !(bool)ducoProtection.IsChecked && !(bool)gps.IsChecked && !(bool)soundSystem.IsChecked) { return 0m; }
            else
            {
                // If windowTiniting is checked add 150 to total
                if ((bool)windowTinting.IsChecked) { total += 150m; }
                // If ducoProtection is checked add 180 to total
                if ((bool)ducoProtection.IsChecked) { total += 180m; }
                // If gps is checked add 320 to total
                if ((bool)gps.IsChecked) { total += 320m; }
                // If soundSystem is checked add 350 to total
                if ((bool)soundSystem.IsChecked) { total += 350m; }
                // return total
                return total;
            }
        }
    }
}
