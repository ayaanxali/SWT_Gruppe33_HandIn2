using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    class FakeChargeControl : IChargeControl
    {
        private IUsbCharger chargerSimulator;

        private bool _isConnected;

        public bool ConnectedStatus { get; set; } 
        //public event EventHandler<ChargingEventArg> ChargingEvent;
        public bool StartChargeIsActivated;
        public bool StopChargeIsActivated;

        public FakeChargeControl(IUsbCharger chargerSimulator_)
        {
            chargerSimulator = chargerSimulator_;
        }
        public bool IsConnected()
        {
            _isConnected = ConnectedStatus;

            return _isConnected;

        }

        public void StartCharge()
        {
            chargerSimulator.StartCharge();
            StartChargeIsActivated = true; 
        }

        public void StopCharge()
        {
            chargerSimulator.StopCharge();
            StopChargeIsActivated = true;

        }

        //protected virtual void OnCharging(ChargingEventArg e)
        //{
        //    ChargingEvent?.Invoke(this,e);
        //}
    }
}