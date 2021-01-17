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
        ObservableCollection<BO.Line> myLineList = new ObservableCollection<BO.Line>();
        ObservableCollection<BO.Station> myStationList = new ObservableCollection<BO.Station>();
     
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


            foreach (var line in myBL.GetAllLines())
            {
                myLineList.Add(line);
            }
            lvLineList.ItemsSource = myLineList;

            foreach(var station in myBL.GetAllStations())
            {
                myStationList.Add(station);
            }
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

        #region Lines
        internal void refreshMyLineList()
        {
            myLineList.Clear();
            foreach (var line in myBL.GetAllLines())
            {
                myLineList.Add(line);
            }
            lvLineList.Items.Refresh();
        }

        private void lvLineList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var line = lvLineList.SelectedItem as BO.Line;
            new LineViewInfo(line).Show();
        }

        private void btnAddNewLine_Click(object sender, RoutedEventArgs e)
        {
            gridAddNewLine.Visibility = Visibility.Visible;
        }

        private void btnAddLine_Click(object sender, RoutedEventArgs e)
        {
            if (tbLineNumber.Text == string.Empty)
            {
                tbLineNumber.BorderBrush = Brushes.Red;
                return;
            }
            try
            {
                addLine();
            }
            catch (BO.BadLineException ex)
            {
                if (ex.Message.StartsWith("M"))
                {
                    MessageBox.Show(ex.Message, "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    spDistAndTime.Visibility = Visibility.Visible; 
                }
                else
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            refreshAddGrig();
        }

        private void addLine()
        {
            int code;
            if (!int.TryParse(tbLineNumber.Text, out code))
            {
                MessageBox.Show("Erorr: not a valid code line", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var firstStation = cbFirstStation.SelectedItem as BO.Station;
            if(firstStation == null)
            {
                MessageBox.Show("Please select first station", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var lastStation = cbLastStation.SelectedItem as BO.Station;
            if (lastStation == null)
            {
                MessageBox.Show("Please select last station", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(firstStation.Code == lastStation.Code)
            {
                MessageBox.Show("Erorr: select diffrent stations\n to the first and the last", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int id;
            var newLine = new BO.Line()
            {
                Code = code,
                FirstStation = firstStation.Code,
                LastStation = lastStation.Code,
                IsDeleted = false,
            };
            id = myBL.AddLine(newLine);
            myBL.AddLineStation(new BO.LineStation()
            {
                IsDeleted = false,
                LineId = id,
                LineStationIndex = 0,
                Station = newLine.FirstStation,
                NextStation = newLine.LastStation,
                PrevStation = 0
            });
            myBL.AddLineStation(new BO.LineStation()
            {
                IsDeleted = false,
                LineId = id,
                LineStationIndex = 1,
                Station = newLine.LastStation,
                NextStation = -1,
                PrevStation = newLine.FirstStation
            });
            newLine = myBL.GetLine(id);
            myLineList.Add(newLine);
            lvLineList.Items.Refresh();
        }

        private void btnUpdateDistAndTime_Click(object sender, RoutedEventArgs e)
        {
            var firstStation = cbFirstStation.SelectedItem as BO.Station;
            var lastStation = cbLastStation.SelectedItem as BO.Station;

            if(tbDistance.Text == string.Empty)
            {
                tbDistance.BorderBrush = Brushes.Red;
                return;
            }
            
            if (tbTimeForNewLineStation.Text == string.Empty)
            {
                tbTimeForNewLineStation.BorderBrush = Brushes.Red;
                return;
            }
            
            double dist;
            if(!double.TryParse(tbDistance.Text, out dist))
            {
                MessageBox.Show("Erorr: not valid distance", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            TimeSpan time;
            if(!TimeSpan.TryParse(tbTimeForNewLineStation.Text, out time))
            {
                MessageBox.Show("Erorr: not valid time", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                myBL.AddAdjacentStations(new BO.AdjacentStations()
                {
                    Distance = dist,
                    IsDeleted = false,
                    Station1 = firstStation.Code,
                    Station2 = lastStation.Code,
                    Time = time,
                });
            }
            catch(BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            spDistAndTime.Visibility = Visibility.Hidden;
        }

        private void refreshAddGrig()
        {
            gridAddNewLine.Visibility = Visibility.Hidden;
            tbLineNumber.Text = string.Empty;
            tbLineNumber.BorderBrush =  Brushes.Black;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var lineToDel = lvLineList.SelectedItem as BO.Line;
            if (lineToDel == null)
            {
                MessageBox.Show("Please select line to remove", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"You sure you want to delete line '{lineToDel.Code}'",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    myBL.RemoveLine(lineToDel);
                    myLineList.Remove(lineToDel);
                    lvLineList.Items.Refresh();
                }
                catch (BO.BadLineException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
        #endregion

        #region Stations

        internal void refreshMyStationList()
        {
            myStationList.Clear();
            foreach (var station in myBL.GetAllStations())
            {
                myStationList.Add(station);
            }
            lvStationList.Items.Refresh();
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
                    MessageBox.Show("Erorr: not valid station code", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                double lat, lon;
                if (!double.TryParse(tbLatitude.Text, out lat))
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
                    lvStationList.Items.Refresh();
                    lvLineList.Items.Refresh();
                }
                catch (BO.BadStationException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch(BO.BadAdjacentStationsException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                catch(BO.BadLineStationException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
        #endregion

        #region Buss
   
        private void lvBusList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bus = lvBusList.SelectedItem as PO.Bus;
            new BusViewInfo(bus).Show();
        }

        private async void btnTreatment_Click(object sender, RoutedEventArgs e)
        {
            var bus = lvBusList.SelectedItem as PO.Bus;
            if(bus == null)
            {
                MessageBox.Show("Please select bus to send to a treatment", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus;
            try
            {
                myBL.BusServices(busBo, "Treatment");
                spService.Visibility = Visibility.Hidden;
                bus.MaxProgressValue = Constans.TIME_FOR_TREATMENT;
                refreshMyBussList();
                await Task.Delay(Constans.TIME_FOR_TREATMENT);
                busBo.Status = BO.BusStatus.READY;
                myBL.UpdateBus(busBo);
                refreshMyBussList();
            }
            catch (BO.BadBusException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            spService.Visibility = Visibility.Hidden;
        }

        private async void btnRefueling_Click(object sender, RoutedEventArgs e)
        {
            var bus = (lvBusList.SelectedItem as PO.Bus);
            if (bus == null)
            {
                MessageBox.Show("Please select bus to send to a refueling", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus;
            try
            {
                myBL.BusServices(busBo, "Refueling");
                spService.Visibility = Visibility.Hidden;
                bus.MaxProgressValue = Constans.TIME_FOR_FUELING;
                refreshMyBussList();
                await Task.Delay(Constans.TIME_FOR_FUELING);
                busBo.Status = BO.BusStatus.READY;
                myBL.UpdateBus(busBo);
                refreshMyBussList();
            }
            catch (BO.BadBusException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            spService.Visibility = Visibility.Hidden;
        }

        private void btnRide_Click(object sender, RoutedEventArgs e)
        {
            var bus = (lvBusList.SelectedItem as PO.Bus);
            if (bus == null)
            {
                MessageBox.Show("Please select bus to send to a Ride", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            _selectedBusToRide = bus;
            tbKilometr.Visibility = Visibility.Visible;
        }

        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private async void tbKilometr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (tbKilometr.Text == string.Empty)
                {
                    tbKilometr.BorderBrush = Brushes.Red;
                    return;
                }

                var busBo = _selectedBusToRide.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus;
                int kilometers = int.Parse(tbKilometr.Text);
                try
                {
                    myBL.BusServices(busBo, "Ride", kilometers);
                    spService.Visibility = Visibility.Hidden;
                    var timeToRide = Constans.MY_SECONDS * (kilometers / rand.Next(20, 50));
                    _selectedBusToRide.MaxProgressValue = timeToRide;
                    refreshMyBussList();
                    await Task.Delay(timeToRide);
                    busBo.Status = BO.BusStatus.READY;
                    myBL.UpdateBus(busBo);
                    refreshMyBussList();
                }
                catch (BO.BadBusException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                finally
                {
                    _selectedBusToRide = null;
                    tbKilometr.Text = "";
                    tbKilometr.Visibility = Visibility.Hidden;
                    spService.Visibility = Visibility.Hidden;
                }
            }
        }

        private void tbKilometr_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbKilometr.BorderBrush = Brushes.Black;
        }
        

        private void btnRemoveBus_Click(object sender, RoutedEventArgs e)
        {
            var bus = lvBusList.SelectedItem as PO.Bus;
            if(bus == null)
            {
                MessageBox.Show("Please select bus to remove", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"You sure you want to delete bus '{bus.LicenseNumFormat}'",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    var busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus;
                    myBL.RemoveBus(busBo);
                    myBusList.Remove(bus);
                    lvBusList.Items.Refresh();
                }
                catch (BO.BadBusException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }

        private void btnAddeBus_Click(object sender, RoutedEventArgs e)
        {
            gridAddBus.Visibility = Visibility.Visible;
        }
        

        private void btnAddNewBus_Click(object sender, RoutedEventArgs e)
        {
            if(dpDateFrom.SelectedDate == null)
            {
                dpDateFrom.BorderBrush = Brushes.Red;
                return;
            }
            if(tbLicenseNum.Text == string.Empty)
            {
                tbLicenseNum.BorderBrush = Brushes.Red;
                return;
            }

            int licenseNum;
            if(!int.TryParse(tbLicenseNum.Text, out licenseNum) || licenseNum < 0)
            {
                MessageBox.Show("Erorr: not valid number", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            BO.Bus newBus = new BO.Bus()
            {
                FromDate = dpDateFrom.SelectedDate.Value,
                LicenseNum = licenseNum,
                IsDeleted = false,
                FuelRemain = 0,
                KilometersAfterFueling = 0,
                KilometersAfterTreatment = 0,
                LastTreatment = DateTime.Now,
                Status = BO.BusStatus.READY,
                TotalTrip = 0
            };
            try
            {
                myBL.AddBus(newBus);
                var bus = myBL.GetBus(newBus.LicenseNum).CopyPropertiesToNew(typeof(PO.Bus)) as PO.Bus;
                bus.LicenseNumFormat = newBus.FormatLiscenseNumber();
                myBusList.Add(bus);
                lvBusList.Items.Refresh();
            }
            catch (BO.BadBusException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            dpDateFrom.BorderBrush = tbLicenseNum.BorderBrush = Brushes.Black;
            tbLicenseNum.Text = "";
            gridAddBus.Visibility = Visibility.Hidden;
        }

        internal void refreshMyBussList()
        {
            myBusList.Clear();
            foreach (var bus in myBL.GetAllBuss())
            {
                var busPo = bus.CopyPropertiesToNew(typeof(PO.Bus)) as PO.Bus;
                busPo.LicenseNumFormat = bus.FormatLiscenseNumber();
                myBusList.Add(busPo);
            }
            lvBusList.Items.Refresh();
        }

        private void lvBusList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = lvBusList.SelectedItem as PO.Bus;
            if (item != null && item.IsReady)
                spService.Visibility = Visibility.Visible;
            else
                spService.Visibility = Visibility.Hidden;
        }
        #endregion

    }
}
