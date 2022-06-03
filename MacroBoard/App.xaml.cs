using MacroBoard.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace MacroBoard
{
    public partial class App : Application
    {

        string[] args;

        public void app_startup(object sender, StartupEventArgs e)
        {
            this.args = e.Args;
            if (e.Args.Length == 1)
                startUpConsole();
            else
                startUpWindow();
        }


        private void startUpWindow()
        {
            MainWindow mw = new MainWindow();
            mw.Show();
        }


        private void startUpConsole()
        {
            try
            {
                //string workFlowName = args[0];
                // string workFlowPath = AppDomain.CurrentDomain.BaseDirectory + @"\Resources\WFJSON\" + workFlowName + ".json";
                string workFlowPath = args[0];
                WorkFlow workFlow = new Serialization(workFlowPath).Deserialize();
                Executor executor = new Executor(workFlow);
                executor.Execute();
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MainWindow mw = new MainWindow("error occured with input");
                mw.Show();
            }
        }





    }
}
