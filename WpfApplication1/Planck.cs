using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1
{
    class Planck
    {
        String planckGuid;

        DateTime passTime;

        public uint bfActualLength { get; private set; }
        uint bfLengthClass;
        public uint bfActualWidth { get; private set; }
        public uint bfActualThickness { get; private set; }
        uint bfThicknessClass;
        public String bfPlanckQalClass { get; private set; }

        public Planck(uint length, uint width, uint thickness, String qalClass)
        {
            planckGuid = Guid.NewGuid().ToString();
            passTime = DateTime.Now;
            bfActualLength = length;
            bfActualWidth = width;
            bfActualThickness = thickness;
            bfPlanckQalClass = qalClass;
            Console.Out.WriteLine("Create planck with " + planckGuid);
        }
        public uint getVolumeCCm()
        {
            return bfActualThickness * bfActualWidth * bfActualLength;
        }
    }
}
