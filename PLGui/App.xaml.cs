using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace PLGui
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        BlApi.IBL myBL = BlApi.BlFactory.GetBL();
        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                myBL.SetBusStatusOnClosing();
            }
            finally
            {
                base.OnExit(e);
            }
        }
    }
}
