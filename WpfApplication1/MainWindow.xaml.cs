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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.ComponentModel;
using System.Collections;
//using System.Windows.Media.Brush;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker bw = new BackgroundWorker();
        PalletManager palletManager;
        public MainWindow()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(do_work);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.DoWork += new DoWorkEventHandler(do_work);
            palletManager = new PalletManager(this);
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
           for (int i = 1; i <= 1000; i++) { 
                UInt16 len = PlcDb.Instance.readLungime("192.168.0.10");
                String leng=len.ToString();
                lungimeTB.Text = leng;
                Console.Out.WriteLine("Lungime citita " + leng);
                Thread.Sleep(100);
            }
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

        private void do_work(object sender, DoWorkEventArgs e)
        {
            Random rng = new Random();

            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                worker.ReportProgress(rng.Next(160, 350));
                System.Threading.Thread.Sleep(500);
            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.lungimeTB.Text = (e.ProgressPercentage.ToString() );
        }
        private void sysBackStart_Click(object sender, RoutedEventArgs e)
        {
           
            bw.RunWorkerAsync();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
            ArrayList pallets = palletManager.getPallets();
            foreach (Pallet p in pallets)
            {
                updatePallets(p);
            }
        }

        private void btSavePlanck_Click(object sender, RoutedEventArgs e)
        {
            
           // if (lungimeTB.Text != "")
           // {
                UInt16 measuredLength = UInt16.Parse(lungimeTB.Text);
           // }
           // if (latimeTB.Text != "")
           // {
                UInt16 measuredWidth = UInt16.Parse(latimeTB.Text);
           // }
           // if (latimeTB.Text != "")
           // {
                UInt16 measuredThickness = UInt16.Parse(grosimeTB.Text);
           // }
            

            Planck p = new Planck(measuredLength, measuredWidth, measuredThickness, "A");
            Pallet selectedPallet=palletManager.addPlanck(p);
            if (selectedPallet != null)
            {
                updatePallets(selectedPallet);
            }

            lungimeTB.Text = "";
            latimeTB.Text = "";
            grosimeTB.Text = "";

        }

        private void btClassA_Click(object sender, RoutedEventArgs e)
        {
           // btClassA.Background = new System.Windows.Media.SolidColorBrush(System.Windows.SystemColors);
        }

        private void updatePallets(Pallet p)
        {
            String formPalletName = p.getFormName();
            Label lbPcs = (Label)FindName(formPalletName + "Pcs");
            lbPcs.Content = p.getPlanckCount();

            Label lbName = (Label)FindName(formPalletName + "bfId");
            lbName.Content = p.bfPalletId;

            Label lbVolume = (Label)FindName(formPalletName + "Volume");
            lbVolume.Content = p.getVolume();
        }



    }
}
