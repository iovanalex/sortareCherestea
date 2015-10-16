using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WpfApplication1
{
    class PalletManager
    {
        ArrayList pallets = new ArrayList();
        MainWindow mainWin;

        public PalletManager(MainWindow w)
        {
            mainWin = w;
            //              Pallet(palletId, species, minLen, maxLen, minWidth, maxWidth, minContractThickness, maxContractThickness, minFlagThickness, maxFlagThickness, bfClass, productName, fName)
            Pallet p1 = new Pallet("T1/2015", "St", 140, 180, 10, 70, 38, 55, 35, 55, "M","P4StABC39xLRx1500","pallet1");
            Pallet p2 = new Pallet("T2/2015", "St", 190, 220, 10, 70, 38, 55, 35, 55, "M","P4StABC39xLRx2000", "pallet2");
            Pallet p3 = new Pallet("T3/2015", "St", 230, 270, 10, 70, 38, 55, 35, 55, "M","P4StABC39xLRx2500", "pallet3");
            Pallet p4 = new Pallet("T4/2015", "St", 280, 300, 10, 70, 38, 55, 35, 55, "M","P4StABC39xLRx2900", "pallet4");
            Pallet p5 = new Pallet("T5/2015", "St", 310, 450, 10, 70, 38, 55, 35, 55, "M","P4StABC39xLRx3100", "pallet5");
            Pallet p6 = new Pallet("T1/2015", "St", 140, 180, 10, 70, 38, 55, 35, 55, "M", "P4StABC39xLRx1500", "pallet6");
            Pallet p7 = new Pallet("T2/2015", "St", 190, 220, 10, 70, 38, 55, 35, 55, "M", "P4StABC39xLRx2000", "pallet7");
            Pallet p8 = new Pallet("T3/2015", "St", 230, 270, 10, 70, 38, 55, 35, 55, "M", "P4StABC39xLRx2500", "pallet8");
            Pallet p9 = new Pallet("T4/2015", "St", 280, 300, 10, 70, 38, 55, 35, 55, "M", "P4StABC39xLRx2900", "pallet9");
            Pallet p10 = new Pallet("T5/2015", "St", 310, 450, 10, 70, 38, 55, 35, 55, "M", "P4StABC39xLRx3100", "pallet10");
            Pallet p11 = new Pallet("T4/2015", "St", 280, 300, 10, 70, 38, 55, 35, 55, "M", "P4StABC39xLRx2900", "pallet11");
            Pallet p12 = new Pallet("T5/2015", "St", 310, 450, 10, 70, 38, 55, 35, 55, "M", "P4StABC39xLRx3100", "pallet12");

 
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
           

            //pallets.Add(p6);
    
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
    }
}
