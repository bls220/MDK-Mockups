using Malware.MDKUtilities;
using Sandbox.ModAPI.Ingame;
using System;
using System.Timers;
using System.Windows;

namespace IngameScript.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer runTimer = new Timer(16);

        private UIMockedRun MockedRun { get; set; }

        public MainWindow(IMyGridTerminalSystem mockedSetup)
        {
            InitializeComponent();
            // Initialize the MDK utility framework
            MDKUtilityFramework.Load();
            runTimer.Elapsed += runTimer_OnElapsed;
            MockedRun = new UIMockedRun(this) { GridTerminalSystem = mockedSetup };
        }

        private void runTimer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            if (MockedRun == null)
            {
                runTimer.Stop();
                return;
            }

            if (!MockedRun.NextTick())
            {
                runTimer.Stop();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            runTimer.Start();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            runTimer.Stop();
        }
    }
}
