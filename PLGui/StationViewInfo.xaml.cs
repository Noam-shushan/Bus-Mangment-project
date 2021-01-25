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
using BL;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for StationViewInfo.xaml
    /// </summary>
    public partial class StationViewInfo : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<BO.Line> myLineList = new ObservableCollection<BO.Line>();
        ObservableCollection<BO.AdjacentStations> myAdjacentStationsList = new ObservableCollection<BO.AdjacentStations>();
        BO.Station _currStation;

        public StationViewInfo(BO.Station station)
        {
            InitializeComponent();
            try
            {
                if (station == null)
                    return;
                _currStation = myBL.GetStation(station.Code);
            }
            catch (BO.BadStationException ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            textBoxStationInfo.Text = _currStation.ToString();
            foreach (var line in _currStation.LinesPassBy)
            {
                myLineList.Add(line);
            }
            lvLinesPassBy.ItemsSource = myLineList;

            foreach (var adst in _currStation.MyAdjacentStations)
            {
                myAdjacentStationsList.Add(adst);
            }
            lvAdjacentStationsList.ItemsSource = myAdjacentStationsList;
        }

        private void lvLinesPassBy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var line = lvLinesPassBy.SelectedItem as BO.Line;
            new LineViewInfo(line).Show();
        }

        private void btnUpdateTimeAndDist_Click(object sender, RoutedEventArgs e)
        {
            if (isEmptyBoxs())
                return;
            try
            {
                var adjacentStations = lvAdjacentStationsList.SelectedItem as BO.AdjacentStations;
                double dist;
                if (!double.TryParse(tbUpdateDistanc.Text, out dist))
                {
                    MessageBox.Show("Error: not a valid distanc", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                adjacentStations.Distance = dist;
                TimeSpan time = tbUpdateTime.SelectedTime.Value.TimeOfDay;

                adjacentStations.TimeInHours = time.Hours; 
                adjacentStations.TimeInMinutes = time.Minutes;
                myBL.UpdateAdjacentStations(adjacentStations);
                lvAdjacentStationsList.Items.Refresh();
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            gridUpdate.Visibility = Visibility.Hidden;
            tbUpdateDistanc.BorderBrush = tbUpdateTime.BorderBrush = Brushes.Black;
            btnUpdateStation.Visibility = Visibility.Visible;
        }

        private void lvAdjacentStationsList_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            gridUpdate.Visibility = Visibility.Visible;
            var station = lvAdjacentStationsList.SelectedItem as BO.AdjacentStations;
            if (station == null)
            {
                MessageBox.Show("Please select station to update", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            textBlockUpdate.Text = $"Update distance and time\nfrom station {station.Station2}";
        }

        bool isEmptyBoxs()
        {
            bool isEmptyBox = false;
            if (tbUpdateDistanc.Text == string.Empty)
            {
                tbUpdateDistanc.BorderBrush = Brushes.Red;
                isEmptyBox = true;
            }
            if (tbUpdateTime.SelectedTime == null)
            {
                tbUpdateTime.BorderBrush = Brushes.Red;
                isEmptyBox = true;
            }
            return isEmptyBox;
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (tbNewName.Text != string.Empty)
            {
                _currStation.Name = tbNewName.Text;
            }
            double lat, lon;
            if(validLatLon(out lat, out lon))
            {
                _currStation.Latitude = lat;
                _currStation.Longitude = lon;
            }
            try
            {
                myBL.UpdateStation(_currStation);
                refreshMe(_currStation.Code);
                Management mywin = null;
                foreach (var win in Application.Current.Windows)
                {
                    if (win.GetType().Name == "Management")
                        mywin = win as Management;
                }
                if (mywin != null)
                    mywin.refreshMyStationList();
            }
            catch(BO.BadStationException ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            spUpdate.Visibility = Visibility.Hidden;
            btnUpdateStation.Visibility = Visibility.Visible;
        }

        private void btnUpdateStation_Click(object sender, RoutedEventArgs e)
        {
            btnUpdateStation.Visibility = Visibility.Hidden;
            spUpdate.Visibility = Visibility.Visible;
        }

        bool validLatLon(out double lat, out double lon)
        {
            bool valid = true;
            if (!double.TryParse(tbLatitude.Text, out lat))
                valid = false;
            if (!double.TryParse(tbLongitude.Text, out lon))
                valid = false;
            return valid;
        }

        void refreshMe(int code)
        {
            _currStation = myBL.GetStation(code);
            textBoxStationInfo.Text = _currStation.ToString();
        }

        private void lvAdjacentStationsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnUpdateStation.Visibility = Visibility.Hidden;
        }

        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+.]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
