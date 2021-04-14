using System;
using System.Collections.Generic;
using System.Text;

namespace LadeskabLibrary
{
    public interface IDisplay
    {
        public void ShowMessageConnectPhone();
        public void ShowMessageConnectionIsFailed();
        public void ShowMessageOccupiedLocker();
        public void ShowMessageCorrectId();
        public void ShowMessageWrongId();
        public void ShowMessageScanRfid();
        public void ShowStatusPhoneIsCharging();
        public void ShowStatusChargingIsOverloaded();
        public void ShowStatusPhoneIsFullyCharged();
    }
}
