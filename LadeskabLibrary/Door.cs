using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace LadeskabLibrary
{
    public class Door : IDoor
    {
        public event EventHandler<ChangeDoorStatusEvent> DoorChangedEvent;
        public bool oldStatus;

        public void LockDoor()
        {
            Console.WriteLine("Døren er låst");
            //DoorStatusChanged(new ChangeDoorStatusEvent{Status = false});
        }

        public void UnlockDoor()
        {
            Console.WriteLine("Døren er åben");
            //DoorStatusChanged(new ChangeDoorStatusEvent { Status = true});
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
            DoorChangedEvent?.Invoke(this,e);
        }
    }
}
