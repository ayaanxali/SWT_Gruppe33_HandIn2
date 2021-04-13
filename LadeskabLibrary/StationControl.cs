using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LadeskabLibrary.AllInterfaces;


namespace LadeskabLibrary
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        public enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen,
            DoorClosed
        };

       
        private IDoor _door;
        private IDisplay _display;
        private IRfidReader _rfidReader;
        private ILogFile _logFile;
        private IChargeControl _charger;
        private LadeskabState _state;
        private int _oldId;

        private bool doorStatus { get; set; }


        public StationControl(IRfidReader RfidReader, IDoor door, IChargeControl charger, IDisplay display, ILogFile logFile)
        {
            _door = door;
            _charger = charger;
            _display = display;
            _rfidReader = RfidReader;
            _logFile = logFile;

            //this method:HandleRfidDetectedEvent & m.m will be called when new data are ready
            _rfidReader.RfidReaderEvent += HandleRfidDetectedEvent;
            _door.DoorChangedEvent += HandleDoorStatusEvent;

            _state = LadeskabState.Available;

        }


        private void HandleRfidDetectedEvent(object sender, RfidDetectedEventArgs e)
        {
            RfidDetected(e.Id);
        }

        private void RfidDetected(int id)
        {
            switch (_state)
            {
                case LadeskabState.Available:
                    // Check for ladeforbindelse
                    if (_charger.IsConnected())
                    {
                        _door.LockDoor();


                        _charger.StartCharge();
                        _oldId = id;

                        _logFile.LockDoorLog(id);

                        _display.ShowOccupiedLocker();
                        _state = LadeskabState.Locked;

                    }
                    else
                    {
                        _display.ShowConnectionIsFailed();
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    DoorOpened();
                    _state = LadeskabState.Available;

                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        
                        _logFile.UnLockDoorLog(id);

                        _display.ShowCorrectId();
                        
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.ShowWrongId();
                        
                    }

                    break;
                case LadeskabState.DoorClosed:
                    DoorClosed();
                   // _state = LadeskabState.Available;
                    
                    break;
            }
        }

        public void DoorOpened()
        {
            _display.ShowConnectPhone();
        }

        public void DoorClosed()
        {
            _display.ShowScanRfid();
        }

        private void DoorStatusChanged()
        {
            if (doorStatus != false)
            {
                _state = LadeskabState.DoorOpen;
            }
            else
                _state = LadeskabState.DoorClosed;
        }
        private void HandleDoorStatusEvent(object sender, ChangeDoorStatusEvent e)
        {
            doorStatus = e.Status;
            DoorStatusChanged();
        }
    }
}
