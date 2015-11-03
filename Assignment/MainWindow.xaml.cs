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
            if (!(String.IsNullOrEmpty(customerName.Text) || String.IsNullOrEmpty(customerPhone.Text))) // If customer name and phone are not empty
            {
                customerDetails.IsEnabled = false; // Disable the customer details groupbox
                vehiclePrice.Focus(); // Set focus
            }
            else
            {
                MessageBox.Show("Customer details must be filled in before saving"); // Prompt user to enter data

                if (String.IsNullOrEmpty(customerName.Text))
                {
                    customerName.Focus(); // Set focus to Customer Name
                }
                else
                {
                    customerPhone.Focus(); // Set focus to Customer Phone
                }
            }
        }

        private void Close_Application(object sender, RoutedEventArgs e)
        {
            this.Close(); // Close application
        }

        private void Calculate_Totals(object sender, RoutedEventArgs e)
        {
            const decimal GST = 0.1m; // Set constant GST to decimal 0.1
            decimal price = 0, trade = 0, subTotal = 0, gstTotal = 0, total = 0; // Set vehicle price, trade in, sub total, gst total and total total to 0
            try
            {
                price = decimal.Parse(vehiclePrice.Text); // Try to parse decimal
            }
            catch (FormatException)
            {
                MessageBox.Show("Error! Vehicle price required as a decimal", "Error!", MessageBoxButton.OK); // Prompt user with error
                vehiclePrice.Clear(); // Clear textbox
                vehiclePrice.Focus(); // Set focus to textbox
                return; // Exit
            }

            try
            {
                if (!String.IsNullOrWhiteSpace(tradeInValue.Text)) // If trade in isn't null or whitespace
                {
                    trade = decimal.Parse(tradeInValue.Text); // Try to parse decimal
                }
            }
            catch
            {
                MessageBox.Show("Error! Trade-in price required as a decimal", "Error!", MessageBoxButton.OK); // Prompt user with error
                vehiclePrice.Clear(); // Clear textbox
                vehiclePrice.Focus(); // Set focus to textbox
                return; // Exit
            }

            if ((bool)war3.IsChecked) // If 3 year warrenty is checked
            {
                price += price*0.1m;
            }
            else if ((bool)war5.IsChecked) // If 5 year warrenty is checked
            {
                price += price * 0.2m;
            }

            price += getExtras(); // Call getExtras

            if (trade > price) // Business 'Logic'
            {
                subTotal = 0;
            }
            else
            {
                subTotal = price - trade; // Subtotal =  vehicle price - trade in price
            }

            gstTotal = subTotal * GST; // GST = vehicle price * 0.
            total = subTotal + gstTotal; // Total = Subtotal + GST

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
                // Set focus
                vehiclePrice.Focus();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerName.Focus(); // Set focus to Customer Name Textbox
        }

        private void View_Report(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sale Count: " + reportCount.ToString() + "\n\nTotal Sales: " + totalSales.ToString("C")); // Show sales report
        }

        private decimal getExtras()
        {
            decimal total = 0;
            if (!(bool)windowTinting.IsChecked && !(bool)ducoProtection.IsChecked && !(bool)gps.IsChecked && !(bool)soundSystem.IsChecked) { return 0m; } // If all are unchecked
            else
            { 
                if ((bool)windowTinting.IsChecked) { total += 150m; } // If windowTiniting is checked add 150 to total
                if ((bool)ducoProtection.IsChecked) { total += 180m; } // If ducoProtection is checked add 180 to total
                if ((bool)gps.IsChecked) { total += 320m; } // If gps is checked add 320 to total
                if ((bool)soundSystem.IsChecked) { total += 350m; } // If soundSystem is checked add 350 to total
                return total; // return total
            }
        }
    }
}
