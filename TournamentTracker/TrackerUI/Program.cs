using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;

namespace TrackerUI
{
    // TODO - Delete my server name in App.Config
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialise the database connections.
            TrackerLibrary.GlobalConfig.InitializeConnections(DatabaseType.Sql);

            //Application.Run(new TournamentDashboardForm());
            Application.Run(new CreatePrizeForm());

        }
    }
}
