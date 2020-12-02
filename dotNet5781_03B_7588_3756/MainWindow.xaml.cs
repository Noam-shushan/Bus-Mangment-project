using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using dotNet5781_01_7588_3756;

namespace dotNet5781_03B_7588_3756
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random rand = new Random();
        public static ObservableCollection<Bus> MyBusList { get; } = new ObservableCollection<Bus>();
        public static Bus NewBus { get; set; } = null;

        public MainWindow()
        {
            initializeBusList(15);
            InitializeComponent();
            activeBuses.ItemsSource = MyBusList;
        }

        private void initializeBusList(int numOfBuss)
        {
            for(int i = 0; i < numOfBuss; i++) 
            {
                var startActivity = randomDate();
                string licenseNumber = randomLicenseNumber(startActivity);
                var lastTreatment = randomDate("lastTreatment");
                int kilometers = rand.Next(2000, 100000);
                int kilometersAfterTreatment = rand.Next(0, Bus.KILOMETER_BEFORE_TREATMENT);
                MyBusList.Add(new Bus(licenseNumber, startActivity,
                    kilometers, lastTreatment, kilometersAfterTreatment));
            }
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

        private void addBusButton_Click(object sender, RoutedEventArgs e)
        {
            new AddBus().ShowDialog();
            if(NewBus != null)
            {
                MyBusList.Add(NewBus);
                NewBus = null;
            }

        }

        private void rbChooseBusToRide_Click(object sender, RoutedEventArgs e)
        {
            new MakeRide().ShowDialog();
        }

        private void refuelingButton_Click(object sender, RoutedEventArgs e)
        {
            var busToRefuele = ((sender as Button).DataContext as Bus);
            busToRefuele.KilometersAfterFueling = 0;
        }

        private void activeBuses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedBus = activeBuses.SelectedItem as Bus;
            new SelectedBus(selectedBus).ShowDialog();
        }
    }
}