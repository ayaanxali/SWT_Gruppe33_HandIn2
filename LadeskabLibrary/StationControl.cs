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

        // Her mangler flere member variable
        public LadeskabState _state;
        private IChargeControl _charger;
        public int _oldId;
        private IDoor _door;
        private IDisplay _display;
        private IRfidReader _rfidReader;
        private ILogFile _logFile;
        private bool doorStatus { get; set; }
       // public bool charging = false;

        //private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor
        //public StationControl(IRfidReader RfidReader, IDoor door)
        //{
        //    _door = door;
        //    _rfidReader = RfidReader;

        //    //this method:HandleRfidDetectedEvent & m.m will be called when new data are ready
        //    _rfidReader.RfidReaderEvent += HandleRfidDetectedEvent;
        //    _door.DoorChangedEvent += HandleDoorStatusEvent;
        //}


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

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        public void RfidDetected(int id)
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
                        //Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                        _state = LadeskabState.Locked;

                    }
                    else
                    {
                        _display.ShowConnectionIsFailed();
                        //Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    DoorOpened();

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
        //Her mangler de andre trigger handlere
        private void HandleDoorStatusEvent(object sender, ChangeDoorStatusEvent e)
        {
            doorStatus = e.Status;
            DoorStatusChanged();
        }
    }
}
