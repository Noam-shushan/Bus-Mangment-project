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
    /// Interaction logic for BussListWin.xaml
    /// </summary>
    public partial class BussListWin : Window
    {
        public BussListWin(ObservableCollection<PO.Bus> busList)
        {
            InitializeComponent();
            lvBusList.ItemsSource = busList;
        }

        private void lvBusList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void lvBusList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bus = lvBusList.SelectedItem as PO.Bus;
            new BusViewInfo(bus).Show();
        }

        private void btnTreatment_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefueling_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
