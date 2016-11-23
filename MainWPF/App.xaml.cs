using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MainWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            AppDomain.CurrentDomain.AssemblyLoad += new AssemblyLoadEventHandler(CurrentDomain_AssemblyLoad);
            var x = e.OriginalSource as System.Windows.Controls.Button;
            var y = x.TemplatedParent as Window;
            y.Close();
        }


        void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            if (args.LoadedAssembly.FullName.StartsWith("ManagedInjector"))
                MessageBox.Show("hey you, stop snooping");//and shut down your application.
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
        }
    }
}