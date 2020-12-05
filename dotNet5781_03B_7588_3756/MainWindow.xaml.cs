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
        private Random rand = new Random();
        public ObservableCollection<MyBus> MyBusList { get; } = new ObservableCollection<MyBus>();
        public static MyBus NewBus { get; set; } = null;
        public const int MY_SECONDS = 100;
        public const int TIME_FOR_TREATMENT =  144 * 1000;
        public const int TIME_FOR_FUELING = 2 * 6 * 1000;

        public MainWindow()
        {
            initializeBusList(15);
            InitializeComponent();
            activeBuses.ItemsSource = MyBusList;
        }



        private void addBusButton_Click(object sender, RoutedEventArgs e)
        {
            new AddBus().Show();
            if(NewBus != null)
            {
                MyBusList.Add(NewBus);
                NewBus = null;
            }

        }

        private void rbChooseBusToRide_Click(object sender, RoutedEventArgs e)
        {
            var selectedBus = ((sender as Button).DataContext as MyBus);
            new MakeRide(selectedBus).ShowDialog();
            activeBuses.Items.Refresh();
        }


        private void activeBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedBus = activeBuses.SelectedItem as MyBus;
            new SelectedBus(selectedBus).Show();
            activeBuses.Items.Refresh();
        }

        private async void TreatmentAllButton_Click(object sender, RoutedEventArgs e)
        {
            List<Task> tasks = new List<Task>();
            foreach(var b in MyBusList)
            {
                if (b.NeedsTreatment())
                {
                    b.CurrentStatus = MyBus.Status.TREATMENT;
                    tasks.Add(TreatmentAsync(b));
                }
                    
            }
            await Task.WhenAll(tasks);
            foreach(var b in MyBusList)
            {
                if (b.CurrentStatus == MyBus.Status.TREATMENT)
                    b.CurrentStatus = MyBus.Status.READY;
            }
            activeBuses.Items.Refresh();
        }
        // Refueling
        private async void refuelingButton_Click(object sender, RoutedEventArgs e)
        {
            var busToRefuele = ((sender as Button).DataContext as MyBus);
            await FuelingtAsync(busToRefuele);
            activeBuses.Items.Refresh();
        }
        
        /// <summary>
        /// Treatmen for a bus
        /// It takes 144 seconds in the exercise simulation clock
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public static async Task TreatmentAsync(MyBus bus)
        {
            bus.CurrentStatus = MyBus.Status.TREATMENT;
            bus.Treatment();
            await Task.Delay(TIME_FOR_TREATMENT);
            bus.CurrentStatus = MyBus.Status.READY;
        }
        /// <summary>
        /// Refueling the bus
        /// It takes 6 seconds in the refueling simulation of the exercise
        /// </summary>
        /// <param name="bus"></param>
        /// <returns></returns>
        public static async Task FuelingtAsync(MyBus bus)
        {
            bus.CurrentStatus = MyBus.Status.REFUELING;
            bus.KilometersAfterFueling = 0;
            await Task.Delay(TIME_FOR_FUELING);
            bus.CurrentStatus = MyBus.Status.READY;
        }

        private void initializeBusList(int numOfBuss)
        {
            for (int i = 0; i < numOfBuss; i++)
            {
                var startActivity = randomDate();
                string licenseNumber = randomLicenseNumber(startActivity);
                var lastTreatment = randomDate("lastTreatment");
                int kilometers = rand.Next(2000, 100000);
                int kilometersAfterTreatment = rand.Next(0, Bus.KILOMETER_BEFORE_TREATMENT);
                MyBusList.Add(new MyBus(licenseNumber, startActivity,
                    kilometers, lastTreatment, kilometersAfterTreatment));
            }
            MyBusList[0].Treatment();
            MyBusList[1].KilometersAfterTreatment = Bus.KILOMETER_BEFORE_TREATMENT - 1;
            MyBusList[2].KilometersAfterTreatment = Bus.MAX_KILOMETER_AFTER_REFUELING - 1;
        }

        private DateTime randomDate(string begin = "startActivity")
        {
            DateTime start = begin == "startActivity" ?
                new DateTime(2000, 1, 1) : new DateTime(2018, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rand.Next(range));
        }

        private string randomLicenseNumber(DateTime startActivity)
        {
            string res;
            if (startActivity.Year < 2018)
            {
                res = rand.Next(10000000).ToString();
                int numOfZeros = Math.Abs(res.Length - 7);
                for (int j = 0; j < numOfZeros; j++)
                    res = res.Insert(0, "0");
            }
            else
            {
                res = rand.Next(100000000).ToString();
                int numOfZeros = Math.Abs(res.Length - 8);
                for (int j = 0; j < numOfZeros; j++)
                    res = res.Insert(0, "0");
            }
            return res;
        }
    }

}