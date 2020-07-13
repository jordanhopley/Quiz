using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Quiz
{
    partial class App : Application
    {

        public Manager Manager { get; set; }

        private void AppStartup(object sender, StartupEventArgs e)
        {
            Manager = new Manager();

            _ = new MainWindow();
        }

    }
}
