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

namespace PLGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();

        public MainWindow()
        {
            InitializeComponent();
            tbUsername.Focus();
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            lbErrPassword.Content = lbErrUsername.Content = "";
            tbPassword.BorderBrush = tbUsername.BorderBrush = Brushes.Black;
           _ = new BO.User();
            BO.User user;
            try
            {
                if (tbUsername.Text == string.Empty)
                {
                    tbUsername.BorderBrush = Brushes.Red;
                    return;
                }
                user = myBL.GetUser(tbUsername.Text);
            }
            catch (BO.BadUsernameException ex)
            {
                lbErrUsername.Content = "Error: " + ex.Message;
                return;
            }

            if (tbPassword.Password == string.Empty)
            {
                tbPassword.BorderBrush = Brushes.Red;
                return;
            }
            if(myBL.GetHashPassword(tbPassword.Password) != user.HashedPassword)
            {
                lbErrPassword.Content = "Error: not valid password";
                return;
            }
            
            if (user.Admin)
                new Management().Show();
            else
                new Traveling().Show();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            new AddUserWin().Show();
        }

        private void btnLogIn_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                btnLogIn_Click(sender, e);
            }
        }

        private void tbUsername_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
                tbPassword.Focus();
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnLogIn.Focus();
        }
    }
}
