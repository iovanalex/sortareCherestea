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
            bfPalletVolume.Text = p.getVolume().ToString();
            String query ;
            query = "SELECT * FROM Plancks WHERE bfPalletID="+p.getGuid();

            fillDataGrid(palletPlanksGrid, query);
        }

        private void fillDataGrid(DataGrid dg, String CmdString)
        {
            string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
            //string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                //CmdString = "SELECT * FROM Plancks";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("Raport");
                sda.Fill(dt);
                dg.ItemsSource = dt.DefaultView;
            }
        }
    }
}
