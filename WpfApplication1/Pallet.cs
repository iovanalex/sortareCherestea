using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

using System.Windows;

namespace WpfApplication1
{
    public class Pallet
    {
        String palletGuid;
        ArrayList plancks = new ArrayList();
        DateTime timeStart;
        DateTime timeStop;
        String formName;

        public String bfPalletId { get; private set; }
        public String bfSpecies { get; private set; }
        public uint bfMinLen;
        uint bfMaxLen;
        uint bfPlanckMinWidth;
        uint bfPlanckMaxWidth;

        uint bfPlanckMinContractThickness;
        uint bfPlanckMaxContractThickness;
        uint bfPlanckMinFlagThickness;
        uint bfPlanckMaxFlagThickness;

        String bfClass;
        String bfProductName;


        public uint getMinContractThickness()
        {
            return bfPlanckMinContractThickness;
        }

        public bool containsPlanck(Planck p)
        {
            return plancks.Contains(p);
        }

        public void removePlanck(Planck p)
        {
            plancks.Remove(p);
        }
        public uint getMinFlag()
        {
            return bfPlanckMinFlagThickness;
        }

        public uint getMaxFlag()
        {
            return bfPlanckMaxFlagThickness;
        }

        public String getGuid()
        {
            return palletGuid;
        }

        public void setGuid(String gUid)
        {
            this.palletGuid = gUid;
        }

        public String getProductName()
        {
            return bfProductName;
        }

        public Pallet(String palletId,  String species, uint minLen, uint maxLen, uint minWidth, uint maxWidth, uint minContractThickness, uint maxContractThickness, uint minFlagThickness, uint maxFlagThickness, String bfClass, String productName, String fName, bool recoverFromDatabase)
        {
            if (!recoverFromDatabase)
            {
                palletGuid = Guid.NewGuid().ToString();
            }
           
            
            bfPalletId = palletId;
            bfSpecies = species;
            bfMinLen = minLen;
            bfMaxLen = maxLen;
            bfPlanckMinWidth = minWidth;
            bfPlanckMaxWidth = maxWidth;
            bfPlanckMinContractThickness = minContractThickness;
            bfPlanckMaxContractThickness = maxContractThickness;
            bfPlanckMaxFlagThickness = maxFlagThickness;
            bfPlanckMinFlagThickness = minFlagThickness;
            this.bfClass = bfClass;
            formName = fName;
            bfProductName = productName;

            //------------------STORE PALLET DATA TO DB--------------------------
            //INSERT INTO Pallets('bfPalletId', 'bfSpecies', bfMinLen','bfMaxLen', 'bfPlankMinWidth', 'bfPlankMaxWidth', 'bfPlankMinThickness', 'bfPlankMaxThickness', 'formName', 'timeStart' VALUES ,fag,201,240,8,60,38,38,pallet19/11/2015 12:37:24 AM
            if (!recoverFromDatabase)
            {
                String InsertPalletQueryString = @"set dateformat mdy; INSERT INTO Pallets (
                    Id,
					bfPalletId, 
					bfSpecies, 
					bfMinLen,
					bfMaxLen, 
					bfPlanckMinWidth, 
					bfPlanckMaxWidth, 
					bfPlanckMinThickness, 
					bfPlanckMaxThickness, 
					formName, 
					timeStart,
                    bfProductCode) 
					VALUES ('" + this.palletGuid +
                                  "','" + this.bfPalletId +
                                  "','" + this.bfSpecies +
                                  "','" + this.bfMinLen +
                                  "','" + this.bfMaxLen +
                                  "','" + this.bfPlanckMinWidth +
                                  "','" + this.bfPlanckMaxWidth +
                                  "','" + this.bfPlanckMinContractThickness +
                                  "','" + this.bfPlanckMaxContractThickness +
                                  "','" + this.formName +
                                  "','" + DateTime.Now +
                                  "','" + this.bfProductName + "')";
                string ConString = ConfigurationManager.ConnectionStrings["WpfApplication1.Properties.Settings.SortareCheresteaConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(ConString))
                {
                    con.Open();
                    SqlCommand cmd;
                    // Console.Out.WriteLine("Add new pallet query: " + InsertPalletQueryString);
                    try
                    {
                        cmd = new SqlCommand(InsertPalletQueryString, con);
                        cmd.ExecuteNonQuery();
                        // Console.Out.WriteLine("INFO: Inserted Pallet: " + this.palletGuid);
                    }
                    catch (Exception ex)
                    {
                        new measurementWindow("first block" + InsertPalletQueryString+"\n\n"+ex.StackTrace).Show();
                        System.IO.StreamWriter file = new System.IO.StreamWriter("d:\\error.txt");
                        file.WriteLine(InsertPalletQueryString);
                        file.WriteLine(ex.StackTrace);
                        file.Close();
                    }

                    String updateLastPalletIdCommand = @"UPDATE Variabile 
                                                        SET value='" + this.bfPalletId +
                                                         "' WHERE varName='lastPalletId'";
                    try
                    {
                        cmd = new SqlCommand(updateLastPalletIdCommand, con);
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        new measurementWindow("second block" + InsertPalletQueryString).Show();
                        

                    }
                    con.Close();
                }
            }
            
        }

        public bool matchPlanck(Planck p)
        {
          //  Console.Out.WriteLine(@"Checking planck " + p.bfActualLength + " against pallet " + this.bfPalletId+
          //      " with length "+this.bfMinLen+" to "+this.bfMaxLen+
          //      " and width "+this.bfPlanckMinWidth+" to "+this.bfPlanckMaxWidth+" and thickness "+this.bfPlanckMinFlagThickness+" to "+this.bfPlanckMaxFlagThickness);
            if (
                ((bfMinLen <= p.bfActualLength) && (p.bfActualLength <= bfMaxLen)) &&
                ((bfPlanckMinWidth <= p.bfActualWidth) && (p.bfActualWidth <= bfPlanckMaxWidth)) &&
                ((bfPlanckMinFlagThickness <= (int)(p.bfActualThickness/10)) && ((int)(p.bfActualThickness/10) <= bfPlanckMaxFlagThickness)) &&
                (bfClass == p.bfPlanckQalClass)
                ) 
            {
                return true;
            }
            else
            {
                if 
                    (
                    ((bfMinLen <= p.bfActualLength) && (p.bfActualLength <= bfMaxLen)) &&
                    ((bfPlanckMinWidth <= p.bfActualWidth) && (p.bfActualWidth <= bfPlanckMaxWidth))&&
                    (bfClass == p.bfPlanckQalClass)
                    )
                {
                    String message = "Grosimea actuala este " + ((double)p.bfActualThickness)/10 + " iar cea acceptata este intre " + bfPlanckMinFlagThickness + " si " + bfPlanckMaxFlagThickness + ".\nDoriti sa acceptati?";
                    MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(message, "Grosime eronata", System.Windows.MessageBoxButton.YesNo);
                    if (messageBoxResult == MessageBoxResult.Yes)
                    {
                        return true;
                    }
                    else{
                        return false;
                    }
                }
            }
            return false;
        }

        public void addPlanck(Planck p)
        {
            if (plancks.Count == 0)
            {
                timeStart = DateTime.Now;
            }
            plancks.Add(p);
           // Console.Out.WriteLine("Added one plank to pallet " + palletGuid);
        }

        public int getPlanckCount()
        {
            return plancks.Count;
        }

        public double getVolume()
        {
            double vol = 0;
            foreach (Planck p in plancks)
            {
                vol += p.getVolumeCCm(this);
            }
            return vol / 10000000.0;
        }

        public String getFormName()
        {
            return formName;
        }

        public ArrayList getPlanks()
        {
            return plancks;
        }
        public void updateDatabase()
        {
            // Sortare
        }
        public String getBfProductName()
        {
            return bfProductName;
        }

        public void reset()
        {
            plancks.RemoveRange(0, plancks.Count);
            palletGuid = Guid.NewGuid().ToString();
        }

        public bool contains(Planck p)
        {
            return plancks.Contains(p);
        }


    }
}
