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
using System.Windows.Shapes;

using System.Linq;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class NewPalletWindow : Window
    {
        string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
        SqlConnection con = null;
        String palletId = null;
        String formName = null;
        PalletManager pm = null;
        public NewPalletWindow(String formName, PalletManager pm)
        {
            
            InitializeComponent();
            this.formName = formName;
            this.pm = pm;
            if (con == null)
            {
                con = new SqlConnection(ConString);
                con.Open();
            }

            
            String nextPalletId = "SELECT value+1 AS palID FROM Variabile WHERE varName='lastPalletId'";
            Console.Out.WriteLine("Query for next pallet id: " + nextPalletId);
            SqlCommand cmd0 = new SqlCommand(nextPalletId, con);
            SqlDataReader reader0 = cmd0.ExecuteReader();
            if (reader0.Read())
            {
                palletId = reader0["palID"].ToString();
                palletIdText.Content = palletId;
            }
            reader0.Close();
            
            String productsQuery = "SELECT bfProductCode FROM Products";
            SqlCommand cmd = new SqlCommand(productsQuery, con);
            SqlDataReader DR = cmd.ExecuteReader();
            while (DR.Read())
            {
                bfProductsCb.Items.Add(DR[0]);
            }
            DR.Close();


        }

        private void bfProductsCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            String selectedProduct = bfProductsCb.SelectedItem.ToString();
            String productDetails = "SELECT * FROM Products WHERE bfProductCode='" + selectedProduct + "'";
            // Console.Out.WriteLine("Selected product query: " + productDetails);
            using (SqlCommand cmd = new SqlCommand(productDetails, con))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    newPalletMinLen.Content = reader["minLength"];
                    newPalletMaxLen.Content = reader["maxLength"];
                    newPalletMinWidth.Content = reader["minWidth"];
                    newPalletMaxWidth.Content = reader["maxWidth"];
                    newPalletMinThick.Content = reader["minThickness"];
                    newPalletMaxThick.Content = reader["maxThickness"];
                    newPalletClass.Content = reader["class"];
                    newPalletSpecies.Content = reader["species"];

                }
                reader.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Pallet p1 = new Pallet("1700", "St", 160, 190, 1, 100, 30, 42, 35, 55, "A", "1700", "pallet1");
            if (palletIdText.Content != null)
            {

                Pallet p = new Pallet(palletIdText.Content.ToString(),
                                    newPalletSpecies.Content.ToString(),
                                    UInt32.Parse(newPalletMinLen.Content.ToString()),
                                    UInt32.Parse(newPalletMaxLen.Content.ToString()),
                                    UInt32.Parse(newPalletMinWidth.Content.ToString()),
                                    UInt32.Parse(newPalletMaxWidth.Content.ToString()),
                                    UInt32.Parse(newPalletMinThick.Content.ToString()),
                                    UInt32.Parse(newPalletMaxThick.Content.ToString()),
                                    UInt32.Parse(newPalletMinThick.Content.ToString()),
                                    UInt32.Parse(newPalletMaxThick.Content.ToString()),
                                    newPalletClass.Content.ToString(),
                                    bfProductsCb.SelectedItem.ToString(),
                                    formName
                                    );
                pm.addPallet(p);
                this.Close();
            }
        }
    }
}
