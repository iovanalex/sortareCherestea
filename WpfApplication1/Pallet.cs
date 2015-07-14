using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

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
        uint bfPlanckMinThickness;
        uint bfPlanckMaxThickness;
        String bfClass;

        public Pallet(String palletId, String species, uint minLen, uint maxLen, uint minWidth, uint maxWidth, uint minThickness, uint maxThickness, String bfClass, String fName )
        {
            palletGuid = Guid.NewGuid().ToString();
            bfPalletId = palletId;
            bfSpecies = species;
            bfMinLen = minLen;
            bfMaxLen = maxLen;
            bfPlanckMinWidth = minWidth;
            bfPlanckMaxWidth = maxWidth;
            bfPlanckMinThickness = minThickness;
            bfPlanckMaxThickness = maxThickness;
            this.bfClass = bfClass;
            formName = fName;
        }

        public bool matchPlanck(Planck p){
            if  (
                ((bfMinLen <= p.bfActualLength) && (p.bfActualLength <= bfMaxLen)) &&
                ((bfPlanckMinWidth <= p.bfActualWidth) && (p.bfActualWidth <= bfPlanckMaxWidth)) &&
                ((bfPlanckMinThickness <= p.bfActualThickness) && (p.bfActualThickness <= bfPlanckMaxThickness)) &&
                (bfClass==p.bfPlanckQalClass)
                )return true;

            return false;
        }

        public void addPlanck(Planck p){
            if (plancks.Count==0)
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
            double vol=0;
            foreach (Planck p in plancks)
            {
                vol += p.getVolumeCCm();
            }
            return vol/1000000.0;
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
    }
}
