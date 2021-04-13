using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace LadeskabLibrary
{
    public class ChargeControl : IChargeControl
    {
        private IUsbCharger chargerSimulator;

        private IDisplay display;
       // private bool _isConnected;
       // public bool ConnectedStatus { get; set; }

        public ChargeControl(IUsbCharger chargerSimulator_, IDisplay display_)
        {
            chargerSimulator = chargerSimulator_;
            display = display_;
        }

        public bool IsConnected()
        {
            bool ConnectedStatus = chargerSimulator.Connected;

            return ConnectedStatus;

        }

        public void StartCharge()
        {
            chargerSimulator.StartCharge();
            display.ShowPhoneIsCharging();
        }

        public void StopCharge()
        {
            chargerSimulator.StopCharge();
            display.ShowPhoneIsNotCharging();
        }
    }
}
