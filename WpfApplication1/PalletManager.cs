using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WpfApplication1
{
    public class PalletManager
    {
        ArrayList pallets = new ArrayList();
       
        Stack<Planck> planckStack = new Stack<Planck>(5);
        MainWindow mainWin;

        public PalletManager(MainWindow w)
        {
            mainWin = w;

            /*
            //              Pallet(palletId, species, minLen, maxLen, minWidth, maxWidth, minContractThickness, maxContractThickness, minFlagThickness, maxFlagThickness, bfClass, productName, fName)
            Pallet p1 = new Pallet("1700", "St", 160, 190, 1, 100, 30, 42, 35, 55, "A","1700","pallet1");
            Pallet p2 = new Pallet("2000", "St", 200, 240, 1, 100, 30, 42, 35, 55, "A", "2000", "pallet2");
            Pallet p3 = new Pallet("2500", "St", 250, 260, 1, 100, 30, 42, 35, 55, "A", "2500", "pallet3");
            Pallet p4 = new Pallet("2700", "St", 270, 290, 1, 100, 30, 42, 35, 55, "A", "2700", "pallet4");
            Pallet p5 = new Pallet("3000", "St", 300, 320, 1, 100, 30, 42, 35, 55, "A", "3000", "pallet5");
            Pallet p6 = new Pallet("3300", "St", 330, 360, 1, 100, 30, 42, 35, 55, "A", "3300", "pallet6");
            Pallet p7 = new Pallet("1700", "St", 160, 190, 1, 100, 30, 42, 35, 55, "B", "1700", "pallet7");
            Pallet p8 = new Pallet("2000", "St", 200, 240, 1, 100, 30, 42, 35, 55, "B", "2000", "pallet8");
            Pallet p9 = new Pallet("2500", "St", 250, 260, 1, 100, 30, 42, 35, 55, "B", "2500", "pallet9");
            Pallet p10 = new Pallet("2700", "St", 270, 290, 1, 100, 30, 42, 35, 55, "B", "2700", "pallet10");
            Pallet p11 = new Pallet("3000", "St", 300, 320, 1, 100, 30, 42, 35, 55, "B", "3000", "pallet11");
            Pallet p12 = new Pallet("3300", "St", 330, 360, 1, 100, 30, 42, 35, 55, "B", "3300", "pallet12");

 
            pallets.Add(p1);
            pallets.Add(p2);
            pallets.Add(p3);
            pallets.Add(p4);
            pallets.Add(p5);
            pallets.Add(p6);
            pallets.Add(p7);
            pallets.Add(p8);
            pallets.Add(p9);
            pallets.Add(p10);
            pallets.Add(p11);
            pallets.Add(p12);
           */
    
        }

        public void addPallet(Pallet p){
            pallets.Add(p);
            mainWin.updatePallets(p);
        }

        public Pallet addPlanck(Planck p)
        {
            foreach (Pallet pallet in pallets)
            {
                if (pallet.matchPlanck(p))
                {
                    pallet.addPlanck(p);
                    p.setPallet(pallet);
                    p.bfPalletGuid = pallet.getGuid();
                    //mainWin.updatePallets(pallet);
                    planckStack.Push(p);
                    return pallet;
                }
            }
            return null;
        }

        public ArrayList getPallets()
        {
            return pallets;
        }

        public Pallet getPalletByFromName(String panelName)
        {
            foreach (Pallet p in pallets)
            {
                if (p.getFormName() == panelName)
                {
                    return p;
                }
            }
            return null;
        }
        public Planck getLatest()
        {
            Planck latest = planckStack.Peek();
            bool gasit=false;
            foreach (Pallet pal in pallets)
            {
                if (pal.contains(latest))
                {
                    gasit = true;
                    break;
                }
            }
            if (gasit)
            {
                return planckStack.Pop();
            }
            else
            {
                return null;
            }
        }
    }
}
