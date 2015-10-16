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
        Control focusedControl;
       


        public MainWindow()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(do_work);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.DoWork += new DoWorkEventHandler(do_work);
            palletManager = new PalletManager(this);
            focusedControl = (Control)FindName("latimeTB");

        }
        
      

        private void do_work(object sender, DoWorkEventArgs e)
        {
            Random rng = new Random();
           // UInt16 len = PlcDb.Instance.readLungime("192.168.0.10");

            BackgroundWorker worker = sender as BackgroundWorker;
            while (true)
            {
                worker.ReportProgress(rng.Next(160, 350));
                
                System.Threading.Thread.Sleep(500);

            }
        }

        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (AppConfigStore.autoManual == "Auto")
            {
                statusAutoManual.Background = Brushes.Green;
            }
            if (AppConfigStore.autoManual == "Manual")
            {
                statusAutoManual.Background = Brushes.Orange;
            }

            if (AppConfigStore.connectedPlc == true)
            {
               statusConnected.Background = Brushes.Green;
            }
            else
            {
                statusConnected.Background = Brushes.Red;
            }

            if (AppConfigStore.running == true)
            {
               statusOk.Background = Brushes.Green;
               btPornire.IsEnabled = false;
            }
            else
            {
                statusOk.Background = Brushes.Red;
                btPornire.IsEnabled = true;
            }

         




            //this.lungimeTB.Text = (e.ProgressPercentage.ToString() );
            UInt16 len = PlcDb.Instance.readLungime("192.168.0.10");
            UInt16 thick = PlcDb.Instance.readGrosime("192.168.0.10");
            if (len > 0)
            {
                this.lungimeTB.Text = Convert.ToString(len);
            }
            else
            {
                this.lungimeTB.Text = "N/A";
            }
            if (thick > 0)
            {
                this.grosimeTB.Text = Convert.ToString(thick);
            }
            else
            {
                this.grosimeTB.Text = "N/A";
            }
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
            UInt16 measuredLength=0, measuredWidth=0, measuredThickness=0;
            String warnMsg = "";
            String qalClass="";
            
            if ((lungimeTB.Text != "")&&(lungimeTB.Text!="N/A"))
            {
                measuredLength = UInt16.Parse(lungimeTB.Text);
            }
            else {
                warnMsg += "Atentie la lungime\n";
            }

            if ((latimeTB.Text != "")&&(latimeTB.Text!="N/A"))
            {
                measuredWidth = UInt16.Parse(latimeTB.Text);
            }
            else{
                warnMsg += "Atentie la latime\n";
            }

            if ((grosimeTB.Text != "")&&(grosimeTB.Text!="N/A"))
            {
                measuredThickness = UInt16.Parse(grosimeTB.Text);
            }
            else{
                warnMsg += "Atentie la grosime\n";
            }
            if (planckClass.Text!="")
            {
               qalClass=planckClass.Text;
            }
            else
            {
                warnMsg += "NU ati selectat clasa de calitate\n";
            }

            if (warnMsg != "")
            {
                new measurementWindow(warnMsg, lungimeTB, this).Show();
            }
            else
            {
                Planck p = new Planck(measuredLength, measuredWidth, measuredThickness, qalClass);
                Pallet selectedPallet = palletManager.addPlanck(p);
              
                if (selectedPallet != null)
                {
                    updatePallets(selectedPallet);
                    p.updateDatabase();
                    lungimeTB.Text = "";
                    latimeTB.Text = "";
                    grosimeTB.Text = "";
                    planckClass.Text = "";
                    PlcDb.Instance.PLC_NextPlanck_Handler("192.168.0.10");
                }
                else
                {
                    new measurementWindow("Scandura nu a fost potrivita la nici un palet\nVerificati dimensiunile", lungimeTB, this).Show();
                }
            }
        }

        private void btClassA_Click(object sender, RoutedEventArgs e)
        {
            planckClass.Text = "A";
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

            Label lbBfProductName = (Label)FindName(formPalletName + "bfProductName");
            lbBfProductName.Content = p.getBfProductName();
        }

        private void showPalletDetails(String formName){
            Pallet p = palletManager.getPalletByFromName(formName);
            new PalletDetailsWindow(p).Show();
        }

        private void kpd1_Copy_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "2";
            }
        }

        private void kpd1_Click(object sender, RoutedEventArgs e)
        {

            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "1";
            }
        }

        private void kpd1_Copy8_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            Console.Out.WriteLine("Current component is " + component.Name);
            if (component.Text.Length <= 1) component.Text = "";
            else
            {
                UInt16 width = UInt16.Parse(component.Text);
                width /= 10;
                component.Text = Convert.ToString(width);
            }
        }

        private void kpd1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "3";
            }

        }

        private void kpd1_Copy2_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "4";
            }
        }

        private void kpd1_Copy3_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "5";
            }
        }

        private void kpd1_Copy4_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "6";
            }
        }

        private void kpd1_Copy5_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "7";
            }
        }

        private void kpd1_Copy6_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "8";
            }
        }

        private void kpd1_Copy7_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "9";
            }
        }

        private void kpd1_Copy9_Click(object sender, RoutedEventArgs e)
        {
            TextBox component = (TextBox)focusedControl;
            if (component.Text.Length <= 2)
            {
                component.Text += "0";
            }
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWin = new Settings();
            settingsWin.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            lungimeTB.Focus();
            focusedControl = (Control)lungimeTB;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            grosimeTB.Focus();
            focusedControl = (Control)grosimeTB;
        }

        private void latimeTB_GotFocus(object sender, RoutedEventArgs e)
        {
            focusedControl = (Control)latimeTB;
        }

        private void pallet1Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet1");
        }

        private void lungimeTB_GotTouchCapture(object sender, TouchEventArgs e)
        {
            focusedControl = (Control)lungimeTB;
        }

        private void grosimeTB_GotTouchCapture(object sender, TouchEventArgs e)
        {
            focusedControl = (Control)latimeTB;
        }
        private void btClassB_Click(object sender, RoutedEventArgs e)
        {
            planckClass.Text = "B";

        }

        private void pallet2Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet2");
        }

        private void pallet3Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet3");
        }

        private void pallet4Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet4");
        }

        private void pallet5Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet5");
        }

        private void pallet6Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet6");
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btRapoarte_Click(object sender, RoutedEventArgs e)
        {
            new Rapoarte().ShowDialog();
        }

        private void btPornire_Click(object sender, RoutedEventArgs e)
        {
          //  ConnectionSplashScreen css = new ConnectionSplashScreen();
          //  css.ShowDialog();
            System.Threading.Thread.Sleep(500);
            PlcDb.Instance.PLC_Connect_Handler("192.168.0.10");
            System.Threading.Thread.Sleep(500);
            PlcDb.Instance.PLC_Reset_Handler("192.168.0.10");
            System.Threading.Thread.Sleep(500);
           // PlcDb.Instance.PLC_Manual_Handler("192.168.0.10");
           // System.Threading.Thread.Sleep(500);
            PlcDb.Instance.PLC_Auto_Handler("192.168.0.10");
            System.Threading.Thread.Sleep(500);
            PlcDb.Instance.PLC_Start_Handler("192.168.0.10");
            System.Threading.Thread.Sleep(500);
            bw.RunWorkerAsync();
            Console.Out.WriteLine("Am initializat sistemul");
           // css.Close();
        }

        private void bfClassM_Click(object sender, RoutedEventArgs e)
        {
            planckClass.Text = "M";
        }



    }
}
