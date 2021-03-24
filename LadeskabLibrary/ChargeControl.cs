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

       
        public ChargeControl(IUsbCharger chargerSimulator_)
        {
            chargerSimulator = chargerSimulator_;
        }

        public bool IsConnected()
        {
            bool isConnected = chargerSimulator.Connected;

            return isConnected;

        }

        //bool IChargeControl.IsConnected
        //{
        //    get => _isConnected;
        //    set => _isConnected = value;
        //}

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
