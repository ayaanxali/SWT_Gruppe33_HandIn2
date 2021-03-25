using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace LadeskabLibrary
{
    public class Door : IDoor
    {
        public event EventHandler<ChangeDoorStatusEvent> DoorChangedEvent;
        public bool oldStatus { get; set; }
        public bool LockDoorIsActivated;
        public bool UnLockDoorIsActivated;

        public void LockDoor()
        {
            Console.WriteLine("Døren er låst");
            //DoorStatusChanged(new ChangeDoorStatusEvent{Status = false});
            //LockDoorIsActivated = true;

        }

        public void UnlockDoor()
        {
            Console.WriteLine("Døren er åben");
            //DoorStatusChanged(new ChangeDoorStatusEvent { Status = true});
            //UnLockDoorIsActivated = true;
        }


        /// <summary>
        /// metoden ser på om døren er åben eller lukket ved brug af en bool. true for åben dør. false for lukket. 
        /// </summary>
        /// <param name="newstatus"></param>
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
