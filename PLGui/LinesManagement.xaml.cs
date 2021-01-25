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
        private void lvLineList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var line = lvLineList.SelectedItem as BO.Line;
            new LineViewInfo(line).Show();
        }

        internal void refreshMyLineList()
        {
            myLineList.Clear();
            foreach (var line in myBL.GetAllLines())
            {
                myLineList.Add(line);
            }
            lvLineList.Items.Refresh();
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
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            refreshAddGrig();
        }

        private void addLine()
        {
            int code;
            if (!int.TryParse(tbLineNumber.Text, out code))
            {
                MessageBox.Show("Error: not a valid code line", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var firstStation = cbFirstStation.SelectedItem as BO.Station;
            if (firstStation == null)
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
            if (firstStation.Code == lastStation.Code)
            {
                MessageBox.Show("Error: select diffrent stations\n to the first and the last", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            
            if (tbDistance.Text == string.Empty)
            {
                tbDistance.BorderBrush = Brushes.Red;
                return;
            }

            if (tbTimeForNewLineStation.SelectedTime == null)
            {
                tbTimeForNewLineStation.BorderBrush = Brushes.Red;
                return;
            }

            double dist;
            if (!double.TryParse(tbDistance.Text, out dist))
            {
                MessageBox.Show("Error: not valid distance", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            TimeSpan time = tbTimeForNewLineStation.SelectedTime.Value.TimeOfDay;
            try
            {
                myBL.AddAdjacentStations(new BO.AdjacentStations()
                {
                    Distance = dist,
                    IsDeleted = false,
                    Station1 = firstStation.Code,
                    Station2 = lastStation.Code,
                    TimeInHours = time.Hours,
                    TimeInMinutes = time.Minutes
                });
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            spDistAndTime.Visibility = Visibility.Hidden;
        }

        private void refreshAddGrig()
        {
            gridAddNewLine.Visibility = Visibility.Hidden;
            tbLineNumber.Text = string.Empty;
            tbLineNumber.BorderBrush = Brushes.Black;
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
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
