using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using BL;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for StationViewInfo.xaml
    /// </summary>
    public partial class StationViewInfo : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<PO.Line> myLineList = new ObservableCollection<PO.Line>();

        public StationViewInfo(PO.Station station)
        {
            InitializeComponent();
            textBoxStationInfo.Text = station.ToString();
            var lineList = myBL.GetAllLinePassByStation(station.Code);
            foreach(var line in lineList)
            {
                myLineList.Add(line.CopyPropertiesToNew(typeof(PO.Line)) as PO.Line);
            }
            lvLinesPassBy.ItemsSource = myLineList;
        }

        private void lvLinesPassBy_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
