using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    public class FakeUsbCharger : IUsbCharger
    {
        // Constants
        private const double MaxCurrent = 500.0; // mA
        private const double FullyChargedCurrent = 2.5; // mA
        private const double OverloadCurrent = 750; // mA
        private const int ChargeTimeMinutes = 20; // minutes
        private const int CurrentTickInterval = 250; // ms

        public event EventHandler<CurrentEventArgs> CurrentValueEvent;
        public double CurrentValue { get; set; }
        public bool Connected { get; set; }
        private bool _overload;
        private bool _charging;

        public FakeUsbCharger()
        {
            CurrentValue = 0.0;
            Connected = true;
            _overload = false;

            //_timer = new System.Timers.Timer();
            //_timer.Enabled = false;
           // _timer.Interval = CurrentTickInterval;
            //_timer.Elapsed += TimerOnElapsed;
        }
        public void SimulateConnected(bool connected)
        {
            Connected = connected;
        }
        public void SimulateOverload(bool overload)
        {
            _overload = overload;
        }
        public void StartCharge()
        {
            Connected = true;
            if (!_charging)
            {
                if (Connected && !_overload)
                {
                    CurrentValue = 500;
                }
                else if (Connected && _overload)
                {
                    CurrentValue = OverloadCurrent;
                }
                else if (!Connected)
                {
                    CurrentValue = 0.0;
                }

                OnNewCurrent();
                //_ticksSinceStart = 0;

                _charging = true;

                //_timer.Start();
            }
        }
        public void StopCharge()
        {
            CurrentValue = 0.0;
            OnNewCurrent();
            _charging = false;
            
        }
        private void OnNewCurrent()
        {
            CurrentValueEvent?.Invoke(this, new CurrentEventArgs() { Current = this.CurrentValue });
        }
    }
}