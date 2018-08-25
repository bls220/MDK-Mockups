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

        public UIMockedRun()
        {
            RunTimer.Elapsed += RunTimer_OnElapsed;
        }

        public Timer RunTimer { get; set; } = new Timer(16);
        
        public string EchoText { get; set; }

        private void RunTimer_OnElapsed(object sender, ElapsedEventArgs e)
        {
            this.NextTick();
        }

        public override void Echo(string text)
        {
            if (firstEcho)
            {
                firstEcho = false;
                EchoText = $"{text}\n";
            }
            else
            {
                EchoText += $"{text}\n";
            }
        }

        public override bool NextTick(out MockedRunFrame frame)
        {
            firstEcho = true;
            bool cont = base.NextTick(out frame);
            OnTicked();
            return cont;
        }

    }
}
