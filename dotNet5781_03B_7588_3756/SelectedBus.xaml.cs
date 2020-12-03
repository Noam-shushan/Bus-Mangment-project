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
      

        public SelectedBus(Bus selectedBus)
        {
            InitializeComponent();
            selectedBus.Treatment();
            selectedBus.KilometersAfterFueling = 0;
            selectedBus.ToString();
        }
        
       

       

        private void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("The bus will be treated");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show("The bus will be refuel");
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
            
        }
    }
}
