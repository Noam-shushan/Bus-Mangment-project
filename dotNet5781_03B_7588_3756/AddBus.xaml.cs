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
using dotNet5781_01_7588_3756;

namespace dotNet5781_03B_7588_3756
{
    /// <summary>
    /// Interaction logic for AddBus.xaml
    /// </summary>
    public partial class AddBus : Window
    {
        private string licenseNumber = "";
        private DateTime startActivity;

        public AddBus()
        {
            InitializeComponent();
        }

        //private void tbUserStartActivity_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        //private void tbUserLicenseNumber_TextChanged(object sender, TextChangedEventArgs e)
        //{

        //}

        private void applyNewinfoForBus_Click(object sender, RoutedEventArgs e)
        {
            if (!DateTime.TryParse(tbUserStartActivity.Text, out startActivity))
            {
                MessageBox.Show("Error: invalid date time");
            }
            if (UserInput.validLiscenseNumber(tbUserLicenseNumber.Text, startActivity))
                licenseNumber = tbUserLicenseNumber.Text;
            else
                MessageBox.Show("Error: invalid license number");
            MainWindow.MyBusList.Add(new Bus(licenseNumber, startActivity));
        }
    }
}
