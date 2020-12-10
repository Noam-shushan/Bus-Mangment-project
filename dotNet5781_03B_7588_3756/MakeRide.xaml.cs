using System;
using System.Collections.Generic;
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

        public MakeRide(MyBus busToRide)
        {
            myBus = busToRide;
            InitializeComponent();
            tbBusInfo.Text = $"Bus: {myBus.LiscenseNumber}";
        }

        private async void tbUserKilometerToRide_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var validTesk = await rideAsync();
                if (!validTesk)
                    return;
                Close();
            }
        }
        

        private async Task<bool> rideAsync()
        {
             
            int kilometers = validRide();
            if (kilometers == -1)
                return false;
            MainWindow mainWin = (MainWindow) Application.Current.MainWindow;
            await mainWin.RideAsync(myBus, kilometers);
            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private int validRide()
        {
            int kilometers = int.Parse(tbUserKilometerToRide.Text);
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

        private void tbUserKilometerToRide_TextChanged(object sender, TextChangedEventArgs e)
        {
            lErrMsg.Content = "";
        }
    }
}
