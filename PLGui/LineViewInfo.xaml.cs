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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for LineViewInfo.xaml
    /// </summary>
    public partial class LineViewInfo : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<BO.LineStation> myLineStationsList;
        
        BO.LineStation newLineStation; // A line station that will be used to add a new station to the line 
        string _updatePrevOrNext;

        public LineViewInfo(BO.Line line)
        {
            InitializeComponent();
            try
            {
                if (line == null)
                    Close();
                line = myBL.GetLine(line.Id); // Get the line updated from database
            }
            catch (BO.BadLineException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            
            textBlockLineInfo.Text = $"Line number: {line.Code}";
            textBlockArea.Text = $"Area: {line.Area.ToString().ToLower()}";

            myLineStationsList = new ObservableCollection<BO.LineStation>(line.LineStations);
            lvLineStations.ItemsSource = myLineStationsList;
            
            newLineStation = new BO.LineStation() // Set with defult values
            {
                LineId = line.Id, // Set the line station with the id of the line shown
                LineStationIndex = -1,
                IsDeleted = false,
                NextStation = -1,
                PrevStation = -1,
                Station = -1
            };
        }

        // on double click on one line station in the list, show the station info
        private void lvLineStations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var stationLine = lvLineStations.SelectedItem as BO.LineStation;
            if (stationLine == null)
                return;

            BO.Station station = new BO.Station();
            try
            {
                station = myBL.GetStation(stationLine.Station); // build the station
            }
            catch (BO.BadStationException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            new StationViewInfo(station).Show();
        }

        // remove station from the line
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var stationToRem = lvLineStations.SelectedItem as BO.LineStation;
            if (stationToRem == null)
            {
                MessageBox.Show("Please select station to remove", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show($"You sure you want to remove line station '{stationToRem.Name}'",
            "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    myBL.RemoveLineStation(stationToRem);
                    refreshMyList();

                    Management myWin = GuiTools.GetCurrentManagmentWin();
                    if (myWin != null)
                        myWin.refreshMyLineList(); // updete the main mangement window with the change
                }
                catch (BO.BadLineException ex)
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

        private void btnAddLineStation_Click(object sender, RoutedEventArgs e)
        {   
            // Fills the comboBox with all stations where the line does not pass
            cbStations.ItemsSource = (from s1 in myBL.GetAllStations()
                                      where !(from s2 in myLineStationsList
                                              select s2.Station).Contains(s1.Code)
                                      select s1).Distinct().ToList();  

            gridAddStation.Visibility = Visibility.Visible;
            btnUpdete.Visibility = Visibility.Visible;
        }

        // the previous station that the user select for the new line station
        private void btnSelectPrevStation_Click(object sender, RoutedEventArgs e)
        {
            var prev = lvLineStations.SelectedItem as BO.LineStation;
            if (prev == null)
            {
                MessageBox.Show("Please select a previous station", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            newLineStation.PrevStation = prev.Station;
            newLineStation.NextStation = prev.NextStation;
            btnUpdete.IsEnabled = true;
        }
        
        // Add new line station to the line
        private void btnUpdete_Click(object sender, RoutedEventArgs e)
        {
            var station = cbStations.SelectedItem as BO.Station;
            if(station == null)
            {
                MessageBox.Show("Please select station to add to the line", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            newLineStation.Station = station.Code;

            if (!isMissDistAndTime())
                return;

            try
            {
                myBL.AddLineStation(newLineStation);

                refreshMyList(); // Updates the display list from the data source
                Management myWin = GuiTools.GetCurrentManagmentWin();
                if (myWin != null)
                    myWin.refreshMyLineList();
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch(BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch(BO.BadLineException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            clearGridAddSation();
        }

        /* check if there is time and distance infromtion in the database
         * if exist, add it to the new line station and update the prev and the next
         */
        private bool isMissDistAndTime()
        {
            // Flags to know if there infromation of distance and time in the database
            bool prevMiss = false, nextMiss = false;
            try
            {
                myBL.AddDistEndTimeToNewLineStation(newLineStation, out prevMiss, out nextMiss);
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (prevMiss && nextMiss)
            {
                spUpdateDistAndTime.Visibility = Visibility.Visible;
                spNext.Visibility = Visibility.Visible;
                spPrev.Visibility = Visibility.Visible;
                textBlockUpdateTimeAndDist.Text = $"Updete distance and time\n"
                    + $"from the next station {newLineStation.NextStation}\n" +
                    $"and from prev station {newLineStation.PrevStation}";
                _updatePrevOrNext = "buth";
                return false;
            }
            else if (prevMiss)
            {
                _updatePrevOrNext = "prev";
                spUpdateDistAndTime.Visibility = Visibility.Visible;
                spPrev.Visibility = Visibility.Visible;
                textBlockUpdateTimeAndDist.Text = $"Updete distance and time\n" +
                    $"from the prev station {newLineStation.PrevStation}\n";
                return false;
            }
            else if (nextMiss)
            {
                _updatePrevOrNext = "next";
                spUpdateDistAndTime.Visibility = Visibility.Visible;
                spNext.Visibility = Visibility.Visible;
                textBlockUpdateTimeAndDist.Text = $"Updete distance and time\n"
                    + $"from the next station {newLineStation.NextStation}";
                return false;
            }
            return true;
        }

        private void clearGridAddSation()
        {
            gridAddStation.Visibility = Visibility.Hidden;
            btnUpdete.Visibility = Visibility.Hidden;
            newLineStation.LineStationIndex = -1;
            newLineStation.NextStation = -1;
            newLineStation.PrevStation = -1;
            newLineStation.Station = -1;
            btnUpdete.IsEnabled = false;
        }

        /// <summary>
        /// Updates the display list from the data source
        /// </summary>
        private void refreshMyList()
        {
            myLineStationsList.Clear();
            foreach (var s in myBL.GetLine(newLineStation.LineId).LineStations)
                myLineStationsList.Add(s);
            
            lvLineStations.Items.Refresh();
        }

        private void btnUpdateDistAndTime_Click(object sender, RoutedEventArgs e)
        {
            double dist1, dist2;
            TimeSpan t1, t2;
            
            if (!validDist(out dist1, out dist2, _updatePrevOrNext))
                return;
            
            if (!validTime(out t1, out t2, _updatePrevOrNext))
                return;

            try
            {
                if (_updatePrevOrNext == "buth" || _updatePrevOrNext == "prev")
                {
                    myBL.AddAdjacentStations(new BO.AdjacentStations
                    {
                        Distance = dist1,
                        IsDeleted = false,
                        Station1 = newLineStation.PrevStation,
                        Station2 = newLineStation.Station,
                        TimeInHours = t1.Hours,
                        TimeInMinutes = t1.Minutes
                    }); 
                }

                if (_updatePrevOrNext == "buth" || _updatePrevOrNext == "next")
                {
                    myBL.AddAdjacentStations(new BO.AdjacentStations
                    {
                        Distance = dist2,
                        IsDeleted = false,
                        Station1 = newLineStation.Station,
                        Station2 = newLineStation.NextStation,
                        TimeInHours = t2.Hours,
                        TimeInMinutes = t2.Minutes
                    }); 
                }
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            
            // clear the view
            textBlockUpdateTimeAndDist.Text = "";
            tbDistancePrevStation.BorderBrush = tbDistanceNextStation.BorderBrush =
                tbTimePrevStation.BorderBrush = tbTimeNextStation.BorderBrush = Brushes.Black;
            spUpdateDistAndTime.Visibility = Visibility.Hidden;
            spPrev.Visibility = Visibility.Hidden;
            spNext.Visibility = Visibility.Hidden;
            
            // call the add new line station button
            btnUpdete_Click(sender, e); 
        }

        private bool validDist(out double dist1, out double dist2, string select = "buth")
        {
            bool valid = true;
            
            if (!double.TryParse(tbDistancePrevStation.Text, out dist1) && (select == "buth" || select == "prev"))
            {
                tbDistancePrevStation.BorderBrush = Brushes.Red;
                valid = false;
            }
            if (!double.TryParse(tbDistanceNextStation.Text, out dist2) && (select == "buth" || select == "next"))
            {
                tbDistanceNextStation.BorderBrush = Brushes.Red;
                valid = false;
            }
            
            return valid;
        }

        private bool validTime(out TimeSpan t1, out TimeSpan t2, string select = "buth")
        {
            bool valid = true;
            t1 = t2 = TimeSpan.Zero;
            if (tbTimePrevStation.SelectedTime == null && (select == "buth" || select == "prev"))
            {
                tbTimePrevStation.BorderBrush = Brushes.Red;
                valid = false;
            }
            else if (tbTimePrevStation.SelectedTime != null)
            {
                t1 = tbTimePrevStation.SelectedTime.Value.TimeOfDay;
            }

            if (tbTimeNextStation.SelectedTime == null && (select == "buth" || select == "next"))
            {
                tbTimeNextStation.BorderBrush = Brushes.Red;
                valid = false;
            }
            else if (tbTimeNextStation.SelectedTime != null)
            {
                t2 = tbTimeNextStation.SelectedTime.Value.TimeOfDay;
            }

            return valid;
        }

        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+.]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
