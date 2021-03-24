using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    class FakeChargeControl : IChargeControl
    {
        private IUsbCharger chargerSimulator;

        private bool _isConnected;
        //public event EventHandler<ChargingEventArg> ChargingEvent;

        public FakeChargeControl(IUsbCharger chargerSimulator_)
        {
            chargerSimulator = chargerSimulator_;
        }
        public bool IsConnected()
        {
            bool isConnected = chargerSimulator.Connected;

            return isConnected;

        }

        bool IChargeControl.IsConnected
        {
            get => _isConnected;
            set => _isConnected = value;
        }

        public void StartCharge()
        {
            chargerSimulator.StartCharge();
        }

        public void StopCharge()
        {
            chargerSimulator.StopCharge();
        }

        //protected virtual void OnCharging(ChargingEventArg e)
        //{
        //    ChargingEvent?.Invoke(this,e);
        //}
    }
}