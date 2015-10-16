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
    /// Interaction logic for Window1.xaml
    /// </summary>

    public partial class measurementWindow : Window
    {
        Window parentWin;
        TextBox parentFieldWarn;
        public measurementWindow(String text, TextBox fieldWarning, Window w)
        {
            InitializeComponent();
            warnText.Content = text;
            parentFieldWarn = fieldWarning;
            parentWin = w;
        }

        public measurementWindow(String text)
        {
            InitializeComponent();
            warnText.Content = text;
            parentFieldWarn = null;
            parentWin = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
