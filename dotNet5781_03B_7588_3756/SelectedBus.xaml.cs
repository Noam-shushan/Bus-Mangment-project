using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace dotNet5781_03B_7588_3756
{
    /// <summary>
    /// Displays bus data
    /// Send for treatment, refueling
    /// </summary>
    public partial class SelectedBus : Window 
    {
        public MyBus CurrentBus { get; set; } = null;
        private MainWindow mainWin = null;
        private bool fuelingFlag = false;
        private bool treatmentFlag = false;
        DispatcherTimer timer = new DispatcherTimer();
        DateTime start; // To show the time left for treatment or refueling

        public SelectedBus(MyBus selectedBus)
        {
            InitializeComponent();
            
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;

            mainWin = (MainWindow)Application.Current.MainWindow;
            CurrentBus = selectedBus;
            setBusInfo();
        }

        //to allow the user to see the progres of the refueling or the treatment even if the user as close the window
        protected override void OnClosing(CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }

        // set the value of my two progres bar
        private void timer_Tick(object sender, EventArgs e)
        {
            if (fuelingFlag)
            {
                pbFueling.Value += timer.Interval.TotalSeconds;
                lTimerFueil.Text = $"{(DateTime.Now - start).Minutes}:{(DateTime.Now - start).Seconds + 1} / 00:12";
            }
            
            if (treatmentFlag)
            {
                pbTreatment.Value += timer.Interval.TotalSeconds;
                lTimerTreat.Text = $"{(DateTime.Now - start).Minutes}:{(DateTime.Now - start).Seconds + 1} / 02:24";
            }
        }

        private async void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            FuelingButton.IsEnabled = false;
            TreatmentButton.IsEnabled = false;
            timer.Start();
            start = DateTime.Now;
            treatmentFlag = true;
            await mainWin.TreatmentAsync(CurrentBus);
            timer.Stop();
            pbTreatment.Value = 0;
            treatmentFlag = false;
            TreatmentButton.IsEnabled = true;
            if (CurrentBus.NeedRefueling()) // If the bus also needs refueling it comes out refueled from the treatment 
                await fueling();
            setBusInfo();
            FuelingButton.IsEnabled = true;
        }

        private async void FuelingButton_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentBus.NeedsTreatment())
            {
                MessageBox.Show("This bus needs treatment first!");
                return;
            }
            TreatmentButton.IsEnabled = false;
            FuelingButton.IsEnabled = false;
            await fueling();
            setBusInfo();
            TreatmentButton.IsEnabled = true;
            FuelingButton.IsEnabled = true;
        }

        private async Task fueling()
        {
            timer.Start();
            start = DateTime.Now;
            fuelingFlag = true;
            await mainWin.FuelingtAsync(CurrentBus);
            timer.Stop();
            pbFueling.Value = 0;
            fuelingFlag = false;
        }

        private void setBusInfo()
        {
            string needTreatment = CurrentBus.NeedsTreatment() ? "\nThis bus needs treatment" : "";
            string needRefueling = CurrentBus.NeedRefueling() ? "\nThis bus needs refueling" : "";
            tbBusInfo.Text = $"{CurrentBus}" +
                $"\nStart activity: {CurrentBus.StartActivity.Value.Date.ToString("dd/MM/yyyy")}" +
                $"\nLast treatment: {CurrentBus.LastTreatment.Value.Date.ToString("dd/MM/yyyy")}" +
                $"{needTreatment}" +
                $"{needRefueling}";
        }
    }
}
