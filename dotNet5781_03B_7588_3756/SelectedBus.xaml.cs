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
    /// Interaction logic for SelectedBus.xaml
    /// </summary>
    public partial class SelectedBus : Window
    {
        private MyBus myBus = null;

        public SelectedBus(MyBus selectedBus)
        {
            InitializeComponent();
            tbBusInfo.Text = selectedBus.ToString();
            myBus = selectedBus;
        }

        private async void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            await MainWindow.TreatmentAsync(myBus);
            MessageBox.Show($"Bus {myBus.LiscenseNumber} will be treated");
        }

        private async void FuelingButton_Click(object sender, RoutedEventArgs e)
        {
            await MainWindow.FuelingtAsync(myBus);
            MessageBox.Show($"Bus {myBus.LiscenseNumber} will be refueled");
        }
    }
}
