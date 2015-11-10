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
    /// Interaction logic for PalletDetailsWindow.xaml
    /// </summary>
    public partial class PalletDetailsWindow : Window
    {
        public PalletDetailsWindow(Pallet p)
        {
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
                Console.Out.WriteLine(queryPlanks);
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
    }
}
