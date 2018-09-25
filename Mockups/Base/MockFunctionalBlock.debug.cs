using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups.Base
{
    public abstract class MockFunctionalBlock : MockTerminalBlock, IMyFunctionalBlock
    {
        private bool _enabled = true;

        public virtual bool Enabled
        {
            get => _enabled;
            set
            {
                if (this._enabled != value)
                {
                    _enabled = value;
                    RaisePropertyChanged();
                }
            }
        }
        public void RequestEnable(bool enable)
        {
            Enabled = enable;
        }
    }
}