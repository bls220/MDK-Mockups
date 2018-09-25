using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Game.Components;
using VRage.Game.ModAPI.Ingame;
using VRageMath;

namespace IngameScript.Mockups.Base
{
    public abstract class MockEntity : IMyEntity, INotifyPropertyChanged
    {
        public virtual Vector3D WorldPosition { get; set; }

        public virtual MyEntityComponentContainer Components
        {
            get { throw new NotImplementedException(); }
        }

        public virtual long EntityId { get; set; }

        public virtual string Name { get; set; }

        public virtual string DisplayName { get; set; }

        public virtual bool HasInventory
        {
            get { throw new NotImplementedException(); }
        }

        public virtual int InventoryCount
        {
            get { throw new NotImplementedException(); }
        }

        public virtual BoundingBoxD WorldAABB { get; set; }

        public virtual BoundingBoxD WorldAABBHr { get; set; }

        public virtual MatrixD WorldMatrix { get; set; }

        public virtual BoundingSphereD WorldVolume { get; set; }

        public virtual BoundingSphereD WorldVolumeHr { get; set; }

        public virtual IMyInventory GetInventory()
        {
            throw new NotImplementedException();
        }

        public virtual IMyInventory GetInventory(int index)
        {
            throw new NotImplementedException();
        }

        public virtual Vector3D GetPosition() => WorldPosition;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName] string property = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
}