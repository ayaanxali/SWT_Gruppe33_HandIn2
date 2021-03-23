using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    class FakeUsbCharger : IUsbCharger
    {
        // Event triggered on new current value
       public event EventHandler<CurrentEventArgs> CurrentValueEvent;
       public bool StartChargeIsActivated { get; set; }
       public bool StopChargeIsActivated { get; set; }

        // Direct access to the current current value
        public double CurrentValue { get; private set; }

        // Require connection status of the phone
       public bool Connected { get; private set; }

       public void SimulateConnected(bool connected)
       {
           Connected = connected;
       }

       // Start charging
        public void StartCharge()
        {
            StartChargeIsActivated = true;
        }
        // Stop charging
        public void StopCharge()
        {
            StopChargeIsActivated = true;
        }
    }
}