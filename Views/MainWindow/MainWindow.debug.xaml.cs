using Sandbox.ModAPI.Ingame;
using System.Windows;

namespace IngameScript.Views.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IMyGridTerminalSystem mockedSetup)
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(new UIMockedRun() { GridTerminalSystem = mockedSetup });
        }
    }
}
