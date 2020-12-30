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

        public StationListWin(ObservableCollection<PO.Station> stationList)
        {
            InitializeComponent();
            lvStationList.ItemsSource = stationList;
        }

        private void lvStationList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var station = lvStationList.SelectedItem as PO.Station;
            new StationViewInfo(station).Show();
        }

        private void lvStationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void btnAddStation_Click(object sender, RoutedEventArgs e)
        {
            gridAddNew.Visibility = Visibility.Visible;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            tbCode.BorderBrush = tbName.BorderBrush = 
                tbLatitude.BorderBrush = tbLongitude.BorderBrush = Brushes.Black;
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
            if (emtyBox)
                return;
            try
            {
                var newStation = new BO.Station()
                {
                    Code = int.Parse(tbCode.Text),
                    IsDeleted = false,
                    Name = tbName.Text,
                    Latitude = double.Parse(tbLatitude.Text),
                    Longitude = double.Parse(tbLongitude.Text)
                };
                myBL.AddStation(newStation);

            }
            catch (BO.BadStationException ex)
            {
                MessageBox.Show("Erorr: " + ex.Message);
                return;
            }
            catch(FormatException ex)
            {
                MessageBox.Show("Erorr: " + ex.Message);
                return;
            }
            gridAddNew.Visibility = Visibility.Hidden;
        }
    }
}
