
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace dotNet5781_03B_7588_3756
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MyData myData = new MyData(); // my colloction of buss
        public MyBus NewBus { get; set; } = null;
        
        // Saves all windows of bus selection by double-clicking to allow user
        // to return to window to see the progress of handling and refueling
        private List<SelectedBus> winsSelectedBus = new List<SelectedBus>();
        
        private const int MY_SECONDS = 100;
        private const int TIME_FOR_TREATMENT = 144 * 1000;
        private const int TIME_FOR_FUELING = 2 * 6 * 1000;

        public MainWindow()
        {
            myData.InitializeBusList(numOfBuss: 15);
            InitializeComponent();
            activeBuses.ItemsSource = myData.MyBusList;
        }

        public void RefreshMyView()
        {
            activeBuses.Items.Refresh();
        }

        // add new bus 
        private void addBusButton_Click(object sender, RoutedEventArgs e)
        {
            new AddBus().ShowDialog();
            if (NewBus != null)
            {
                myData.MyBusList.Add(NewBus);
                NewBus = null;
            }
        }

        // add a ride to selected bus
        private void rbChooseBusToRide_Click(object sender, RoutedEventArgs e)
        {
            var selectedBus = ((sender as Button).DataContext as MyBus);
            new MakeRide(selectedBus).ShowDialog();
            RefreshMyView();
        }
        
        // Show bus informtion and make a treatment or refueling
        private void activeBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedBus = activeBuses.SelectedItem as MyBus;
            
            if (!selectedBus.IsReady)
            {
                if (selectedBus.CurrentStatus == MyBus.Status.TREATMENT || selectedBus.CurrentStatus == MyBus.Status.REFUELING)
                {   // find the bus that in progres
                    var w = winsSelectedBus.Find(x => x.CurrentBus.LiscenseNumber == selectedBus.LiscenseNumber);
                    if(w != null)
                        w.Show();                                       
                }
                return;
            }
            
            var currWin = new SelectedBus(selectedBus);
            winsSelectedBus.Add(currWin);
            currWin.Show();
            RefreshMyView();
        }
        
        // Make a treatment to all buss that needed
        private async void TreatmentAllButton_Click(object sender, RoutedEventArgs e)
        {
            List<Task> tasks = new List<Task>();
            foreach (var b in myData.MyBusList)
            {
                if (b.NeedsTreatment())
                {
                    b.CurrentStatus = MyBus.Status.TREATMENT;
                    RefreshMyView();
                    tasks.Add(TreatmentAsync(b));
                }
            }
            await Task.WhenAll(tasks);
            foreach (var b in myData.MyBusList)
            {
                if (b.CurrentStatus == MyBus.Status.TREATMENT)
                    b.CurrentStatus = MyBus.Status.READY;
            }
            RefreshMyView();
        }
        
        // Refueling
        private async void refuelingButton_Click(object sender, RoutedEventArgs e)
        {
            var busToRefuele = ((sender as Button).DataContext as MyBus);
            await FuelingtAsync(busToRefuele);
        }
        /// <summary>
        /// Refueling the bus
        /// It takes 12 seconds in the refueling simulation of the exercise
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public async Task FuelingtAsync(MyBus bus)
        {
            bus.CurrentStatus = MyBus.Status.REFUELING;
            RefreshMyView();
            await Task.Delay(TIME_FOR_FUELING);
            bus.KilometersAfterFueling = 0;
            bus.CurrentStatus = MyBus.Status.READY;
            RefreshMyView();
        }
        /// <summary>
        /// Make a ride 
        /// </summary>
        /// <param name="bus"></param>
        /// <param name="kilometers"></param>
        /// <returns></returns>
        public async Task RideAsync(MyBus bus, int kilometers)
        {
            bus.CurrentStatus = MyBus.Status.RIDE;
            RefreshMyView();
            await Task.Delay(MY_SECONDS * (kilometers / myData.rand.Next(20, 50)));
            bus.Kilometers = kilometers;
            bus.CurrentStatus = MyBus.Status.READY;
            RefreshMyView();
        }

        /// <summary>
        /// Treatmen for a bus
        /// It takes 144 seconds in the exercise simulation clock
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public async Task TreatmentAsync(MyBus bus)
        {
            bus.CurrentStatus = MyBus.Status.TREATMENT;
            RefreshMyView();
            await Task.Delay(TIME_FOR_TREATMENT);
            bus.Treatment();
            bus.CurrentStatus = MyBus.Status.READY;
            RefreshMyView();
        }
    }
}