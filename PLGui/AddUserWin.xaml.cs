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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for AddUserWin.xaml
    /// </summary>
    public partial class AddUserWin : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();

        public AddUserWin()
        {
            InitializeComponent();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            errLabUesrname.Content = errLabPassword.Content = "";
            tbPassword.BorderBrush = tbUsername.BorderBrush = Brushes.Black;
            var newUser = new BO.User();
            try
            {
                if(tbUsername.Text == string.Empty)
                {
                    tbUsername.BorderBrush = Brushes.Red;
                    return;
                }
                if (tbPassword.Password == string.Empty)
                {
                    tbPassword.BorderBrush = Brushes.Red;
                    return;
                }

                newUser.UserName = tbUsername.Text;
                newUser.HashedPassword = myBL.GetHashPassword(tbPassword.Password);
                if (cbIsAdmin.IsChecked.Value)
                    newUser.Admin = true;
                
                myBL.AddUser(newUser);
            }
            catch(BO.BadUsernameException ex)
            {
                errLabUesrname.Content = $"Error: {ex.Message}";
                return;
            }
            Close();
        }
    }
}
