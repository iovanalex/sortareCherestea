using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

using System.Linq;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

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
        uint bfMinLen;
        uint bfMaxLen;
        uint bfPlanckMinWidth;
        uint bfPlanckMaxWidth;
        uint bfPlanckMinContractThickness;
        uint bfPlanckMaxContractThickness;
        uint bfPlanckMinFlagThickness;
        uint bfPlanckMaxFlagThickness;
        String bfClass;
        String bfProductName;

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
                String InsertPalletQueryString = @"set dateformat dmy; INSERT INTO Pallets (
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
                    // Console.Out.WriteLine("Add new pallet query: " + InsertPalletQueryString);
                    SqlCommand cmd = new SqlCommand(InsertPalletQueryString, con);
                    cmd.ExecuteNonQuery();
                    Console.Out.WriteLine("INFO: Inserted Pallet: " + this.palletGuid);

                    String updateLastPalletIdCommand = @"UPDATE Variabile 
                                                        SET value='" + this.bfPalletId +
                                                         "' WHERE varName='lastPalletId'";
                    cmd = new SqlCommand(updateLastPalletIdCommand, con);
                    cmd.ExecuteNonQuery();
                }
            }
            
        }

        public bool matchPlanck(Planck p)
        {
            if (
                ((bfMinLen <= p.bfActualLength) && (p.bfActualLength <= bfMaxLen)) &&
                ((bfPlanckMinWidth <= p.bfActualWidth) && (p.bfActualWidth <= bfPlanckMaxWidth)) &&
                ((bfPlanckMinContractThickness <= p.bfActualThickness) && (p.bfActualThickness <= bfPlanckMaxContractThickness)) &&
                (bfClass == p.bfPlanckQalClass)
                ) return true;

            return false;
        }

        public void addPlanck(Planck p)
        {
            if (plancks.Count == 0)
            {
                timeStart = DateTime.Now;
            }
            plancks.Add(p);
            Console.Out.WriteLine("Added one plank to pallet " + palletGuid);
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
                vol += p.getVolumeCCm();
            }
            return vol / 1000000.0;
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
