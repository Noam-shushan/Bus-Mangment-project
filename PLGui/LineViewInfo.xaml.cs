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
    /// Interaction logic for LineViewInfo.xaml
    /// </summary>
    public partial class LineViewInfo : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<BO.LineStation> myLineStationsList = new ObservableCollection<BO.LineStation>();
        BO.LineStation newLineStation; // A line station that will be used to add a new station to the line 
        string _updatePrevOrNext;

        public LineViewInfo(BO.Line line)
        {
            InitializeComponent();
            try
            {
                if (line == null)
                    Close();
                line = myBL.GetLine(line.Id); // Get the bus line updated from database
            }
            catch (BO.BadLineException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            
            textBlockLineInfo.Text = $"Line number: {line.Code}";
            textBlockArea.Text = $"Area: {line.Area.ToString().ToLower()}";
            
            foreach(var ls in line.LineStations)
            {
                myLineStationsList.Add(ls);
            }
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

        private void lvLineStations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var stationLine = lvLineStations.SelectedItem as BO.LineStation;
            if (stationLine == null)
                return;
            var station = myBL.GetStation(stationLine.Station);
            new StationViewInfo(station).Show();
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var stationToRem = lvLineStations.SelectedItem as BO.LineStation;
            if (stationToRem == null)
            {
                MessageBox.Show("Please select station to remove", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                myBL.RemoveLineStation(stationToRem);
                refreshMyList();
                Management mywin = null;
                foreach (var win in Application.Current.Windows)
                {
                    if (win.GetType().Name == "Management")
                        mywin = win as Management;
                }
                if (mywin != null)
                    mywin.refreshMyLineList();
            }
            catch (BO.BadLineException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void btnAddLineStation_Click(object sender, RoutedEventArgs e)
        {
            cbStations.ItemsSource = (from s1 in myBL.GetAllStations()
                                      where !(from s2 in myLineStationsList
                                              select s2.Station).Contains(s1.Code)
                                      select s1).Distinct().ToList();
            gridAddStation.Visibility = Visibility.Visible;
            btnUpdete.Visibility = Visibility.Visible;
        }

        private void btnPreviousStation_Click(object sender, RoutedEventArgs e)
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

        private void btnUpdete_Click(object sender, RoutedEventArgs e)
        {
            var station = cbStations.SelectedItem as BO.Station;
            if(station == null)
            {
                MessageBox.Show("Please select station to add to the line", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            newLineStation.Station = station.Code;
            bool prevMiss = false, nextMiss = false;
            BO.AdjacentStations nextStationAdst, prevStationAdst;
            try
            {
                nextStationAdst = myBL.GetAdjacentStations(newLineStation.Station, newLineStation.NextStation);
                newLineStation.DistanceFromNext = nextStationAdst.Distance;
                newLineStation.TimeFromNext = nextStationAdst.Time;
            }
            catch (BO.BadAdjacentStationsException)
            {
                MessageBox.Show($"Missing distance and time information from station {newLineStation.NextStation}", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                
                spUpdateDistAndTime.Visibility = Visibility.Visible;
                spNext.Visibility = Visibility.Visible;
                textBlockUpdateTimeAndDist.Text = $"Updete distance and time\n" 
                    + $"and from the next station {newLineStation.NextStation}";
                nextMiss = true;
            }
            try
            {
                prevStationAdst = myBL.GetAdjacentStations(newLineStation.PrevStation, newLineStation.Station);
                newLineStation.DistanceFromPrev = prevStationAdst.Distance;
                newLineStation.TimeFromPrev = prevStationAdst.Time;
            }
            catch (BO.BadAdjacentStationsException)
            {
                MessageBox.Show($"Missing distance and time information from station {newLineStation.PrevStation}", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);

                spUpdateDistAndTime.Visibility = Visibility.Visible;
                spPrev.Visibility = Visibility.Visible;
                textBlockUpdateTimeAndDist.Text = $"Updete distance and time\n" +
                    $"from the prev station {newLineStation.PrevStation}\n";
                prevMiss = true;
            }
            if (prevMiss && nextMiss)
            {
                _updatePrevOrNext = "buth";
                return;
            }    
            else if (prevMiss)
            {
                _updatePrevOrNext = "prev";
                return;
            }   
            else if (nextMiss)
            {
                _updatePrevOrNext = "next";
                return;
            }
            
            try
            {
                myBL.AddLineStation(newLineStation);

                refreshMyList(); // Updates the display list from the data source
                Management mywin = null;
                foreach (var win in Application.Current.Windows)
                {
                    if (win.GetType().Name == "Management")
                        mywin = win as Management;
                }
                if (mywin != null)
                    mywin.refreshMyLineList();
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch(BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch(BO.BadLineException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            clearGridAddSation();
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
            textBlockNextName.Text = $" {myLineStationsList.FirstOrDefault(s => s.Station == newLineStation.NextStation)?.Name }";
            textBlockPrevName.Text = $" {myLineStationsList.FirstOrDefault(s => s.Station == newLineStation.PrevStation)?.Name }";
            
            double dist1, dist2;
            TimeSpan t1, t2;
            if (!validBoxs(out dist1, out dist2, out t1, out t2, _updatePrevOrNext))
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
                        Time = t1
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
                        Time = t2
                    }); 
                }
            }
            catch (BO.BadAdjacentStationsException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            textBlockUpdateTimeAndDist.Text = "";
            tbDistancePrevStation.BorderBrush = tbDistanceNextStation.BorderBrush =
                tbTimePrevStation.BorderBrush = tbTimeNextStation.BorderBrush = Brushes.Black;
            spUpdateDistAndTime.Visibility = Visibility.Hidden;
            spPrev.Visibility = Visibility.Hidden;
            spNext.Visibility = Visibility.Hidden;
            btnUpdete_Click(sender, e);
        }

        private bool validBoxs(out double dist1, out double dist2, out TimeSpan t1, out TimeSpan t2, string select = "buth")
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
            if (!TimeSpan.TryParse(tbTimePrevStation.Text, out t1) && (select == "buth" || select == "prev"))
            {
                tbTimePrevStation.BorderBrush = Brushes.Red;
                valid = false;
            }
            if (!TimeSpan.TryParse(tbTimeNextStation.Text, out t2) && (select == "buth" || select == "next"))
            {
                tbTimeNextStation.BorderBrush = Brushes.Red;
                valid = false;
            }
            return valid;
        }
    }
}
