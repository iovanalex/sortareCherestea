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

using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for ProductsManager.xaml
    /// </summary>
    public partial class ProductsManager : Window
    {
        public ProductsManager()
        {
            InitializeComponent();
            string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
            //string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                String CmdString = "SELECT Id, bfShortName, species, class, active FROM Products";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Products");
                sda.Fill(dt);
                dg.ItemsSource = dt.DefaultView;
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           System.Diagnostics.Process.Start("osk.exe");
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView row = (DataRowView)dg.SelectedItems[0];
            String selectedProductId = row["Id"].ToString();
            Console.Out.WriteLine("Just selected " + selectedProductId);

            string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
            String productDetails = "SELECT * FROM Products WHERE Id='" + selectedProductId + "'";
            Console.Out.WriteLine("Selected product query: " + productDetails);
            SqlConnection con = new SqlConnection(ConString);
            con.Open();
            using (SqlCommand cmd = new SqlCommand(productDetails, con))
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    bfProductName.Text = reader["bfShortName"].ToString();
                    productName.Text = reader["bfProductCode"].ToString();
                    minLen.Text = reader["minLength"].ToString();
                    maxLen.Text = reader["maxLength"].ToString();
                    minWidth.Text = reader["minWidth"].ToString();
                    maxWidth.Text = reader["maxWidth"].ToString();
                    minThick.Text = reader["minThickness"].ToString();
                    maxThick.Text = reader["maxThickness"].ToString();
                    species.Text = reader["species"].ToString();
                    //productClass.SelectedItem = reader["species"].ToString();
                    if (reader["active"].ToString() == "1")
                    {
                        isActive.IsChecked = true;
                    }
                    else {
                        isActive.IsChecked = false;
                    }

                }
                reader.Close();
            }            
        }
    }
}
