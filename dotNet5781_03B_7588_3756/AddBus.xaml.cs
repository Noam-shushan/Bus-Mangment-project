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

        private void applyNewinfoForBus_Click(object sender, RoutedEventArgs e)
        {
            bool flag = true;
            lErrMsgDate.Content = "";
            lErrMsgLicenseNumber.Content = "";
            if (!DateTime.TryParse(tbUserStartActivity.Text, out startActivity))
            {
                lErrMsgDate.Content = "Error: invalid date";
                flag = false;
            }

            if (UserInput.ValidLiscenseNumber(tbUserLicenseNumber.Text.Replace("-", ""), startActivity) && 
                UserInput.FormatValidLiscenseNumber(tbUserLicenseNumber.Text))
            {
                licenseNumber = tbUserLicenseNumber.Text.Replace("-", "");
            }
            else
            {
                lErrMsgLicenseNumber.Content = "Error: invalid license number";
                flag = false;
            }
            if (!flag)
                return;

            MainWindow.NewBus = new MyBus(licenseNumber, startActivity);
            Close();
        }

        private void tbUserStartActivity_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox text = (TextBox)sender;
            text.Text = string.Empty;
            text.GotFocus -= tbUserStartActivity_GotFocus;
        }
    }
}
