using System;
using System.Collections.Generic;
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

namespace dotNet5781_03B_7588_3756
{
    /// <summary>
    /// Interaction logic for MakeRide.xaml
    /// </summary>
    public partial class MakeRide : Window
    {
        private MyBus myBus = null;
        private Random rand = new Random();

        public MakeRide(MyBus busToRide)
        {
            myBus = busToRide;
            InitializeComponent();
        }

        private async void ApplyRideButton_Click(object sender, RoutedEventArgs e)
        {
            var validTesk = await rideAsync();
            if (!validTesk)
                return;
            Close();
        }

        private async Task<bool> rideAsync()
        {
             
            int kilometers = validRide();
            if (kilometers == -1)
                return false;
            myBus.CurrentStatus = MyBus.Status.RIDE;
            await Task.Delay(MainWindow.MY_SECONDS * (kilometers / rand.Next(20, 50)));
            myBus.Kilometers = kilometers;
            myBus.CurrentStatus = MyBus.Status.READY;
            return true;
        }

        private int validRide()
        {
            int kilometers;
            if (!int.TryParse(tbUserKilometerToRide.Text, out kilometers))
            {
                lErrMsg.Content = "Not a valid number";
                return -1;
            }
            if (kilometers < 0)
            {
                lErrMsg.Content = "Not a valid number";
                return -1;
            }
            if (myBus.NeedsTreatment())
            {
                lErrMsg.Content = "This bus need treatment";
                return -1;
            }
            if (myBus.NeedRefueling(kilometers))
            {
                lErrMsg.Content = "This bus need refueing to make this ride";
                return -1;
            }
            return kilometers;
        }
    }
}
