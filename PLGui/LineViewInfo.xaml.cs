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
        BO.LineStation newLineStation; 

        public LineViewInfo(BO.Line line)
        {
            InitializeComponent();
            try
            {
                if (line == null)
                    return;
                line = myBL.GetLine(line.Id);
            }
            catch (BO.BadLineException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            textBlockLineInfo.Text = $"Line number: {line.Code}";
            textBlockArea.Text = $"Area: {line.Area.ToString().ToLower()}";
            foreach(var ls in line.LineStations)
            {
                myLineStationsList.Add(ls);
            }

            newLineStation = new BO.LineStation()
            {
                LineId = line.Id,
                LineStationIndex = -1,
                IsDeleted = false,
                NextStation = -1,
                PrevStation = -1,
                Station = -1
            };
            lvLineStations.ItemsSource = myLineStationsList;
        }

        private void lvLineStations_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var stationLine = lvLineStations.SelectedItem as BO.LineStation;
            if (stationLine == null)
                return;
            var station = myBL.GetStation(stationLine.Station);
            new StationViewInfo(station).Show();
        }

        private void btnUpdete_Click(object sender, RoutedEventArgs e)
        {
            if(tbNewStationNumber.Text == string.Empty)
            {
                tbNewStationNumber.BorderBrush = Brushes.Red;
                return;
            }
            try
            {
                int stationCode;
                if(!int.TryParse(tbNewStationNumber.Text, out stationCode))
                {
                    MessageBox.Show("Erorr: not valid station number", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                newLineStation.Station = stationCode;
                int lineCode = myBL.GetLine(newLineStation.LineId).Code;
                myBL.AddLineStation(newLineStation);
                myBL.AddAdjacentStations(new BO.AdjacentStations()
                {
                    Station1 = newLineStation.Station,
                    Station2 = newLineStation.NextStation,
                    IsDeleted = false,
                    LineCode = lineCode,
                });
                myBL.AddAdjacentStations(new BO.AdjacentStations()
                {
                    Station1 = newLineStation.PrevStation,
                    Station2 = newLineStation.Station,
                    IsDeleted = false,
                    LineCode = lineCode,
                });
                refreshMyList();
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
            tbNewStationNumber.Text = "";
            tbNewStationNumber.BorderBrush = Brushes.Black;
            gridAddStation.Visibility = Visibility.Hidden;
            btnUpdete.Visibility = Visibility.Hidden;
            newLineStation.LineStationIndex = -1;
            newLineStation.NextStation = -1;
            newLineStation.PrevStation = -1;
            newLineStation.Station = -1;
        }

        private void btnAddLineStation_Click(object sender, RoutedEventArgs e)
        {
            gridAddStation.Visibility = Visibility.Visible;
            btnUpdete.Visibility = Visibility.Visible;
        }

        private void btnPreviousStation_Click(object sender, RoutedEventArgs e)
        {
            var prev = lvLineStations.SelectedItem as BO.LineStation;
            if(prev == null)
            {
                MessageBox.Show("Please select a previous station", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            newLineStation.PrevStation = prev.Station;
            newLineStation.NextStation = prev.NextStation;
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
            }
            catch(BO.BadLineException ex)
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

        private void refreshMyList()
        {
            myLineStationsList.Clear();
            foreach (var s in myBL.GetLine(newLineStation.LineId).LineStations)
                myLineStationsList.Add(s);
        }
    }
}
