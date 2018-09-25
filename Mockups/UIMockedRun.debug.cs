using IngameScript.Mockups;
using System.Timers;

namespace IngameScript.Views
{
    public class UIMockedRun : MockedRun
    {
        public delegate void OnTickedEventHandler();
        /// <summary>
        /// Fires after the Run has been stepped forward.
        /// </summary>
        public event OnTickedEventHandler OnTicked;

        private bool firstEcho = true;
        private string _echoText;

        public UIMockedRun()
        {
            this.RunTimer.Elapsed += this.RunTimer_OnElapsed;
        }

        // Arguably should be part of the UI. But this seems more like Business Logic to me.
        private Timer RunTimer { get; set; } = new Timer(16);

        public string EchoText
        {
            get => this._echoText;
            set
            {
                if (this._echoText != value)
                {
                    this._echoText = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        private void RunTimer_OnElapsed(object sender, ElapsedEventArgs e) => this.NextTick();

        public override void Echo(string text)
        {
            if (this.firstEcho)
            {
                this.firstEcho = false;
                this.EchoText = $"{text}\n";
            }
            else
            {
                this.EchoText += $"{text}\n";
            }
        }

        public bool IsRunning => this.RunTimer.Enabled;

        public override bool NextTick(out MockedRunFrame frame)
        {
            this.firstEcho = true;
            bool cont = base.NextTick(out frame);
            OnTicked?.Invoke();
            return cont;
        }

        /// <summary>
        /// Starts continuously running the simulation.
        /// </summary>
        /// <remarks>Simulation Speed is based on <see cref="RunTimer"/> interval</remarks>
        public void Run()
        {
            if (!this.RunTimer.Enabled)
            {
                this.RunTimer.Enabled = true;
                this.RaisePropertyChanged(nameof(this.IsRunning));
            }
        }

        /// <summary>
        /// Stops continously running the simulation.
        /// </summary>
        public void Pause()
        {
            if (this.RunTimer.Enabled)
            {
                this.RunTimer.Enabled = false;
                this.RaisePropertyChanged(nameof(this.IsRunning));
            }
        }
    }
}