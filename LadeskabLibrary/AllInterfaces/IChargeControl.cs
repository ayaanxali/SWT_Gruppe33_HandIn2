using System;
using System.Collections.Generic;
using System.Text;

namespace LadeskabLibrary
{
    public interface IChargeControl
    {
        //public bool ConnectedStatus { get; set; }
        public void StartCharge();
        public void StopCharge();
        public bool IsConnected();
    }

}
