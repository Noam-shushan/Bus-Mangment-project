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
            var newUser = new BO.User();
            try
            {
                if(tbUsername.Text == string.Empty)
                {
                    errLabUesrname.Content = "most be filld";
                    return;
                }
                if (tbPassword.Password == string.Empty)
                {
                    errLabPassword.Content = "most be filld";
                    return;
                }

                newUser.UserName = tbUsername.Text;
                newUser.Password = tbPassword.Password;
                if (cbIsAdmin.IsChecked.Value)
                    newUser.Admin = true;
                
                myBL.AddUser(newUser);
            }
            catch(BO.BadUsernameException ex)
            {
                errLabUesrname.Content = $"Erorr: {ex.Message}";
                return;
            }
            Close();
        }
    }
}
