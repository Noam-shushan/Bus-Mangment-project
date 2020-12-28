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
    /// Interaction logic for Management.xaml
    /// </summary>
    public partial class Management : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<PO.Bus> myBusList { get; } = new ObservableCollection<PO.Bus>();

        public Management()
        {
            InitializeComponent();
            
            foreach(var bus in myBL.GetAllBuss())
            {
                myBusList.Add(Tools.CopyBusBOToPO(bus));
            }
            cbBuss.ItemsSource = myBusList;
            cbBuss.DisplayMemberPath = "LicenseNum";

            cbLinse.ItemsSource = myBL.GetAllLines();
            cbLinse.DisplayMemberPath = "Id";

            cbStations.ItemsSource = myBL.GetAllStations();
            cbStations.DisplayMemberPath = "Code";
        }

        private void btnBuss_Click(object sender, RoutedEventArgs e)
        {
            new BussListWin().Show();
        }

        private void btnLines_Click(object sender, RoutedEventArgs e)
        {
            new LinesListWin().Show();
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            new StationListWin().Show();
        }

        private void cbBuss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var bus = cbBuss.SelectedItem as PO.Bus;
            new BusViewInfo(bus).Show();
        }

        private void cbLinse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
