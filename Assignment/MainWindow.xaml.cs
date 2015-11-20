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
            Sale sale;

            try
            {
                sale = new Sale(vehiclePrice, tradeInValue);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }


            if ((bool)war3.IsChecked) { sale.Price *= Constants.WARRANTY_3_YEARS; }
            else if ((bool)war5.IsChecked) { sale.Price *= Constants.WARRANTY_5_YEARS; }

            sale.Price += getExtras();

            try
            {
                sale.CalculateTotal(subTotal, gstTotal, total);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            dailyReport.IsEnabled = true;

        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] textBox = new TextBox[] {
                customerName,
                customerPhone,
                vehiclePrice,
                tradeInValue,
            };
            Label[] label = new Label[] {
                subTotal,
                gstTotal,
                total,
            };

            // Prompt user if they want to reset
            if (MessageBox.Show("Are you sure you want to reset?", "Reset form", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                // Clear all values
                foreach (TextBox t in textBox)
                {
                    t.Clear();
                }
                foreach (Label l in label)
                {
                    l.Content = null;
                }
                customerDetails.IsEnabled = true; // Enable Customer Details
                customerName.Focus(); // Set focus
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerName.Focus(); // Set focus to Customer Name Textbox
        }

        private void View_Report(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Sale Count: " + Sale.ReportCount.ToString() + "\n\nTotal Sales: " + Sale.TotalSales.ToString("C")); // Show sales report
        }

        private decimal getExtras()
        {
            decimal total = 0;
            if (!(bool)windowTinting.IsChecked && !(bool)ducoProtection.IsChecked && !(bool)gps.IsChecked && !(bool)soundSystem.IsChecked) { return 0m; } // If all are unchecked
            else
            { 
                if ((bool)windowTinting.IsChecked) { total += Constants.WINDOW_TINTING; } // If windowTiniting is checked add 150 to total
                if ((bool)ducoProtection.IsChecked) { total += Constants.DUCO_PROTECTION; } // If ducoProtection is checked add 180 to total
                if ((bool)gps.IsChecked) { total += Constants.GPS_NAVIGATIONAL_SYSTEM; } // If gps is checked add 320 to total
                if ((bool)soundSystem.IsChecked) { total += Constants.DELUX_SOUND_SYSTEM; } // If soundSystem is checked add 350 to total
                return total; // return total
            }
        }
    }
}
