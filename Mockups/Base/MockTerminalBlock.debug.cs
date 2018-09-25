using Sandbox.ModAPI.Ingame;
using Sandbox.ModAPI.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace IngameScript.Mockups.Base
{
    public abstract class MockTerminalBlock : MockCubeBlock, IMyTerminalBlock
    {
        private string _customName;
        private string _detailedInfo;
        private string _customInfo;
        private string _customData;
        private bool _showOnHUD;
        private bool _showInTerminal;
        private bool _showInToolbarConfig;
        private bool _showInInventory;

        public virtual bool HasLocalPlayerAccess() => throw new NotImplementedException();

        public virtual bool HasPlayerAccess(long playerId) => throw new NotImplementedException();

        void IMyTerminalBlock.SetCustomName(string text) => this.CustomName = text;

        void IMyTerminalBlock.SetCustomName(StringBuilder text) => this.CustomName = text.ToString();

        public virtual void GetActions(List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null) => throw new NotImplementedException();

        public virtual void SearchActionsOfName(string name, List<ITerminalAction> resultList, Func<ITerminalAction, bool> collect = null) => throw new NotImplementedException();

        public virtual ITerminalAction GetActionWithName(string name) => throw new NotImplementedException();

        public virtual ITerminalProperty GetProperty(string id) => throw new NotImplementedException();

        public virtual void GetProperties(List<ITerminalProperty> resultList, Func<ITerminalProperty, bool> collect = null) => throw new NotImplementedException();

        public virtual string CustomName
        {
            get => this._customName;
            set
            {
                if (this._customName != value)
                {
                    this._customName = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual string CustomNameWithFaction
        {
            get
            {
                if (String.IsNullOrEmpty(this.GetOwnerFactionTag()))
                {
                    return this.CustomName;
                }

                return $"{this.GetOwnerFactionTag()}.{this.CustomName}";
            }
        }

        public virtual string DetailedInfo
        {
            get => this._detailedInfo;
            set
            {
                if (this._detailedInfo != value)
                {
                    this._detailedInfo = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual string CustomInfo
        {
            get => this._customInfo;
            set
            {
                if (this._customInfo != value)
                {
                    this._customInfo = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual string CustomData
        {
            get => this._customData;
            set
            {
                if (this._customData != value)
                {
                    this._customData = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual bool ShowOnHUD
        {
            get => this._showOnHUD;
            set
            {
                if (this._showOnHUD != value)
                {
                    this._showOnHUD = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual bool ShowInTerminal
        {
            get => this._showInTerminal;
            set
            {
                if (this._showInTerminal != value)
                {
                    this._showInTerminal = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual bool ShowInToolbarConfig
        {
            get => this._showInToolbarConfig;
            set
            {
                if (this._showInToolbarConfig != value)
                {
                    this._showInToolbarConfig = value;
                    this.RaisePropertyChanged();
                }
            }
        }

        public virtual bool ShowInInventory
        {
            get => this._showInInventory;
            set
            {
                if (this._showInInventory != value)
                {
                    this._showInInventory = value;
                    this.RaisePropertyChanged();
                }
            }
        }
    }
}