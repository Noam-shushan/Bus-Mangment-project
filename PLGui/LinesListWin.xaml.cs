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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for LinesListWin.xaml
    /// </summary>
    public partial class LinesListWin : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        ObservableCollection<BO.Line> myLineList;

        public LinesListWin(ObservableCollection<BO.Line> lineList)
        {
            InitializeComponent();
            myLineList = lineList;
            lvLineList.ItemsSource = myLineList;
        }

        private void lvLineList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var line = lvLineList.SelectedItem as BO.Line;
            new LineViewInfo(line).Show();
        }

        private void btnAddNewLine_Click(object sender, RoutedEventArgs e)
        {
            gridAddNewLine.Visibility = Visibility.Visible;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (isEmptyBoxsOnAddLine())
                return;
            try
            {
                addLine();
            }
            catch (BO.BadLineException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.BadLineStationException ex)
            {
                MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally
            {
                refreshAddGrig();
            }   
        }

        private void addLine()
        {
            int code;
            if(!int.TryParse(tbLineNumber.Text, out code))
            {
                MessageBox.Show("Erorr: not a valid code line", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int firstStation;
            if(!int.TryParse(tbFirstStation.Text, out firstStation))
            {
                MessageBox.Show("Erorr: not a valid station code for the first station", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int lastStation;
            if (!int.TryParse(tbLastStation.Text, out lastStation))
            {
                MessageBox.Show("Erorr: not a valid station code for the last station", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var newLine = new BO.Line()
            {
                Code = code,
                FirstStation = firstStation,
                LastStation = lastStation,
                IsDeleted = false
            };
            int id = myBL.AddLine(newLine);
            myBL.AddLineStation(new BO.LineStation()
            {
                IsDeleted = false,
                LineId = id,
                LineStationIndex = 0,
                Station = newLine.FirstStation,
                NextStation = newLine.LastStation,
                PrevStation = 0
            });
            myBL.AddLineStation(new BO.LineStation()
            {
                IsDeleted = false,
                LineId = id,
                LineStationIndex = 1,
                Station = newLine.LastStation,
                NextStation = -1,
                PrevStation = newLine.FirstStation
            });
            newLine.LineStations = myBL.GetAllLineStationsByLineID(id);
            myLineList.Add(newLine);
            lvLineList.Items.Refresh();
        }

        private void refreshAddGrig()
        {
            gridAddNewLine.Visibility = Visibility.Collapsed;
            tbLineNumber.Text = tbFirstStation.Text = tbLastStation.Text = string.Empty;
            tbLineNumber.BorderBrush = tbFirstStation.BorderBrush = tbLastStation.BorderBrush = Brushes.Black;
        }

        private bool isEmptyBoxsOnAddLine()
        {
            bool isEmptyBox = false;
            if (tbLineNumber.Text == string.Empty)
            {
                tbLineNumber.BorderBrush = Brushes.Red;
                isEmptyBox = true;
            }
            if (tbFirstStation.Text == string.Empty)
            {
                tbFirstStation.BorderBrush = Brushes.Red;
                isEmptyBox = true;
            }
            if (tbLastStation.Text == string.Empty)
            {
                tbLastStation.BorderBrush = Brushes.Red;
                isEmptyBox = true;
            }
            return isEmptyBox;
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            var lineToDel = lvLineList.SelectedItem as BO.Line;
            if(lineToDel == null)
            {
                MessageBox.Show("Please select line to remove", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(MessageBox.Show($"You sure you want to delete line '{lineToDel.Code}'",
                "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    myBL.RemoveLine(lineToDel);
                    myLineList.Remove(lineToDel);
                    lvLineList.Items.Refresh();
                }
                catch (BO.BadLineException ex)
                {
                    MessageBox.Show("Erorr:" + ex.Message, "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
        }
    }
}
