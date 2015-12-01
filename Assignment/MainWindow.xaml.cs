using System;
using System.Collections.Generic;
using System.IO;
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

        List<Sale> SaleList; // Create Sale List
        Sale sale; // Create sale

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Save_Customer_Details(object sender, RoutedEventArgs e)
        {
            if (!(String.IsNullOrEmpty(customerName.Text) || String.IsNullOrEmpty(customerPhone.Text))) // If customer name and phone are not empty
            {
                customerDetails.IsEnabled = false; // Disable the customer details groupbox
                Customer newCustomer = new Customer(customerName.Text, customerPhone.Text);
                if (newCustomer.Check())
                {
                    newCustomer.SaveCustomer();
                    customerPhone.ItemsSource = Customer.Phones;
                }
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
            decimal insurance = GetInsurance(); // Return insurance fraction
            decimal warranty = GetWarranty(); // Return warranty fraction

            if (String.IsNullOrWhiteSpace(vehiclePrice.Text)) // Make sure vehicle price has been entered
            {
                MessageBox.Show("Error: Vehicle price is required!");
                vehiclePrice.Focus();
                return;
            }
            try
            {
                sale = new Sale(vehiclePrice, tradeInValue, customerName, customerPhone); // Try to init values
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // Show error message
                return; // Exit
            }

            sale.AddWarranty(warranty); // Add warranty to sale
            sale.AddExtras((bool)windowTinting.IsChecked, (bool)ducoProtection.IsChecked, (bool)gps.IsChecked, (bool)soundSystem.IsChecked); // Add extras to sale
            sale.AddInsurance(insurance); // Add Insurance to sale
            sale.OutputTotal(subTotal, gstTotal, total); // Output calculated totals
            dailyReport.IsEnabled = true; // Enable Daily Reports
            dailyReport_Menu.IsEnabled = true;

            SaleList.Add(sale); // Add Sale to SaleList
            saleSelector.IsEnabled = true; // Enable Sale Selector
            saleSelector.ItemsSource = GetSaleList(); // Set ItemsSource to list of sales
            saleSelector.SelectedIndex = SaleList.Count - 1; // Select new sale

            saleSummary.ItemsSource = sale.CreateSummary(); // Outupt Summary
        }

        private void resetButton_Click(object sender, RoutedEventArgs e)
        {
            TextBox[] textBox = new TextBox[] {
                customerName,
                vehiclePrice,
                tradeInValue,
            };
            Label[] label = new Label[] {
                subTotal,
                gstTotal,
                total,
            };

            // Clear all values
            foreach (TextBox t in textBox) { t.Clear(); }
            foreach (Label l in label) { l.Content = null; }
            customerPhone.Text = "";
            customerDetails.IsEnabled = true; // Enable Customer Details
            customerName.Focus(); // Set focus
            saleSummary.ItemsSource = null; // Clear Listbox
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            customerName.Focus(); // Set focus to Customer Name Textbox
            SaleList = new List<Sale>(); // Init list of sales
            Sale.ReportCount = 0; // Initialise Values
            Sale.TotalSales = 0;
            Customer.FillClass();
            customerPhone.ItemsSource = Customer.Phones;
        }

        private void View_Report(object sender, RoutedEventArgs e)
        {
            Summary sum = new Summary(); // Create new summary  window
            sum.ShowDialog(); // Open summary window as dialog
        }

        private void Open_About(object sender, RoutedEventArgs e)
        {
            About about = new About(); // Create new about window
            about.ShowDialog(); // Open about window as dialog
        }

        private decimal GetInsurance()
        {  
            // Decide what the insurance percent is based on the insurance checkboxes
            decimal insurance = 0;
            if((bool)ins1.IsChecked) { insurance = Constants.INSURANCE_U25; }
            else if ((bool)ins2.IsChecked) { insurance = Constants.INSURANCE_O25; }
            return insurance;
        }

        private decimal GetWarranty()
        {
            // Decide what the warranty is based on the warranty check boxes
            decimal warranty = 0;
            if ((bool)war3.IsChecked) { warranty = Constants.WARRANTY_3_YEARS; }
            else if ((bool)war5.IsChecked) { warranty = Constants.WARRANTY_5_YEARS; }
            return warranty;
        }

        private string[] GetSaleList()
        {
            string[] saleList = new string[SaleList.Count];
            int i = 0;
            foreach (Sale s in SaleList)
            {
                if (String.IsNullOrWhiteSpace(s.Name)) { saleList[i] = "Unnamed - "; }
                else { saleList[i] = s.Name + " - "; }
                saleList[i] += s.Total.ToString("C");
                i++;
            }
            return saleList;
        }

        private Sale GetSale(int sale)
        {
            Sale newSale = SaleList[sale];
            return newSale;
        }

        private void saleSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            saleSummary.ItemsSource = GetSale(saleSelector.SelectedIndex).CreateSummary();
        }

        private class Customer
        {
            static List<Customer> Customers = new List<Customer>(); // List to store customers

            private string Name;
            private string Phone;
            private static string file = "customer.txt"; // File location

            public static void FillClass() // Creates objects from file and inserts them into Customers list
            {
                StreamReader sr = new StreamReader(file);
                string name = sr.ReadLine();
                string phone = sr.ReadLine();
                while (name != null && phone != null) // While name and phone aren't null
                {
                    new Customer(name, phone); // Create new customer
                    name = sr.ReadLine();
                    phone = sr.ReadLine();
                }
                sr.Close(); // Close file
            }

            public Customer(string name, string phone) // Customer constructor
            {
                Name = name;
                Phone = phone;
                Customers.Add(this); // Add new customer to list
            }

            public static string GetName(string phone) // Get name from list of customers
            {
                string name = null;
                foreach (Customer c in Customers)
                {
                    if (c.Phone == phone) // If match is found return the name
                    {
                        name = c.Name;
                    }
                }
                return name;
            }

            static public string[] Phones // Get array of phone numbers for combobox
            {
                get
                {
                    string[] phones = new string[Customers.Count]; // Create array
                    int i = 0;
                    foreach (Customer c in Customers)
                    {
                        phones[i] = c.Phone; // Add customer phone to array
                        i++;
                    }
                    return phones; // Return array
                }
            }

            public void SaveCustomer() // Save customer to file
            {
                if (MessageBox.Show("Do you want to save this customer?", "Save Customer", MessageBoxButton.YesNo) == MessageBoxResult.Yes) // Prompt user to save
                {
                    StreamWriter sw = new StreamWriter(file, true);
                    sw.WriteLine(this.Name);
                    sw.WriteLine(this.Phone);
                    sw.Close();
                }
            }

            public bool Check() // Check to see if current customer is a new customer
            {
                bool unique = true;
                foreach (Customer c in Customers)
                {
                    if ((c.Name == this.Name && c.Phone == this.Phone) && Customers.IndexOf(c) != Customers.Count-1) // If customer is not unique
                    {
                        unique = false;
                    }
                }
                return unique;
            }
        }

        private void customerPhone_SelectionChanged(object sender, SelectionChangedEventArgs e) // On selection change
        {
            customerName.Text = Customer.GetName(customerPhone.SelectedValue?.ToString()); // Set customer name to GetName() if selcted value is not null
        }
    }
}