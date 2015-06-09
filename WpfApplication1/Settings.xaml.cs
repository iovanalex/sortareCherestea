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
        }

        private void PLC_Connect_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Connect_Handler("192.168.0.10");
            Console.Out.WriteLine("Terminat connected");
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Reset_Handler("192.168.0.10");
            Console.Out.WriteLine("Terminat connected");

        }

        private void getLen_click(object sender, RoutedEventArgs e)
        {
             UInt16 len = PlcDb.Instance.readLungime("192.168.0.10");
                String leng = len.ToString();
                //lungimeTB.Text = leng;
                Console.Out.WriteLine("Lungime citita " + leng);
                //Thread.Sleep(100);
            
        }

        private void manual_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Manual_Handler("192.168.0.10");
            Console.Out.WriteLine("Manual connected");
        }

        private void auto_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Auto_Handler("192.168.0.10");
            Console.Out.WriteLine("Auto sent");
        }

        private void start_handler(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_Start_Handler("192.168.0.10");
            Console.Out.WriteLine("Start sent");
        }

        private void nextPlanck_click(object sender, RoutedEventArgs e)
        {
            PlcDb.Instance.PLC_NextPlanck_Handler("192.168.0.10");
            Console.Out.WriteLine("Next planck sent");

        }
    }
}
