using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    public class FakeDoor : IDoor
    {
        public event EventHandler<ChangeDoorStatusEvent> DoorChangedEvent;
        public bool oldStatus;

        public bool LockDoorIsActivated { get; set; }
        public bool UnLockDoorIsActivated { get; set; }


        public void LockDoor()
        {
            //Console.WriteLine("Døren er låst");
            LockDoorIsActivated = true;
        }

        public void UnlockDoor()
        {
            //Console.WriteLine("Døren er åben");
            UnLockDoorIsActivated = true;
        }

        public void SetDoorStatus(bool newstatus)
        {
            if (newstatus != oldStatus)
            {
                DoorStatusChanged(new ChangeDoorStatusEvent { Status = newstatus });
                oldStatus = newstatus;
            }
        }

        protected virtual void DoorStatusChanged(ChangeDoorStatusEvent e)
        {
            DoorChangedEvent?.Invoke(this, e);
        }
    }
}
