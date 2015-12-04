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
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void PLC_Disconnect_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Disconnect_Handler("192.168.0.10");
            Console.Out.WriteLine("Terminat disconnected");
            AppConfigStore.connectedPlc = false;
            AppConfigStore.running = false;
        }

        private void PLC_Connect_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Connect_Handler("192.168.0.10");
            Console.Out.WriteLine("Terminat connected");
            AppConfigStore.connectedPlc = true;
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Reset_Handler("192.168.0.10");
            Console.Out.WriteLine("Terminat connected");

        }

       
        private void manual_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Manual_Handler("192.168.0.10");
            Console.Out.WriteLine("Manual connected");
            AppConfigStore.autoManual = "Manual";
        }

        private void auto_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Auto_Handler("192.168.0.10");
            Console.Out.WriteLine("Auto sent");
            AppConfigStore.autoManual = "Auto";
        }

        private void start_handler(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Start_Handler("192.168.0.10");
            Console.Out.WriteLine("Start sent");
            AppConfigStore.running = true;
        }

        private void nextPlanck_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_NextPlanck_Handler("192.168.0.10");
            Console.Out.WriteLine("Next planck sent");
        }

        private void btInchideSettings_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_PreviewQueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ProductsManager().Show();
        }
    }
}
