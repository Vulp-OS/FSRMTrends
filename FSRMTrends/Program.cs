using System;
using System.Linq;
using System.Windows.Forms;


namespace FSRMTrends
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MainForm theApp = new MainForm();
            bool launchGUI = false;
            theApp.setDebugText = "Debug Mode";

            string[] args = Environment.GetCommandLineArgs().Distinct().ToArray();

            foreach (String arg in args)
            {
                if (arg.Equals("/updateQuotas"))
                {
                    if (SQLDataSource.Default.FSRMServerName == "" || SQLDataSource.Default.ConnectionString == "")
                    {
                        throw new Exception("Please populate settings with Server Name and Connection String");
                    }
                    //run fsrm query and add info to sql
                    SQLCommander.updatePathList();
                    SQLCommander.updateSQLDatabase();
                    return;
                } else
                {
                    launchGUI = true;
                }
            }

            if (launchGUI)
            {
                Application.Run(theApp);
            }
        }
    }
}
