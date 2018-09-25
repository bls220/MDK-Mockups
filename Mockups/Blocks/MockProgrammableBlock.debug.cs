using IngameScript.Mockups.Base;
using Malware.MDKUtilities;
using Sandbox.ModAPI;
using Sandbox.ModAPI.Ingame;
using System;
using System.Diagnostics;
using IMyProgrammableBlock = Sandbox.ModAPI.Ingame.IMyProgrammableBlock;

namespace IngameScript.Mockups.Blocks
{
    public class MockProgrammableBlock : MockFunctionalBlock, IMyProgrammableBlock
    {
        private string _storage = String.Empty;

        public virtual Type ProgramType { get; set; }

        public virtual IMyGridProgram Program { get; set; }

        public virtual IMyGridProgramRuntimeInfo Runtime { get; set; } = new MockGridProgramRuntimeInfo();

        public virtual string Storage
        {
            get => this._storage;
            set
            {
                if ((this._storage ?? String.Empty) != (value ?? String.Empty))
                {
                    this._storage = value ?? String.Empty;
                    RaisePropertyChanged();
                }
            }
        }

        public virtual bool IsRunning { get; set; }

        public virtual string TerminalRunArgument { get; set; }

        public virtual bool Run(string argument, UpdateType updateType)
        {
            if (!this.Enabled)
            {
                return false;
            }

            if (this.Program == null)
            {
                return false;
            }

            if (this.IsRunning)
            {
                return false;
            }

            try
            {
                this.IsRunning = true;
                MDKFactory.Run(this.Program, argument ?? "", updateType: updateType);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                this.IsRunning = false;
            }
        }

        public virtual bool TryRun(string argument) => this.Run(argument, UpdateType.Script);

        /// <summary>
        /// Installs the program for the given mocked run. Any programmable block may only be
        /// a part of a single mocked run or behavior is undefined.
        /// </summary>
        /// <param name="mockedRun"></param>
        public virtual void Install(MockedRun mockedRun)
        {
            if (this.Program != null)
            {
                return;
            }

            if (this.ProgramType == null)
            {
                return;
            }

            Debug.Assert(this.Runtime != null, $"{nameof(this.Runtime)} != null");
            var config = new MDKFactory.ProgramConfig
            {
                GridTerminalSystem = mockedRun.GridTerminalSystem,
                Runtime = Runtime,
                ProgrammableBlock = this,
                Echo = mockedRun.Echo,
                Storage = Storage
            };
            this.Program = MDKFactory.CreateProgram(this.ProgramType, config);
        }

        /// <summary>
        /// Attempts to retrieve the update type for this block.
        /// </summary>
        /// <param name="ticks">The tick count to get the update type for</param>
        /// <param name="updateType">The detected update type</param>
        /// <returns><c>true</c> if this block is scheduled for a later run, <c>false</c> if the block is effectively halted.</returns>
        public virtual bool TryGetUpdateTypeFor(long ticks, out UpdateType updateType)
        {
            updateType = UpdateType.None;
            var runtime = this.Runtime;
            if (runtime == null || runtime.UpdateFrequency == UpdateFrequency.None)
            {
                return false;
            }

            if ((runtime.UpdateFrequency & UpdateFrequency.Once) != 0)
            {
                updateType |= UpdateType.Once;
                runtime.UpdateFrequency &= ~UpdateFrequency.Once;
                if (runtime.UpdateFrequency == UpdateFrequency.None)
                {
                    return false;
                }
            }

            if ((runtime.UpdateFrequency & UpdateFrequency.Update1) != 0)
            {
                updateType |= UpdateType.Update1;
            }

            if ((runtime.UpdateFrequency & UpdateFrequency.Update10) != 0 && ticks % 10 == 0)
            {
                updateType |= UpdateType.Update10;
            }

            if ((runtime.UpdateFrequency & UpdateFrequency.Update100) != 0 && ticks % 100 == 0)
            {
                updateType |= UpdateType.Update100;
            }

            return updateType != UpdateType.None;
        }

        /// <summary>
        /// Determines if this programmable block is scheduled to be run at a later stage.
        /// </summary>
        /// <param name="ticks"></param>
        /// <returns></returns>
        public virtual bool IsScheduledForLater(long ticks)
        {
            var runtime = this.Runtime;
            if (runtime == null)
            {
                return false;
            }

            var freq = runtime.UpdateFrequency;
            if (freq == UpdateFrequency.None)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Removes the Once flag from this programmable block.
        /// </summary>
        public virtual void ToggleOnceFlag()
        {
            var runtime = this.Runtime;
            if (runtime == null)
            {
                return;
            }

            runtime.UpdateFrequency &= ~UpdateFrequency.Once;
        }
    }
}