using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoUpdaterDotNET;

namespace ThpsSaveManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string UpdateXmlUrl = "https://github.com/rybergy/THPS-SaveManager/releases/latest/download/version.xml";

        public App()
        {
            AutoUpdater.Start(UpdateXmlUrl);

            SaveUtilities.Initialize();
        }
    }
}
