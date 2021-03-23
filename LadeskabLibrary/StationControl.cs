using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


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
        private int _oldId;
        private IDoor _door;
        private IDisplay _display;
        private IRfidReader _rfidReader;
        private bool doorStatus { get; set; }

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor
        public StationControl(IRfidReader RfidReader, IDoor door, IChargeControl charger, IDisplay display)
        {
            _door = door;
            _charger = charger;
            _display = display;
            _rfidReader = RfidReader;

                //this method:HandleRfidDetectedEvent & m.m will be called when new data are ready
            _rfidReader.RfidReaderEvent += HandleRfidDetectedEvent;
            _door.DoorChangedEvent += HandleDoorStatusEvent;
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
                        //if (doorStatus == false) //door is closed
                       // {
                            _door.LockDoor();


                            _charger.StartCharge();
                            _oldId = id;
                            using (var writer = File.AppendText(logFile))
                            {
                                writer.WriteLine(DateTime.Now + ": Skab låst med RFID: {0}", id);
                            }

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
                    //_state = LadeskabState.Available;
                    break;

                case LadeskabState.Locked:
                    // Check for correct ID
                    if (id == _oldId)
                    {
                        _charger.StopCharge();
                        _door.UnlockDoor();
                        using (var writer = File.AppendText(logFile))
                        {
                            writer.WriteLine(DateTime.Now + ": Skab låst op med RFID: {0}", id);
                        }

                        _display.ShowCorrectId();
                        //Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        _display.ShowWrongId();
                        //Console.WriteLine("Forkert RFID tag");
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

        public void DoorStatusChanged()
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
