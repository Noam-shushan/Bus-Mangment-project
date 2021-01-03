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
    /// Interaction logic for StationViewInfo.xaml
    /// </summary>
    public partial class StationViewInfo : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<BO.Line> myLineList = new ObservableCollection<BO.Line>();
        ObservableCollection<BO.AdjacentStations> myAdjacentStationsList = new ObservableCollection<BO.AdjacentStations>();

        public StationViewInfo(BO.Station station)
        {
            InitializeComponent();
            try
            {
                if (station == null)
                    return;
                station = myBL.GetStation(station.Code);
            }
            catch (BO.BadStationException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            textBoxStationInfo.Text = station.ToString();
            foreach (var line in station.LinesPassBy)
            {
                myLineList.Add(line);
            }
            lvLinesPassBy.ItemsSource = myLineList;

            foreach (var adst in station.MyAdjacentStations)
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

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (isEmptyBoxs())
                return;
            try
            {
                var adjacentStations = lvAdjacentStationsList.SelectedItem as BO.AdjacentStations;
                double dist;
                if (!double.TryParse(tbUpdateDistanc.Text, out dist) || dist < 0)
                {
                    MessageBox.Show("Erorr: not a valid distanc", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                adjacentStations.Distance = dist;
                TimeSpan time;
                if (!TimeSpan.TryParse(tbUpdateTime.Text, out time))
                {
                    MessageBox.Show("Erorr: not a valid time", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                adjacentStations.Time = time;
                myBL.UpdateAdjacentStations(adjacentStations);
                lvAdjacentStationsList.Items.Refresh();
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            gridUpdate.Visibility = Visibility.Hidden;
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
            tbUpdateDistanc.BorderBrush = tbUpdateTime.BorderBrush = Brushes.Black;
            tbUpdateDistanc.Text = tbUpdateTime.Text = string.Empty;
        }

        bool isEmptyBoxs()
        {
            bool isEmptyBox = false;
            if (tbUpdateDistanc.Text == string.Empty)
            {
                tbUpdateDistanc.BorderBrush = Brushes.Red;
                isEmptyBox = true;
            }
            if (tbUpdateTime.Text == string.Empty)
            {
                tbUpdateTime.BorderBrush = Brushes.Red;
                isEmptyBox = true;
            }
            return isEmptyBox;
        }

        private void tbUpdateTime_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            TextBox txtBox = sender as TextBox;
            if (txtBox.Text == "hh:mm:ss")
            {
                txtBox.Text = string.Empty;
            }
        }
    }
}
