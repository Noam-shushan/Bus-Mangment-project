using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for Traveling.xaml
    /// </summary>
    public partial class Traveling : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        BL.Clock myClock = BlApi.ClockFactory.GetClock();

        ObservableCollection<BO.Station> myStationList;
        ObservableCollection<BO.Line> myLineList;
        ObservableCollection<BO.LineTrip> myLinesTripList;

        Stopwatch watch;
        BackgroundWorker timerWorker;

        public Traveling()
        {
            InitializeComponent();
            
            watch = new Stopwatch();
            timerWorker = new BackgroundWorker();
            timerWorker.DoWork += Worker_DoWork;
            timerWorker.ProgressChanged += Worker_ProgressChanged;
            timerWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            timerWorker.WorkerReportsProgress = true;
            timerWorker.WorkerSupportsCancellation = true;

            myStationList = new ObservableCollection<BO.Station>(myBL.GetAllStations());
            cbStations.ItemsSource = myStationList;
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lvLinesTrip.Visibility = Visibility.Hidden;           
            clock.Height = 402;
            tbSpeed.IsEnabled = true;

            watch.Stop();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e) 
        {
            while (!timerWorker.CancellationPending) 
            {         
                timerWorker.ReportProgress(1);
                Thread.Sleep(1000);
            }
        }

        private ObservableCollection<BO.LineTrip> GetUpdeteLineTripList()
        {
            return new ObservableCollection<BO.LineTrip>(myLinesTripList);
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            myClock.CurrentTime = TimeSpan.FromTicks(watch.Elapsed.Ticks * myClock.Rate); // update the clock
            
            foreach (var lineTrip in GetUpdeteLineTripList())
            {
                if (lineTrip.CurrentTimeToStation != TimeSpan.Zero)
                {   // update the curret time to the station   
                    lineTrip.CurrentTimeToStation = myClock.Time < lineTrip.ArrivalTimeToStation ?
                            lineTrip.ArrivalTimeToStation - myClock.Time : TimeSpan.Zero;
                }
                else
                {
                    txtLastLineArrive.Text = $"The last line to arrive {lineTrip.LineCode}";
                    myLinesTripList.Remove(lineTrip);
                }
                lvLinesTrip.Items.Refresh();
            }

            watch.Restart();
            clock.Time = myClock.CurrentClock;
        }

        private void btnPlayStop_Checked(object sender, RoutedEventArgs e)
        {
            if(cbStations.SelectedItem == null)
            {
                MessageBox.Show("Please select station", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                btnPlayStop.IsChecked = false;
                return;
            }

            clock.Height = 110;
            tbSpeed.IsEnabled = false;
            lvLinesTrip.Visibility = Visibility.Visible;

            int speed;
            if (!int.TryParse(tbSpeed.Text, out speed))
                speed = 1;

            myClock.Rate = speed;
            myClock.CurrentClock = clock.Time;
            myClock.IsClockRunning = true;
            
            timerWorker.RunWorkerAsync(0);
        }

        private void btnPlayStop_Unchecked(object sender, RoutedEventArgs e)
        {
            timerWorker.CancelAsync();
            myClock.IsClockRunning = false;
        }

        private void cbStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var station = cbStations.SelectedItem as BO.Station;
            myLineList = new ObservableCollection<BO.Line>(station.LinesPassBy);
            lvLinesPassBy.ItemsSource = myLineList;

            myLinesTripList = new ObservableCollection<BO.LineTrip>(myBL.GetAllLineTripsByStation(station));

            lvLinesTrip.ItemsSource = myLinesTripList;
        }

        private void numberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9+]");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
