using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    public class Planck
    {
        String planckGuid;

        DateTime passTime;

        public uint bfActualLength { get; private set; } //in cm
        uint bfLengthClass;
        public uint bfActualWidth { get; private set; } //in cm
        public uint bfActualThickness { get; private set; } //in mm
        uint bfThicknessClass;
        public String bfPlanckQalClass { get; private set; }

        public Planck(uint length, uint width, uint thickness, String qalClass)
        {
            planckGuid = Guid.NewGuid().ToString();
            passTime = DateTime.Now;
            this.bfActualLength = length;
            this.bfActualWidth = width;
            this.bfActualThickness = thickness;
            this.bfPlanckQalClass = qalClass;
            Console.Out.WriteLine("Created planck with " + planckGuid);
        }
        public double getVolumeCCm()
        {
            return (this.bfActualThickness/10.0) * this.bfActualWidth * this.bfActualLength;
        }

        public void updateDatabase()
        {
            DbPlanksDataContext db = new DbPlanksDataContext();
            Plancks dbPlancks=new Plancks();
            //dbPlancks.Id = this.planckGuid;
            dbPlancks.bfActualLength=this.bfActualLength;
            dbPlancks.bfActualThickness=this.bfActualThickness;
            dbPlancks.bfActualWidth=this.bfActualThickness;
            dbPlancks.bfQalClass=this.bfPlanckQalClass;
            dbPlancks.bfPlanckId = this.planckGuid;

            dbPlancks.timeStamp = DateTime.Now;

            Console.Out.WriteLine("Inserting " +this.planckGuid);

            db.Plancks.InsertOnSubmit(dbPlancks);
            db.SubmitChanges();
           

        }
    }
}
