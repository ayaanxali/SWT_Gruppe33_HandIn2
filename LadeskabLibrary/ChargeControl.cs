using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace LadeskabLibrary
{
    public class ChargeControl : IChargeControl
    {
        private IUsbCharger chargerSimulator;
        private bool _isConnected;
        public bool ConnectedStatus { get; set; }

        public ChargeControl(IUsbCharger chargerSimulator_)
        {
            chargerSimulator = chargerSimulator_;
        }

        public bool IsConnected()
        {
            bool isConnected = ConnectedStatus;

            return isConnected;

        }

        public void StartCharge()
        {
            chargerSimulator.StartCharge();
        }

        public void StopCharge()
        {
            chargerSimulator.StopCharge();
        }
    }
}
