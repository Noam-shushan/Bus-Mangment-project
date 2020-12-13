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
    /// Adding a new bus to the company
    /// You can select the start date of an activity from a calendar
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
            lErrMsgDate.Content = "";
            lErrMsgLicenseNumber.Content = "";
            if(dpUserStartActivity.SelectedDate == null)
            {
                lErrMsgDate.Content = "Error: date is missing";
                return;
            }
            startActivity = dpUserStartActivity.SelectedDate.Value.Date; // get the date from the date piker

            if (UserInput.ValidLiscenseNumber(tbUserLicenseNumber.Text.Replace("-", ""), startActivity) &&
                UserInput.FormatValidLiscenseNumber(tbUserLicenseNumber.Text)) // check validtion of the liscense number
            {
                licenseNumber = tbUserLicenseNumber.Text.Replace("-", "");
                if (MyData.CheckExistingLicenseNumber(int.Parse(licenseNumber)))
                {
                    lErrMsgLicenseNumber.Content = "Error: license number already exist";
                    return;
                }
                MyData.UniqueLicenseNumbers.Add(int.Parse(licenseNumber)); // save the new liscense number
            }
            else
            {
                lErrMsgLicenseNumber.Content = "Error: invalid license number";
                return;
            }
            MainWindow mainWin = (MainWindow)Application.Current.MainWindow;
            mainWin.NewBus = new MyBus(licenseNumber, startActivity);
            Close();
        }
    }
}