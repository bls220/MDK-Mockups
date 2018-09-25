using IngameScript.Mockups;
using System.Windows;

namespace IngameScript.Views.MainWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MockGridTerminalSystem mockedSetup)
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(new UIMockedRun() { GridTerminalSystem = mockedSetup });
        }
    }
}
