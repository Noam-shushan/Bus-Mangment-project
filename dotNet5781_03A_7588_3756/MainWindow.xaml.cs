using System.Windows;
using System.Windows.Controls;
using dotNet5781_02_7588_3756;

namespace dotNet5781_03A_7588_3756
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BusLine currentDisplayBusLine;
        BusLineCollection busColl = new BusLineCollection();

        public MainWindow()
        {
            InitializationAndMenu.InitializBusCollection(busColl, 10 ,40);
            InitializeComponent();
            cbBusLines.ItemsSource = busColl;
            cbBusLines.DisplayMemberPath = "BusLineNum";
            cbBusLines.SelectedIndex = 0;


        }
        
        private void ShowBusLine(int index)
        {
            currentDisplayBusLine = busColl[index];
            UpGrid.DataContext = currentDisplayBusLine;
            lbBusLineStations.DataContext = currentDisplayBusLine.Stations;
        }

        private void cbBusLines_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowBusLine((cbBusLines.SelectedValue as BusLine).BusLineNum);
        }
    }
}
