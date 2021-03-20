using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace LadeskabLibrary
{
    public class StationControl
    {
        // Enum med tilstande ("states") svarende til tilstandsdiagrammet for klassen
        private enum LadeskabState
        {
            Available,
            Locked,
            DoorOpen,
            DoorClosed
        };

        // Her mangler flere member variable
        private LadeskabState _state;
        private IChargeControl _charger;
        private int _oldId;
        private IDoor _door;
        private IDisplay display;
        private bool doorStatus { get; set; }

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        // Her mangler constructor
        public StationControl(IRfidReader RfidReader, IDoor door)
        {
            //this method:HandleRfidDetectedEvent will be called when new data are ready
            RfidReader.RfidReaderEvent += HandleRfidDetectedEvent;
            door.DoorChangedEvent += HandleDoorStatusEvent;
        }
        

        private void HandleRfidDetectedEvent(object sender, RfidDetectedEventArgs e)
        {
            RfidDetected(e.Id);
        }

        // Eksempel på event handler for eventet "RFID Detected" fra tilstandsdiagrammet for klassen
        private void RfidDetected(int id)
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

                            display.ShowOccupiedLocker();
                            //Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
                            _state = LadeskabState.Locked;
                        
                    }
                    else
                    {
                        display.ShowConnectionIsFailed();
                        //Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
                    }

                    break;

                case LadeskabState.DoorOpen:
                    // Ignore
                    //display.ShowConnectPhone();
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

                        display.ShowCorrectId();
                        //Console.WriteLine("Tag din telefon ud af skabet og luk døren");
                        _state = LadeskabState.Available;
                    }
                    else
                    {
                        display.ShowWrongId();
                        //Console.WriteLine("Forkert RFID tag");
                    }

                    break;
                //case LadeskabState.DoorClosed:
                  //  DoorClosed();
                   // _state = LadeskabState.Available;
                //break;
            }
        }

        private void DoorOpened()
        {
            display.ShowConnectPhone();
        }

        private void DoorClosed()
        {
            display.ShowScanRfid();
        }

        public void DoorStatusChanged()
        {
            if (doorStatus != false)
            {
                _state = LadeskabState.DoorOpen;
            }
            else
                _state = LadeskabState.DoorOpen;
        }
        // Her mangler de andre trigger handlere
        private void HandleDoorStatusEvent(object sender, ChangeDoorStatusEvent e)
        {
            doorStatus = e.Status;
            DoorStatusChanged();
        }
    }
}
