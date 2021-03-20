using System;
using System.Collections.Generic;
using System.Text;

namespace LadeskabLibrary
{
    public interface IDoor
    {
        event EventHandler<ChangeDoorStatusEvent> DoorChangedEvent;
        
        public void LockDoor();
        public void UnlockDoor();
        public void SetDoorStatus(bool newstatus);
    }
    public class ChangeDoorStatusEvent : EventArgs
    {
        public bool Status { get; set; }
    }
}
