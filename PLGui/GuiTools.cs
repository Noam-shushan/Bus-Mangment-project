using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PLGui
{
    public static class GuiTools
    {
        public static Management GetCurrentManagmentWin()
        {
            foreach (var win in Application.Current.Windows)
            {
                if (win.GetType().Name == "Management")
                    return win as Management;
            }
            return null;
        }
    }
}
