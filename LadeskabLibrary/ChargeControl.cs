using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace LadeskabLibrary
{
    public class ChargeControl : IChargeControl
    {
        private IUsbCharger chargerSimulator;

        public ChargeControl(IUsbCharger chargerSimulator_)
        {
            chargerSimulator = chargerSimulator_;
        }

        public bool IsConnected()
        {
            bool isConnected = chargerSimulator.Connected;

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
