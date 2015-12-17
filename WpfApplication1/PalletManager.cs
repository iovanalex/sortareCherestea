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
       
      //  Stack<Planck> planckStack = new Stack<Planck>(5);
        Queue<Planck> planckQueue = new Queue<Planck>(5);
        MainWindow mainWin;

        public PalletManager(MainWindow w)
        {
            mainWin = w;         
        }

        public void addPallet(Pallet p){
            pallets.Add(p);

            mainWin.updatePallets(p);
        }

        public void removePlanck(Planck planck)
        {
            foreach (Pallet pallet in pallets)
            {
                if (pallet.containsPlanck(planck))
                {
                    pallet.removePlanck(planck);
                    mainWin.updatePallets(pallet);
                }
            }
        }

        public Pallet addPlanck(Planck p)
        {
            foreach (Pallet pallet in pallets)
            {
                if (pallet.matchPlanck(p))
                {
                    pallet.addPlanck(p);
                    p.setPallet(pallet);
                    p.selectedPalletGuid = pallet.getGuid();
                    p.bfPalletId = pallet.getGuid();
                    mainWin.updatePallets(pallet);

                    if (planckQueue.Count >= 5)
                    {
                        planckQueue.Dequeue();
                    }

                    //planckStack.Push(p);
                    planckQueue.Enqueue(p);
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
            return planckQueue.Last();
        }

        public void removePallet(Pallet p)
        {
            String idToRemove = p.bfPalletId;
            pallets.Remove(p);
            Console.Out.WriteLine("Just removed pallet with bfId=" + idToRemove);
            mainWin.removeUIPallet(p);
            foreach (Pallet pal in pallets)
            {
                Console.Out.WriteLine("oen pal"+pal.bfPalletId);
            }
        }

        public void clearEverithing()
        {
            pallets.Clear();
            planckQueue.Clear();
        }
    }
}
