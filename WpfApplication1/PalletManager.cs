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

        public PalletManager()
        {
            Pallet p1 = new Pallet("1/2015", "stejar", 100, 140, 20, 40, 20, 40, "pallet1");
            Pallet p2 = new Pallet("2/2015", "stejar", 200, 270, 20, 40, 20, 40, "pallet2");
            Pallet p3 = new Pallet("3/2015", "stejar", 300, 350, 20, 40, 20, 40, "pallet3");
            Pallet p4 = new Pallet("4/2015", "stejar", 300, 350, 20, 40, 20, 40, "pallet4");
            Pallet p5 = new Pallet("5/2015", "stejar", 300, 350, 20, 40, 20, 40, "pallet5");
            Pallet p6 = new Pallet("6/2015", "stejar", 300, 350, 20, 40, 20, 40, "pallet6");
            pallets.Add(p1);
            pallets.Add(p2);
            pallets.Add(p3);
            pallets.Add(p4);
            pallets.Add(p5);
            pallets.Add(p6);
    
        }
        public Pallet addPlanck(Planck p)
        {
            foreach (Pallet pallet in pallets)
            {
                if (pallet.matchPlanck(p))
                {
                    pallet.addPlanck(p);
                    return pallet;
                }
            }
            return null;
        }

        public ArrayList getPallets()
        {
            return pallets;
        }
    }
}
