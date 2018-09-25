using IngameScript.Mockups;
using IngameScript.Views.MainWindow;
using Malware.MDKUtilities;
using System.Windows;

namespace IngameScript
{
    public partial class App : Application
    {
        public MockGridTerminalSystem MockedGrid { get; set; }

        public App()
        {
            // Initialize the MDK utility framework
            MDKUtilityFramework.Load();
        }

        public new void Run()
        {
            // Eventually passing the mocked grid won't be needed.
            base.Run(new MainWindow(MockedGrid));
        }
    }
}
