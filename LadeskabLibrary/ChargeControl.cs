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
        private double CurrentNow;
       // public bool ConnectedStatus { get; set; }

        public ChargeControl(IUsbCharger chargerSimulator_, IDisplay display_)
        {
            chargerSimulator = chargerSimulator_;
            display = display_;

            chargerSimulator.CurrentValueEvent += HandleCurrentStatusEvent; 
        }

        public bool IsConnected()
        {
            bool ConnectedStatus = chargerSimulator.Connected;

            return ConnectedStatus;
        }

        public void StartCharge()
        {
            chargerSimulator.StartCharge();
        }

        public void StopCharge()
        {
            chargerSimulator.StopCharge();
        }

        public void Fuse()
        {
            if (CurrentNow >= 501 && CurrentNow <= 750)
            {
                //StopCharge();
                display.ShowStatusChargingIsOverloaded();
            }
            else if (CurrentNow <= 500 && CurrentNow > 5)
            {
                display.ShowStatusPhoneIsCharging();
            }
            else if(0 <= CurrentNow && CurrentNow <= 5)
            {
                display.ShowStatusPhoneIsFullyCharged();
            }
           
                
        }
        private void HandleCurrentStatusEvent(object sender, CurrentEventArgs e)
        {
            CurrentNow = e.Current;
            Fuse();
        }
    }
}
