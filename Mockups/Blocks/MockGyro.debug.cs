using IngameScript.Mockups.Base;
using Sandbox.ModAPI.Ingame;

namespace IngameScript.Mockups.Blocks
{
    public class MockGyro : MockFunctionalBlock, IMyGyro
    {
        private float _gyroPower;
        private bool _gyroOverride;
        private float _yaw;
        private float _pitch;
        private float _roll;

        public virtual float GyroPower
        {
            get => this._gyroPower;
            set
            {
                if (this._gyroPower != value)
                {
                    _gyroPower = value;
                    RaisePropertyChanged();
                }
            }
        }

        public virtual bool GyroOverride
        {
            get => this._gyroOverride;
            set
            {
                if (this._gyroOverride != value)
                {
                    _gyroOverride = value;
                    RaisePropertyChanged();
                }
            }
        }

        public virtual float Yaw
        {
            get => this._yaw;
            set
            {
                if (this._yaw != value)
                {
                    _yaw = value;
                    RaisePropertyChanged();
                }
            }
        }

        public virtual float Pitch
        {
            get => this._pitch;
            set
            {
                if (this._pitch != value)
                {
                    _pitch = value;
                    RaisePropertyChanged();
                }
            }
        }

        public virtual float Roll
        {
            get => this._roll;
            set
            {
                if (this._roll != value)
                {
                    _roll = value;
                    RaisePropertyChanged();
                }
            }
        }
    }
}