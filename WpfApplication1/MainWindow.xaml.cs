using System;
using System.Collections;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
//using System.Windows.Media.Brush;

using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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
        bool manualDataInput = false;


        public MainWindow()
        {
            InitializeComponent();
            bw.WorkerReportsProgress = true;
            bw.DoWork += new DoWorkEventHandler(do_work);
            bw.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            bw.DoWork += new DoWorkEventHandler(do_work);
            palletManager = new PalletManager(this);
            focusedControl = (Control)FindName("latimeTB");

            //--------LOAD OPEN PALLETS----------------
            
            palletManager.clearEverithing();
            string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                con.Open();
                String getOpenPallets = @"SELECT pa.Id, pa.bfPalletId,pa.bfSpecies,pa.bfMinLen,
                                                 pa.bfMaxLen,bfPlanckMinWidth,bfPlanckMaxWidth,
	                                             pa.bfPlanckMinThickness,pa.bfPlanckMaxThickness,pr.class,pr.bfProductCode,pa.formName
	                                        FROM Pallets pa, Products pr 
	                                        WHERE  timeStop IS NULL AND pa.bfProductCode=pr.bfProductCode;";
                SqlCommand cmd = new SqlCommand(getOpenPallets, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                   // Console.Out.WriteLine("Found open package with id "+reader["bfPalletId"]+" on position "+reader["formName"]);
                    Pallet onePallet=new Pallet(reader["bfPalletId"].ToString(),
                                        reader["bfSpecies"].ToString(),
                                        UInt32.Parse(reader["bfMinLen"].ToString()),
                                        UInt32.Parse(reader["bfMaxLen"].ToString()),
                                        UInt32.Parse(reader["bfPlanckMinWidth"].ToString()),
                                        UInt32.Parse(reader["bfPlanckMaxWidth"].ToString()),
                                        UInt32.Parse(reader["bfPlanckMinThickness"].ToString()),
                                        UInt32.Parse(reader["bfPlanckMaxThickness"].ToString()),
                                        0,
                                        0,
                                        reader["class"].ToString(),
                                        reader["bfProductCode"].ToString(),
                                        reader["formName"].ToString(),
                                        true);
                    onePallet.setGuid(reader["Id"].ToString());
                    palletManager.addPallet(onePallet);

                    String getPalletPlanks = "SELECT * FROM Plancks WHERE bfPalletId='" + reader["Id"] + "'";
                    SqlCommand cmdListPlanks = new SqlCommand(getPalletPlanks, con);
                    SqlDataReader readerPlancks = cmdListPlanks.ExecuteReader();
                    while (readerPlancks.Read())
                    {
                      Planck onePlanck=new Planck(
                          UInt32.Parse(readerPlancks["bfActualLength"].ToString()),
                          UInt32.Parse(readerPlancks["bfActualLength"].ToString()),
                          UInt32.Parse(readerPlancks["bfActualThickness"].ToString()),
                          readerPlancks["bfQalClass"].ToString(),
                          readerPlancks["bfPlanckId"].ToString(),
                          onePallet.getGuid().ToString()
                          );
                      onePallet.addPlanck(onePlanck);
                      Console.Out.WriteLine("Found one planck"+onePlanck.bfPalletId);
                    }
                    updatePallets(onePallet);
                }
            }
             
            //-----------------------------------------

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
            if (manualDataInput == false)
            {
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
                    p.setOnPallet(selectedPallet.getGuid());
                    updatePallets(selectedPallet);
                    p.updateDatabase();
                    lungimeTB.Text = "";
                    latimeTB.Text = "";
                    grosimeTB.Text = "";
                    planckClass.Text = "";
                    
                   // PlcDb.Instance.PLC_NextPlanck_Handler("192.168.0.10");

                    String coadaScanduri = selectedPallet.bfPalletId + "," + selectedPallet.getBfProductName() + "," + p.bfActualLength + "," + p.bfPlanckQalClass;
                    planckQueue.Items.Add(coadaScanduri);
                    manualDataInput = false;
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
            planckClass.Background = btClassA.Background;
        }

      

        public void updatePallets(Pallet p)
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
            if ((p!=null)&&(p.getPlanckCount()!=0))
            {
                new PalletDetailsWindow(p).Show();
            }
            else
            {
                new NewPalletWindow(formName, palletManager).Show();
            }
        }

        private void resetPallet(String formName)
        {
            Pallet p = palletManager.getPalletByFromName(formName);
            p.reset();
            updatePallets(p);
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
            if (manualDataInput == false)
            {
                manualDataInput = true;
                manualLengthBtn.BorderThickness = new System.Windows.Thickness(4);
                lungimeTB.IsEnabled = true;
                grosimeTB.IsEnabled = true;
            }
            else
            {
                manualDataInput = false;
                manualLengthBtn.BorderThickness = new System.Windows.Thickness(0);
                lungimeTB.IsEnabled = false;
                grosimeTB.IsEnabled = false;
            }
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
            
            Console.Out.WriteLine("Am initializat sistemul");
            // css.Close();
            ArrayList pallets = palletManager.getPallets();
            foreach (Pallet p in pallets)
            {
                updatePallets(p);
            }
            bw.RunWorkerAsync();

        }

        private void latimeTB_GotFocus(object sender, RoutedEventArgs e)
        {
            focusedControl = (Control)latimeTB;
        }
        private void lungimeTB_GotTouchCapture(object sender, TouchEventArgs e)
        {
            focusedControl = (Control)lungimeTB;
        }
    

        private void btClassB_Click(object sender, RoutedEventArgs e)
        {
            planckClass.Text = "B";
            planckClass.Background = btClassB.Background;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btRapoarte_Click(object sender, RoutedEventArgs e)
        {
            new Rapoarte().ShowDialog();
        }

        private void bfClassM_Click(object sender, RoutedEventArgs e)
        {
            planckClass.Text = "M";
            planckClass.Background = bfClassM.Background;
        }
        //----------------------------------------------------------------------------
        private void pallet1Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet1");
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

        private void pallet7Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet7");
        }

        private void pallet8Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet8");
        }

        private void pallet9Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet9");
        }

        private void pallet10Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet10");
        }

        private void pallet11Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet11");
        }

        private void pallet12Details_Click(object sender, RoutedEventArgs e)
        {
            showPalletDetails("pallet12");
        }
        //----------------------------------------------------------------------------
        private void lungimeTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            focusedControl = (Control)lungimeTB;
        }

        private void grosimeTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            focusedControl = (Control)grosimeTB;
        }

        private void latimeTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            focusedControl = (Control)latimeTB;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Planck latest = palletManager.getLatest();
            manualDataInput = true;
           // lungimeTB.Text = UInt32.Parse(latest.bfActualLength);
           // latimeTB=latest.bfActualWidth
        }


       



    }
}
