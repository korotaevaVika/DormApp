using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace DormApplication
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        internal static StandardKernel kernel;

        protected override void OnStartup(StartupEventArgs e)
        {
            kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());
            base.OnStartup(e);
        }
    }
}
