using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    class FakeChargeControl : IChargeControl
    {
        private IUsbCharger chargerSimulator;
        private IDisplay display;

        private bool _isConnected;

        //private bool ConnectedStatus { get; set; } 
        //public event EventHandler<ChargingEventArg> ChargingEvent;
        public bool StartChargeIsActivated;
        public bool StopChargeIsActivated;

        public FakeChargeControl(IUsbCharger chargerSimulator_, IDisplay display_)
        {
            chargerSimulator = chargerSimulator_;
            display = display_;
        }
        public bool IsConnected()
        {
            //ConnectedStatus = _isConnected;
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
        //    ChargingEvent?.Invoke(this, e);
        //}
    }
}