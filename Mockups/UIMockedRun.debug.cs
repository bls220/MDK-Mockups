using IngameScript.Mockups;
using System;
using System.Windows.Threading;

namespace IngameScript.UI
{
    public class UIMockedRun : MockedRun
    {
        // Possibly placeholder until the Presentation Layer is made.
        private MainWindow Ui { get; set; }

        public UIMockedRun(MainWindow ui)
        {
            this.Ui = ui;
        }

        bool firstEcho = true;
        public override void Echo(string text)
        {
            if (firstEcho)
            {
                firstEcho = false;
                Ui.EchoBlock.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() => Ui.EchoBlock.Text = $"{text}\n"));
            }
            else
            {
                Ui.EchoBlock.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() => Ui.EchoBlock.Text += $"{text}\n"));
            }
        }

        public override bool NextTick(out MockedRunFrame frame)
        {
            firstEcho = true;
            return base.NextTick(out frame);
        }
    }
}
