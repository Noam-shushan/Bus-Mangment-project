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
        #region Buss

        private void lvBusList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var bus = lvBusList.SelectedItem as PO.Bus;
            new BusViewInfo(bus).Show();
        }

        private async void btnTreatment_Click(object sender, RoutedEventArgs e)
        {
            var bus = lvBusList.SelectedItem as PO.Bus;
            if (bus == null)
            {
                MessageBox.Show("Please select bus to send to a treatment", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var busBo = bus.CopyPropertiesToNew(typeof(BO.Bus)) as BO.Bus;
            try
            {
                myBL.BusServices(busBo, "Treatment");
                spService.Visibility = Visibility.Hidden;
                refreshMyBussList();
                await Task.Delay(Constans.TIME_FOR_TREATMENT);
                busBo.Status = BO.BusStatus.READY;
                myBL.UpdateBus(busBo);
                refreshMyBussList();
            }
            catch (BO.BadBusException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                refreshMyBussList();
                await Task.Delay(Constans.TIME_FOR_FUELING);
                busBo.Status = BO.BusStatus.READY;
                myBL.UpdateBus(busBo);
                refreshMyBussList();
            }
            catch (BO.BadBusException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            tbKilometr.Focus();
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
                    refreshMyBussList();
                    await Task.Delay(timeToRide);
                    busBo.Status = BO.BusStatus.READY;
                    myBL.UpdateBus(busBo);
                    refreshMyBussList();
                }
                catch (BO.BadBusException ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (bus == null)
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
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            if (dpDateFrom.SelectedDate == null)
            {
                dpDateFrom.BorderBrush = Brushes.Red;
                return;
            }
            if (tbLicenseNum.Text == string.Empty)
            {
                tbLicenseNum.BorderBrush = Brushes.Red;
                return;
            }

            int licenseNum;
            if (!int.TryParse(tbLicenseNum.Text, out licenseNum) || licenseNum < 0)
            {
                MessageBox.Show("Error: not valid number", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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