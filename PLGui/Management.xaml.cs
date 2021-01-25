using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
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
        ObservableCollection<BO.Line> myLineList;
        ObservableCollection<BO.Station> myStationList;
     
        PO.Bus _selectedBusToRide;
        Random rand = new Random(1000);

        public Management()
        {
            InitializeComponent();
            foreach (var bus in myBL.GetAllBuss())
            {
                var busPo = bus.CopyPropertiesToNew(typeof(PO.Bus)) as PO.Bus;
                busPo.LicenseNumFormat = bus.FormatLiscenseNumber();
                myBusList.Add(busPo);
            }
             lvBusList.ItemsSource = myBusList;

            myLineList = new ObservableCollection<BO.Line>(myBL.GetAllLines());
            lvLineList.ItemsSource = myLineList;

            myStationList = new ObservableCollection<BO.Station>(myBL.GetAllStations());
            lvStationList.ItemsSource = myStationList;
            
            cbFirstStation.ItemsSource = myStationList;
            cbLastStation.ItemsSource = myStationList;
        }

        #region Visibility of the grids (Lines, Buss, Stations)
        
        private void btnBuss_Click(object sender, RoutedEventArgs e)
        {
            gridStations.Visibility = Visibility.Collapsed;
            gridLines.Visibility = Visibility.Collapsed;
            gridBuss.Visibility = Visibility.Visible;
        }

        private void btnLines_Click(object sender, RoutedEventArgs e)
        {
            gridStations.Visibility = Visibility.Collapsed;
            gridBuss.Visibility = Visibility.Collapsed;
            gridLines.Visibility = Visibility.Visible;
        }

        private void btnStations_Click(object sender, RoutedEventArgs e)
        {
            gridLines.Visibility = Visibility.Collapsed;
            gridBuss.Visibility = Visibility.Collapsed;
            gridStations.Visibility = Visibility.Visible;
        } 
        #endregion

    }
}
