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
        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
            lbErrPassword.Content = lbErrUsername.Content = "";
            _ = new BO.User();
            BO.User user;
            try
            {
                if (tbUsername.Text == string.Empty)
                {
                    lbErrUsername.Content = "Error: most be filld";
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
                lbErrPassword.Content = "Error: most be filld";
                return;
            }
            if(tbPassword.Password != user.Password)
            {
                lbErrPassword.Content = "Error: not valid password";
                return;
            }
            
            if (user.Admin)
                new Management().Show();
            else
                new Traveling().Show();
        }
    }
}
