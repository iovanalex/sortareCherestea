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
            //w Pallet(bfIdPallet, species, lungMin,lungMax,latMin, latMax, grosMin, grosMax, clasaCal)
            Pallet p1 = new Pallet("1/2015", "stejar", 200, 250, 20, 60, 20, 50, "A", "pallet1");
            Pallet p2 = new Pallet("2/2015", "stejar", 251, 310, 20, 60, 20, 50, "A", "pallet2");
            Pallet p3 = new Pallet("3/2015", "stejar", 311, 450, 20, 60, 20, 50, "A", "pallet3");
            Pallet p4 = new Pallet("4/2015", "stejar", 200, 250, 20, 60, 20, 50, "B", "pallet4");
            Pallet p5 = new Pallet("5/2015", "stejar", 251, 310, 20, 60, 20, 50, "B", "pallet5");
            Pallet p6 = new Pallet("6/2015", "stejar", 311, 450, 20, 60, 20, 50, "B", "pallet6");
            pallets.Add(p1);
            pallets.Add(p2);
            pallets.Add(p3);
            //pallets.Add(p4);
            //pallets.Add(p5);
            //pallets.Add(p6);
    
        }
        public Pallet addPlanck(Planck p)
        {
            foreach (Pallet pallet in pallets)
            {
                if (pallet.matchPlanck(p))
                {
                    pallet.addPlanck(p);
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
