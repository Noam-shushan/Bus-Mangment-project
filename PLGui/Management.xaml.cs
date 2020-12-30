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
using BL;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for Management.xaml
    /// </summary>
    public partial class Management : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<PO.Bus> myBusList = new ObservableCollection<PO.Bus>();
        ObservableCollection<PO.Line> myLineList = new ObservableCollection<PO.Line>();
        ObservableCollection<PO.Station> myStationList = new ObservableCollection<PO.Station>();

        public Management()
        {
            InitializeComponent();
            
            foreach(var bus in myBL.GetAllBuss())
            {
                var busPo = bus.CopyPropertiesToNew(typeof(PO.Bus)) as PO.Bus;
                busPo.LicenseNumber = bus.FormatLiscenseNumber();
                myBusList.Add(busPo);
            }
            cbBuss.ItemsSource = myBusList;
            cbBuss.DisplayMemberPath = "LicenseNumber";

            foreach(var line in myBL.GetAllLines())
            {
                var linePo = line.CopyPropertiesToNew(typeof(PO.Line)) as PO.Line;
                linePo.LineStations = myBL.GetAllLineStationsByLineID(line.Id);
                myLineList.Add(linePo);
            }
            cbLinse.ItemsSource = myLineList;
            cbLinse.DisplayMemberPath = "Id";

            foreach(var station in myBL.GetAllStations())
            {
                myStationList.Add(station.CopyPropertiesToNew(typeof(PO.Station)) as PO.Station);
            }
            cbStations.ItemsSource = myStationList;
            cbStations.DisplayMemberPath = "Code";
        }

        private void btnBuss_Click(object sender, RoutedEventArgs e)
        {
            new BussListWin(myBusList).Show();
        }

        private void btnLines_Click(object sender, RoutedEventArgs e)
        {
            new LinesListWin(myLineList).Show();
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            new StationListWin(myStationList).Show();
        }

        private void cbBuss_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var bus = cbBuss.SelectedItem as PO.Bus;
            new BusViewInfo(bus).Show();
        }

        private void cbLinse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var line = cbLinse.SelectedItem as PO.Line;
            new LineViewInfo(line).Show();
        }

        private void cbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
