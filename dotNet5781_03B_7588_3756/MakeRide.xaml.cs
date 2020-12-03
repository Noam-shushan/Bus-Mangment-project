using System;
using System.Collections.Generic;
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

namespace dotNet5781_03B_7588_3756
{
    /// <summary>
    /// Interaction logic for MakeRide.xaml
    /// </summary>
    public partial class MakeRide : Window
    {
        private Bus myBus = null;

        public MakeRide(Bus busToRide)
        {
            myBus = busToRide;
            InitializeComponent();
        }

        private void ApplyRideButton_Click(object sender, RoutedEventArgs e)
        {     
            int kilometers;
            if (!int.TryParse(tbUserKilometerToRide.Text, out kilometers))
            {
                lErrMsg.Content = "Not a valid number";
                return;
            }
            if(kilometers < 0)
            {
                lErrMsg.Content = "Not a valid number";
                return;
            }
            if (myBus.NeedsTreatment())
            {
                lErrMsg.Content = "This bus need treatment";
                return;
            }
            if(myBus.NeedRefueling())
            {
                lErrMsg.Content = "This bus need refueing to make this ride";
                return;
            }
            myBus.Kilometers = kilometers;
            Close();
        }
    }
}
