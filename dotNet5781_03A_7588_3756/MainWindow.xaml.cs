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
using System.Windows.Navigation;
using System.Windows.Shapes;
using dotNet5781_02_7588_3756;

namespace dotNet5781_03A_7588_3756
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            BusLineCollection busColl = new BusLineCollection();
            InitializationAndMenu.InitializBusCollection(busColl, 10 ,40);
            cbBusLines.ItemsSource = busColl;
            cbBusLines.DisplayMemberPath = " BusLineNum ";
            cbBusLines.SelectedIndex = 0;
            //ShowBusLine(……….)
            InitializeComponent();

        }
    }
}
