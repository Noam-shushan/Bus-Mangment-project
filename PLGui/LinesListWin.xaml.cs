using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for LinesListWin.xaml
    /// </summary>
    public partial class LinesListWin : Window
    {
        public LinesListWin(ObservableCollection<PO.Line> lineList)
        {
            InitializeComponent();
            lvLineList.ItemsSource = lineList;
        }

        private void lvLineList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var line = lvLineList.SelectedItem as PO.Line;
            new LineViewInfo(line).Show();
        }
    }
}
