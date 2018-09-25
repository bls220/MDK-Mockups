using System.ComponentModel;
using IngameScript.Mockups;

namespace IngameScript.Views.MainWindow
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly UIMockedRun _MockedRun;

        public MainWindowViewModel(UIMockedRun mockedRun)
        {
            this._MockedRun = mockedRun;

            // Setup Commands
            this.ToggleSimulationCommand = new BaseCommand(this.ToggleSimulation);
            this.StepSimulationCommand = new BaseCommand(this.StepSimulation, this.CanStepSimulation);

            // Subscribe to changes in Model
            this._MockedRun.PropertyChanged += this.Model_PropertyChanged;

            // Link ViewModels
            this.BlocksList = new BlockListViewModel((this._MockedRun.GridTerminalSystem as MockGridTerminalSystem).Blocks);
        }

        #region Properties

        public bool IsRunning => this._MockedRun.IsRunning;
        public string EchoText => this._MockedRun.EchoText;

        public BlockListViewModel BlocksList { get; set; }

        #endregion Properties

        #region Commands

        public BaseCommand ToggleSimulationCommand { get; private set; }

        private void ToggleSimulation()
        {
            if (this.IsRunning)
            {
                this._MockedRun.Pause();
            }
            else
            {
                this._MockedRun.Run();
            }
        }

        public BaseCommand StepSimulationCommand { get; private set; }
        private bool CanStepSimulation() => !this.IsRunning;
        // TODO: auto step to next run time
        private void StepSimulation() => this._MockedRun.NextTick();

        #endregion Commands

        protected override void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.Model_PropertyChanged(sender, e);
            if(e.PropertyName == nameof(this._MockedRun.IsRunning))
            {
                StepSimulationCommand.RaiseCanExecuteChanged();
            }
        }
    }
}