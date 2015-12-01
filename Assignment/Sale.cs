using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Assignment
{

    /*
        Sale Class

        For some reason I decided it would be 'better' to make each sale into its own object.
        Technnically it works, and each object is injected into the SaleList list, but as
        it stands right now I don't actually have any plans to save, export, review, modify
        or anything else with them as it is :/
    */
    public class Sale
    {
        public decimal Price { get; set; }
        public decimal Trade { get; set; }
        public decimal SubTotal { get; set; }
        public decimal GstTotal { get; set; }
        public decimal Total { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public decimal Warranty { get; set; }
        public decimal Insurance { get;  set;}
        public decimal Extras { get; set; }
        public static int ReportCount { get; set; }
        public static decimal TotalSales { get; set; }

        private List<Extra> extraList = new List<Extra>();

        public Sale(TextBox _priceBox, TextBox _tradeBox, TextBox _name, ComboBox _phone)
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
                throw; // Throw Parse error to be caught by parent
            }
        }

        private decimal Parse(TextBox txt)
        {
            decimal var;
            try
            {
                if (!String.IsNullOrWhiteSpace(txt.Text))
                {
                    // Try to parse param if it's not null
                    var = decimal.Parse(txt.Text);
                }
                else
                {
                    var = 0m;
                }
            }
            catch
            {
                // Clear and focus on error location. Prompt user as to where the error occurred using the ToolTip
                txt.Clear();
                txt.Focus();
                throw new Exception("Error! Value of " + txt.ToolTip.ToString() + " required as a decimal");
            }
            return var;
        }

        public void OutputTotal(Label subTotal, Label gstTotal, Label total)
        {
            // Calculate subtotal
            SubTotal = Price - Trade + Warranty + Extras + Insurance;
            if (SubTotal < 0) {
                MessageBox.Show("Total is less than 0, no refunds will be given.");
                SubTotal = 0;
            }

            /*
                For some reason GST is calculated based on the SubTotal, so you could buy
                an infinitely priced car and pay no tax as long as you trade in an infitely
                priced car in return.
            */
            AddGST();
            Total = SubTotal + GstTotal;

            ReportCount++; // Iterate ReportCount
            TotalSales += Total; // Add new sale to TotalSales

            // Apply figures
            subTotal.Content = SubTotal.ToString("C");
            gstTotal.Content = GstTotal.ToString("C");
            total.Content = Total.ToString("C");
        }

        private class Extra
        {
            public bool Selected { get; set; }
            public decimal Price { get; set; }
            public string Description { get; set; }

            public Extra(bool selected, decimal price, string description) // Constructor
            {
                Selected = selected;
                Price = price;
                Description = description;
            }
        }

        /* 
            Extras Constructor

            Extras are created as objects, I had planned on doing some sort of
            proper sale object, including sub objects for the extras.
        */
        public void AddExtras(bool tinting, bool duco, bool gps, bool sound)
        {
            /*
                Add each extra to the extraList list.
                Each extra also has a description.
                This currently does nothing :)
            */
            extraList.Add(new Extra(tinting, Constants.WINDOW_TINTING, "Window Tinting"));
            extraList.Add(new Extra(duco, Constants.DUCO_PROTECTION, "Duco Protection"));
            extraList.Add(new Extra(gps, Constants.GPS_NAVIGATIONAL_SYSTEM, "GPS Navigational System"));
            extraList.Add(new Extra(sound, Constants.DELUX_SOUND_SYSTEM, "Delux Sound System"));

            GetExtras(); // Calculate Extras
        }

        public void GetExtras()
        {
            decimal total = 0m;
            foreach (Extra e in extraList)
            {
                /*
                    If Extra (e) is selected then increment total by e.Price
                    Else increment by 0
                */
                total += (e.Selected) ? e.Price : 0;
            }
            Extras = total;
        }

        private string GetSelectedExtras()
        {
            string list = "";
            foreach (Extra e in extraList)
            {
                if (e.Selected)
                {
                    list += e.Description + ", ";
                }
            }
            list = list.TrimEnd(',', ' ');
            if (list == "")
            {
                list = "~";
            }
            return list;
        }

        public void AddInsurance(decimal insurance)
        {
            // Calculate Insurance
            // This is largely superficial
            Insurance = (Price + Extras) * insurance;
        }
        public void AddWarranty(decimal warranty)
        {
            // Calculate Warranty
            // This is largely superficial
            Warranty = Price * warranty;
        }

        public void AddGST()
        {
            // Calculate GST
            // This is largely superficial
            // Also SubToal, WTF
            GstTotal = SubTotal * Constants.GST;
        }

        private string CreateArrayEntry(string value, string cat = "")
        {
            // Define how many spaces are required
            int needs;
            if (cat.Length == 0) { needs = 0; }
            else { needs = 34 - cat.Length - value.Length; }

            string s = cat;
            for (int i = 0; i < needs; i++)
            {
                s += " ";
            }
            s += value;
            return s;
        }

        public Array CreateSummary()
        {
            String[] a = new String[23];
            a[0] = "CUSTOMER";
            a[1] = CreateArrayEntry(Name, "Name:");
            a[2] = CreateArrayEntry(Phone, "Phone:");
            a[3] = "";
            a[4] = "VEHICLE";
            a[5] = CreateArrayEntry(Price.ToString("C"), "Total:");
            a[6] = CreateArrayEntry("-" + Trade.ToString("C"), "Trade:");
            a[7] = "";
            a[8] = "EXTRAS";
            a[9] = CreateArrayEntry(GetSelectedExtras());
            a[10] = CreateArrayEntry(Extras.ToString("C"), "Extras Total:");
            a[11] = "";
            a[12] = "WARRANTY & INSURANCE";
            a[13] = CreateArrayEntry(Warranty.ToString("C"), "Warranty:");
            a[14] = CreateArrayEntry(Insurance.ToString("C"), "Insurance:");
            a[15] = "";
            a[16] = "";
            a[17] = "TOTALS";
            a[18] = CreateArrayEntry(SubTotal.ToString("C"), "Sub Total:");
            a[19] = CreateArrayEntry(GstTotal.ToString("C"), "GST (10%):");
            a[20] = CreateArrayEntry(Total.ToString("C"), "Grand Total:");
            a[21] = "";
            a[22] = "Thank you for using Motor Manager!";
            return a;
        }
        // Calculate GST
        // This is largely superficial
        // Also SubToal, WTF
        public void GetGST()
        {
            GstTotal = SubTotal * Constants.GST;
        }
    }
}
