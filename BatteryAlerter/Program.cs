using System;
using System.Threading;
using System.Windows.Forms;

namespace BatteryAlert
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static Thread monitorThread;
        static Form1 mainForm;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            mainForm = new Form1();
            Application.Run(mainForm);
        }

        public static void ExitProgram()
        {
            Properties.Settings.Default.Save();
            stopThread();
            Application.Exit();
        }

        public static void startThread()
        {
            MonitorThread oThread = new MonitorThread();
            monitorThread = new Thread(new ThreadStart(oThread.Monitor));
            monitorThread.Start();
        }
        static bool exitThread = false;
        public static void stopThread()
        {
            exitThread = true;
            
        }
        public class MonitorThread
        {

            // This method that will be called when the thread is started
            public void Monitor()
            {
                bool isOnBattery = false;
                while (!exitThread)
                {
                    PowerStatus powerStatus = SystemInformation.PowerStatus;

                    if (powerStatus.PowerLineStatus == PowerLineStatus.Online)
                    {
                        isOnBattery = false;
                    }
                    else if (!isOnBattery)
                    {
                        isOnBattery = true;
                        //Notification
                        if (mainForm != null)
                        {
                            mainForm.sendNotification();
                        }
                    }
                    Thread.Sleep(200);
                }
            }
        };

    }
}
