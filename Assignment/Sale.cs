using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Assignment
{
    public class Sale
    {
        public decimal Price { get; set; }
        public decimal Trade { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GstTotal { get; set; }
        public decimal Total { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public static int ReportCount { get; set; } = 0;
        public static decimal TotalSales { get; set; } = 0m;
        public static decimal Average { get; set; } = 0m;

        public Sale(TextBox _priceBox, TextBox _tradeBox, TextBox _name, TextBox _phone)
        {
            try
            {
                Price = Parse(_priceBox);
                Trade = Parse(_tradeBox);
                Name = _name.Text;
                Phone = _phone.Text;
            }
            catch
            {
                throw;
            }
        }

        private decimal Parse(TextBox txt)
        {
            decimal var;

            try
            {
                if (!String.IsNullOrWhiteSpace(txt.Text))
                {
                    var = decimal.Parse(txt.Text);
                }
                else
                {
                    var = 0m;
                }
            }
            catch
            {
                txt.Clear();
                txt.Focus();
                throw new Exception("Error! Value of " + txt.ToolTip.ToString() + " required as a decimal");
            }

            return var;
        }

        public void CalculateTotal(Label subTotal, Label gstTotal, Label total)
        {
            if (Trade > Price)
            {
                SubTotal = 0;
                MessageBox.Show("Trade-In is greater than Vehicle Price, no refund will be given");
            }
            else
            {
                SubTotal = Price - Trade;
            }

            GstTotal = Price * Constants.GST;

            Total = SubTotal + GstTotal;

            ReportCount++;
            TotalSales += Total;
            Average = TotalSales / ReportCount;

            subTotal.Content = SubTotal.ToString("C");
            gstTotal.Content = GstTotal.ToString("C");
            total.Content = Total.ToString("C");
        }

        public override string ToString()
        {
            string returnString = String.Format("Name {0}, Phone {1}, Price {2}, Trade {3}, Total {4}", Name, Phone, Price, Trade, Total);
            return returnString;
        }
    }
}
