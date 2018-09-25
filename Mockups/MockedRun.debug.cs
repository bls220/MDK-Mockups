using IngameScript.Mockups.Blocks;
using Sandbox.ModAPI.Ingame;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace IngameScript.Mockups
{
    /// <summary>
    /// Represents a mocked-up run of one or more scripts
    /// </summary>
    public abstract class MockedRun : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        private ObservableCollection<MockProgrammableBlock> _programmableBlocks = new ObservableCollection<MockProgrammableBlock>();
        private long _tickCount;
        private IMyGridTerminalSystem _gridTerminalSystem;

        protected MockedRun()
        {
            this.MockedProgrammableBlocks = new ReadOnlyObservableCollection<MockProgrammableBlock>(this._programmableBlocks);
        }

        /// <summary>
        /// Determines whether this run has been initialized.
        /// </summary>
        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Gets or sets the grid terminal system to use during this run. This property must be populated.
        /// </summary>
        public virtual IMyGridTerminalSystem GridTerminalSystem
        {
            get => _gridTerminalSystem;
            set
            {
                if (_gridTerminalSystem != value)
                {
                    _gridTerminalSystem = value;
                    RaisePropertyChanged();
                }
            }
        }

        /// <summary>
        /// A list of mocked programmable blocks available in the <see cref="GridTerminalSystem"/>. This collection
        /// is filled out during the initialization of the run.
        /// </summary>
        public ReadOnlyObservableCollection<MockProgrammableBlock> MockedProgrammableBlocks { get; }

        /// <summary>
        /// Echos text onto the scripting console. This is the method used for the script Echo command
        /// </summary>
        /// <param name="text"></param>
        public abstract void Echo(string text);

        /// <summary>
        /// Attempts to retrieve the update type for this block.
        /// </summary>
        /// <param name="ticks">The tick count to get the update type for</param>
        /// <param name="pb"></param>
        /// <param name="updateType">The detected update type</param>
        /// <returns><c>true</c> if this block is scheduled for a later run, <c>false</c> otherwise.</returns>
        protected virtual bool ScheduleProgrammableBlock(long ticks, MockProgrammableBlock pb, out UpdateType updateType)
        {
            updateType = UpdateType.None;
            var runtime = pb.Runtime;
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

            return true;
        }

        /// <summary>
        /// Runs a single tick of this mocked run.
        /// </summary>
        /// <returns><c>true</c> if the run should continue; <c>false</c> otherwise.</returns>
        public bool NextTick()
        {
            return this.NextTick(out MockedRunFrame frame);
        }

        /// <summary>
        /// Runs a single tick of this mocked run.
        /// </summary>
        /// <returns><c>true</c> if the run should continue; <c>false</c> otherwise.</returns>
        public virtual bool NextTick(out MockedRunFrame frame)
        {
            this.EnsureInit();
            var scheduledPBs = 0;
            var runPBs = 0;
            foreach (var pb in this.MockedProgrammableBlocks)
            {
                if (pb.TryGetUpdateTypeFor(this._tickCount, out UpdateType updateType))
                {
                    this.RunProgrammableBlock(pb, null, updateType);
                    runPBs++;
                }
                pb.ToggleOnceFlag();
                if (pb.IsScheduledForLater(this._tickCount))
                {
                    scheduledPBs++;
                }
            }

            frame = new MockedRunFrame(this._tickCount, scheduledPBs > 0, scheduledPBs, runPBs);
            this._tickCount++;
            return scheduledPBs > 0;
        }

        /// <summary>
        /// Makes sure that the Run has been initialized.
        /// </summary>
        protected void EnsureInit()
        {
            if (this.IsInitialized)
            {
                return;
            }

            this.IsInitialized = true;

            Debug.Assert(this.GridTerminalSystem != null, nameof(this.GridTerminalSystem) + " != null");

            this._tickCount = 0;
            this.FindProgrammableBlocks(this._programmableBlocks);
            this.Starting();
            this.InstallPrograms();
        }

        /// <summary>
        /// Runs a programmable block by its name.
        /// </summary>
        /// <param name="programmableBlockName"></param>
        /// <param name="argument"></param>
        /// <param name="updateType"></param>
        public void Trigger(string programmableBlockName, string argument = null, UpdateType updateType = UpdateType.Trigger)
        {
            var pb = this.GridTerminalSystem.GetBlockWithName(programmableBlockName) as MockProgrammableBlock;
            if (pb == null)
            {
                throw new InvalidOperationException($"Cannot find a mocked programmable block named {programmableBlockName}");
            }

            this.EnsureInit();
            this.RunProgrammableBlock(pb, argument, updateType);
        }

        /// <summary>
        /// Runs a programmable block by its entity ID.
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="argument"></param>
        /// <param name="updateType"></param>
        public void Trigger(long entityId, string argument = null, UpdateType updateType = UpdateType.Trigger)
        {
            var pb = this.GridTerminalSystem.GetBlockWithId(entityId) as MockProgrammableBlock;
            if (pb == null)
            {
                throw new InvalidOperationException($"Cannot find a mocked programmable block with the ID {entityId}");
            }

            this.EnsureInit();
            this.RunProgrammableBlock(pb, argument, updateType);
        }

        /// <summary>
        /// Called as the mock is starting. The <see cref="MockedProgrammableBlocks"/> collection has been
        /// filled at this point.
        /// </summary>
        protected virtual void Starting()
        { }

        /// <summary>
        /// Runs the given programmable block
        /// </summary>
        /// <param name="pb"></param>
        /// <param name="argument"></param>
        /// <param name="updateType"></param>
        protected virtual void RunProgrammableBlock(MockProgrammableBlock pb, string argument, UpdateType updateType) => pb.Run(argument, updateType);

        /// <summary>
        /// Runs through all the detected mocked programmable blocks and
        /// installs them.
        /// </summary>
        protected virtual void InstallPrograms()
        {
            foreach (var pb in this.MockedProgrammableBlocks)
            {
                pb.Install(this);
            }
        }

        /// <summary>
        /// Finds all mocked programmable blocks and places them in the given list
        /// </summary>
        /// <param name="programmableBlocks"></param>
        protected virtual void FindProgrammableBlocks(ObservableCollection<MockProgrammableBlock> programmableBlocks)
        {
            this.GridTerminalSystem.GetBlocksOfType<IMyProgrammableBlock>(null as List<IMyTerminalBlock>, pb =>
            {
                var mock = pb as MockProgrammableBlock;
                if (mock != null)
                {
                    programmableBlocks.Add(mock);
                }

                return false;
            });
        }

        /// <summary>
        /// Called after all programmable blocks have been invoked.
        /// </summary>
        /// <param name="scheduledBlocks"></param>
        /// <returns><c>true</c> if the tick is valid and the run should continue; <c>false</c> to stop the run.</returns>
        protected virtual bool OnTick(int scheduledBlocks) => scheduledBlocks > 0;
    }
}