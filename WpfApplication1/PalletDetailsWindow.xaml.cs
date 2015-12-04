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
    /// Interaction logic for PalletDetailsWindow.xaml
    /// </summary>
    public partial class PalletDetailsWindow : Window
    {
        Pallet p = null;
        PalletManager palletManager = null;
        public PalletDetailsWindow(Pallet p,PalletManager palletManager)
        {
            this.p = p;
            this.palletManager = palletManager;
            InitializeComponent();
            bfPalletId.Text = p.bfPalletId;
            bfPlancks.Text = p.getPlanckCount().ToString();
            bfPalletVolume.Text = p.getVolume().ToString();
            fillDataGrid(palletPlanksGrid, p.getGuid());
        }

        private void fillDataGrid(DataGrid dg, String palletGuid)
        {
            string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;            
            using (SqlConnection con = new SqlConnection(ConString))
            {
                String queryPlanks = "SELECT * FROM Plancks WHERE bfPalletID LIKE '" + palletGuid+"'";
                Console.Out.WriteLine("Searching for planks "+queryPlanks);
                SqlDataAdapter sda = new SqlDataAdapter(new SqlCommand(queryPlanks, con));
                DataTable dt = new DataTable("Raport");
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
            string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                String closePalletQuery = "UPDATE Pallets SET timeStop=GETDATE(), activePallet=0 WHERE bfPalletId='" + p.bfPalletId + "'";
                Console.Out.WriteLine("Closing pallet " + closePalletQuery);
                SqlCommand cmd = new SqlCommand(closePalletQuery, con);
                cmd.ExecuteNonQuery();            
              }
            palletManager.removePallet(p);
            this.Close();            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                String standbyPalletQuery = "UPDATE Pallets SET activePallet=0 WHERE bfPalletId='" + p.bfPalletId + "'";
                Console.Out.WriteLine("Going on standby with pallet " + standbyPalletQuery);
                Console.Out.WriteLine("Going on standby with pallet " + standbyPalletQuery);
                SqlCommand cmd = new SqlCommand(standbyPalletQuery, con);
                cmd.ExecuteNonQuery();
            }
            palletManager.removePallet(p);
            this.Close();            
        }
    }
}
