using Sandbox.ModAPI.Ingame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace IngameScript.Mockups
{
    public class MockGridTerminalSystem : IMyGridTerminalSystem, IEnumerable<IMyTerminalBlock>
    {
        public ObservableCollection<IMyTerminalBlock> Blocks { get; } = new ObservableCollection<IMyTerminalBlock>();
        public ObservableCollection<IMyBlockGroup> Groups { get; } = new ObservableCollection<IMyBlockGroup>();

        public void Add(IMyBlockGroup group) => this.Groups.Add(group);

        public void Add(IMyTerminalBlock block) => this.Blocks.Add(block);

        public void GetBlocks(List<IMyTerminalBlock> blocks)
        {
            blocks.Clear();
            blocks.AddRange(this.Blocks);
        }

        public void GetBlockGroups(List<IMyBlockGroup> blockGroups, Func<IMyBlockGroup, bool> collect = null)
        {
            blockGroups?.Clear();
            foreach (var group in this.Groups)
            {
                if (collect?.Invoke(group) ?? true)
                {
                    blockGroups?.Add(group);
                }
            }
        }

        public void GetBlocksOfType<T>(List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null) where T : class
        {
            blocks?.Clear();
            foreach (var block in this.Blocks)
            {
                if (block is T && (collect?.Invoke(block) ?? true))
                {
                    blocks?.Add(block);
                }
            }
        }

        public void GetBlocksOfType<T>(List<T> blocks, Func<T, bool> collect = null) where T : class
        {
            blocks?.Clear();
            foreach (var block in this.Blocks)
            {
                var typedBlock = block as T;
                if (typedBlock != null && (collect?.Invoke(typedBlock) ?? true))
                {
                    blocks?.Add(typedBlock);
                }
            }
        }

        public void SearchBlocksOfName(string name, List<IMyTerminalBlock> blocks, Func<IMyTerminalBlock, bool> collect = null)
        {
            blocks?.Clear();
            foreach (var block in this.Blocks)
            {
                if (block.CustomName != null && block.CustomName.Contains(name, StringComparison.Ordinal) && (collect?.Invoke(block) ?? true))
                {
                    blocks?.Add(block);
                }
            }
        }

        public IMyTerminalBlock GetBlockWithName(string name) => this.Blocks.FirstOrDefault(block => block.CustomName == name);

        public IMyBlockGroup GetBlockGroupWithName(string name) => this.Groups.FirstOrDefault(block => block.Name == name);

        public IMyTerminalBlock GetBlockWithId(long id) => this.Blocks.FirstOrDefault(block => block.EntityId == id);

        public IEnumerator<IMyTerminalBlock> GetEnumerator() => this.Blocks.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}