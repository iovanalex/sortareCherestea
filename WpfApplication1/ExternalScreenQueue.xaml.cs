using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for ExternalScreenQueue.xaml
    /// </summary>
    public partial class ExternalScreenQueue : Window
    {
        public ExternalScreenQueue()
        {
            InitializeComponent();
        }

        public void removeLast()
        {
            externalScreenQueue.Items.RemoveAt(externalScreenQueue.Items.Count - 1);
        }

        public void addTop(String textScandura)
        {
            externalScreenQueue.Items.Insert(0, textScandura);
        }

        public void removeFirst()
        {
            externalScreenQueue.Items.RemoveAt(0);
        }
    }
}
