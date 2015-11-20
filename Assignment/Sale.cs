using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Assignment
{
    class Sale
    {
        public decimal Price, Trade, SubTotal, GstTotal, Total;
        public static int ReportCount = 0;
        public static decimal TotalSales = 0m;

        public Sale(TextBox _priceBox, TextBox _tradeBox)
        {
            try
            {
                Price = Parse(_priceBox);
                Trade = Parse(_tradeBox);
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
                throw new Exception("Trade-In is greater than Vehicle Price, no refund will be given");
            }
            else
            {
                SubTotal = Price - Trade;
            }

            GstTotal = Price * Constants.GST;

            Total = SubTotal + GstTotal;

            ReportCount++;
            TotalSales += Total;

            subTotal.Content = SubTotal.ToString("C");
            gstTotal.Content = GstTotal.ToString("C");
            total.Content = Total.ToString("C");
        }
    }
}
