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
    /// Interaction logic for StationListWin.xaml
    /// </summary>
    public partial class StationListWin : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<BO.Station> myStationList = new ObservableCollection<BO.Station>();

        public StationListWin(ObservableCollection<BO.Station> stationList)
        {
            InitializeComponent();
            myStationList = stationList;
            lvStationList.ItemsSource = myStationList;
        }

        private void lvStationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            var station = lvStationList.SelectedItem as BO.Station;
            new StationViewInfo(station).Show();
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            gridAddNew.Visibility = Visibility.Visible;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            tbCode.BorderBrush = tbName.BorderBrush = 
                tbLatitude.BorderBrush = tbLongitude.BorderBrush = Brushes.Black;
            if (isEmptyBoxs())
                return;
            try
            {
                int code;
                if(!int.TryParse(tbCode.Text, out code) || code <= 0)
                {
                    MessageBox.Show("Erorr: not valid station code", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                double lat, lon;
                if(!double.TryParse(tbLatitude.Text, out lat))
                {
                    MessageBox.Show("Erorr: not valid latitude", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                if (!double.TryParse(tbLongitude.Text, out lon))
                {
                    MessageBox.Show("Erorr: not valid longitude", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            gridAddNew.Visibility = Visibility.Hidden;
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

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var stationToRem = lvStationList.SelectedItem as BO.Station;
            if(stationToRem == null)
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
                    lvStationList.Items.Refresh();
                }
                catch (BO.BadStationException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
