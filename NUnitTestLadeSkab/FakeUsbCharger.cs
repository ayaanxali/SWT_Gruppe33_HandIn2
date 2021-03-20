using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    class FakeUsbCharger : IUsbCharger
    {
        // Event triggered on new current value
       public event EventHandler<CurrentEventArgs> CurrentValueEvent;

        // Direct access to the current current value
        public double CurrentValue { get; }

        // Require connection status of the phone
       public bool Connected { get; }

        // Start charging
        public void StartCharge()
        {
        }
        // Stop charging
        public void StopCharge()
        {
        }
    }
}