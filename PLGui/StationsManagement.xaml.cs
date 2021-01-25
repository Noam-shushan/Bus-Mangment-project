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
        #region Stations

        internal void refreshMyStationList()
        {
            myStationList.Clear();
            foreach (var station in myBL.GetAllStations())
            {
                myStationList.Add(station);
            }
            lvStationList.Items.Refresh();
            cbFirstStation.Items.Refresh();
            cbLastStation.Items.Refresh();
        }

        private void lvStationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var station = lvStationList.SelectedItem as BO.Station;
            new StationViewInfo(station).Show();
            lvStationList.Items.Refresh();
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            gridAddNewStation.Visibility = Visibility.Visible;
        }

        private void btnAddStationExecution_Click(object sender, RoutedEventArgs e)
        {
            tbCode.BorderBrush = tbName.BorderBrush =
                tbLatitude.BorderBrush = tbLongitude.BorderBrush = Brushes.Black;
            if (isEmptyBoxs())
                return;
            try
            {
                int code;
                if (!int.TryParse(tbCode.Text, out code) || code <= 0)
                {
                    MessageBox.Show("Error: not valid station code", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                double lat, lon;
                if (!double.TryParse(tbLatitude.Text, out lat))
                {
                    MessageBox.Show("Error: not valid latitude", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!double.TryParse(tbLongitude.Text, out lon))
                {
                    MessageBox.Show("Error: not valid longitude", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var newStation = new BO.Station()
                {
                    Code = code,
                    IsDeleted = false,
                    Name = tbName.Text,
                    Latitude = lat,
                    Longitude = lon
                };
                myBL.AddStation(newStation);
                myStationList.Add(newStation);
            }
            catch (BO.BadStationException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            gridAddNewStation.Visibility = Visibility.Hidden;
        }

        bool isEmptyBoxs()
        {
            bool emtyBox = false;
            if (tbCode.Text == string.Empty)
            {
                tbCode.BorderBrush = Brushes.Red;
                emtyBox = true;
            }
            if (tbName.Text == string.Empty)
            {
                tbName.BorderBrush = Brushes.Red;
                emtyBox = true;
            }
            if (tbLatitude.Text == string.Empty)
            {
                tbLatitude.BorderBrush = Brushes.Red;
                emtyBox = true;
            }
            if (tbLongitude.Text == string.Empty)
            {
                tbLongitude.BorderBrush = Brushes.Red;
                emtyBox = true;
            }
            return emtyBox;
        }

        private void btnRemoveStation_Click(object sender, RoutedEventArgs e)
        {
            var stationToRem = lvStationList.SelectedItem as BO.Station;
            if (stationToRem == null)
            {
                MessageBox.Show("Please select station to remove", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"You sure you want to delete station '{stationToRem.Code}'",
                        "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    myBL.RemoveStation(stationToRem);
                    myStationList.Remove(stationToRem);
                    refreshMyLineList();
                    refreshMyStationList();
                }
                catch (BO.BadStationException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (BO.BadAdjacentStationsException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch (BO.BadLineStationException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
        #endregion
    }
}