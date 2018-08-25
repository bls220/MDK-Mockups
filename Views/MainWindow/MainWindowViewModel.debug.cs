using System;

namespace IngameScript.Views.MainWindow
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly UIMockedRun _MockedRun;

        public MainWindowViewModel(UIMockedRun mockedRun)
        {
            _MockedRun = mockedRun;
            mockedRun.OnTicked += RunTicked;
            ToggleRunCommand = new BaseCommand(ToggleRun);
        }

        private void ToggleRun()
        {
            _MockedRun.RunTimer.Enabled = !_MockedRun.RunTimer.Enabled;
            TimerToggleText = _MockedRun.RunTimer.Enabled ? "Pause" : "Start";
        }

        public BaseCommand ToggleRunCommand { get; set; }

        private string _timerToggleText = "Start";
        public string TimerToggleText
        {
            get
            {
                return _timerToggleText;
            }
            set
            {
                if (_timerToggleText != value)
                {
                    _timerToggleText = value;
                    RaisePropertyChanged(nameof(TimerToggleText));
                }
            }
        }

        public string EchoText
        {
            get
            {
                return _MockedRun.EchoText;
            }
            set
            {
                if (_MockedRun.EchoText != value)
                {
                    _MockedRun.EchoText = value;
                    RaisePropertyChanged(nameof(EchoText));
                }
            }
        }

        private void RunTicked()
        {
            // Update all properties
            RaisePropertyChanged(nameof(EchoText));
        }
    }
}
