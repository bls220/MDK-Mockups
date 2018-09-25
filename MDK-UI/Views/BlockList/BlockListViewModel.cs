using Sandbox.ModAPI.Ingame;
using System.Collections.ObjectModel;

namespace IngameScript.Views
{
    public class BlockListViewModel : BaseViewModel
    {
        public BlockListViewModel(ObservableCollection<IMyTerminalBlock> blocks)
        {
            this.Blocks = blocks;
        }

        public ObservableCollection<IMyTerminalBlock> Blocks { get; set; }
    }
}