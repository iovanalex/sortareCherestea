using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Planck
    {
        public uint bfActualLength { get; private set; } //in cm
        uint bfLengthClass=0;
        public uint bfActualWidth { get; private set; } //in cm
        public uint bfActualThickness { get; private set; } //in mm with tenthn of a mm
        uint bfThicknessClass;
        public String bfPlanckQalClass { get; private set; }
        

        String planckGuid;
        public String selectedPalletGuid {get; set;}
        DateTime passTime;
        Pallet currentPallet;
        public String bfPalletId {set; get;}


        public Planck(uint length, uint width, uint thickness, String qalClass, String planckGuid, String bfPalletId)
        {
            planckGuid = Guid.NewGuid().ToString();
            passTime = DateTime.Now;
            this.bfActualLength = length;
            this.bfActualWidth = width;
            this.bfActualThickness = thickness;
            this.bfPlanckQalClass = qalClass;
            this.planckGuid = planckGuid;
            this.bfPalletId = bfPalletId;

            Console.Out.WriteLine("Created from DB planck with " + planckGuid);
        }

        public void setLengthClass(uint bfLengthClassMin)
        {
            this.bfLengthClass = bfLengthClassMin;
        } 

        public void setPallet(Pallet p)
        {
            currentPallet = p;
        }

        public Pallet getCurrentPallet()
        {
            return currentPallet;
        }

        public Planck(uint length, uint width, uint thickness, String qalClass)
        {
            planckGuid = Guid.NewGuid().ToString();
            passTime = DateTime.Now;
            this.bfActualLength = length;
            this.bfActualWidth = width;
            this.bfActualThickness = thickness;
            this.bfPlanckQalClass = qalClass;
           
            Console.Out.WriteLine("Created orginal planck with " + planckGuid);
        }

        public void setOnPallet(String bfPalletId){
            this.bfPalletId = bfPalletId;
        }
        public double getVolumeCCm()
        {
            if (this.bfPlanckQalClass == "A")
            {
                return (this.bfActualThickness / 100.0) * this.bfActualWidth * this.bfLengthClass;
            }
            else
            {
                return (this.bfActualThickness / 100.0) * this.bfActualWidth * this.bfActualLength;
            }
        }


        

        public void updateDatabase()
        {
            DbPlanksDataContext db = new DbPlanksDataContext();
            Plancks dbPlancks=new Plancks();
            //dbPlancks.Id = this.planckGuid;
            dbPlancks.bfActualLength=this.bfActualLength;
            dbPlancks.bfActualThickness=this.bfActualThickness;
            dbPlancks.bfActualWidth=this.bfActualWidth;
            dbPlancks.bfQalClass=this.bfPlanckQalClass;
            dbPlancks.bfPlanckId = this.planckGuid;
            dbPlancks.bfPalletId = this.bfPalletId;
            dbPlancks.bfProductId = this.currentPallet.getBfProductName();

            dbPlancks.timeStamp = DateTime.Now;

            Console.Out.WriteLine("Inserting " +this.planckGuid);

            try
            {
                db.Plancks.InsertOnSubmit(dbPlancks);
                db.SubmitChanges();
            }
            catch (Exception e)
            {
                 new measurementWindow(e.ToString()).Show();

            }
           

        }
    }
}
