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
        private Bus myBus = null;

        public SelectedBus(Bus selectedBus)
        {
            InitializeComponent();
            tbBusInfo.Text = selectedBus.ToString();
            myBus = selectedBus;
        }

        private void TreatmentButton_Click(object sender, RoutedEventArgs e)
        {
            myBus.Treatment();
            MessageBox.Show("The bus will be treated");
        }

        private void FuelingButton_Click(object sender, RoutedEventArgs e)
        {
            myBus.KilometersAfterFueling = 0;
            MessageBox.Show("The bus will be refuel");
        }
    }
}
