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
    /// Interaction logic for LineViewInfo.xaml
    /// </summary>
    public partial class LineViewInfo : Window
    {
        ObservableCollection<PO.LineStation> myLineStationsList = new ObservableCollection<PO.LineStation>();

        public LineViewInfo(PO.Line line)
        {
            InitializeComponent();
            textBlockLineInfo.Text = $"Line number: {line.Code}";
            textBlockArea.Text = $"Area: {line.Area.ToString().ToLower()}";
            foreach(var ls in line.LineStations)
            {
                myLineStationsList.Add(ls.CopyPropertiesToNew(typeof(PO.LineStation)) as PO.LineStation);
            }
            
            lbLineStations.ItemsSource = myLineStationsList;
        }
    }
}
