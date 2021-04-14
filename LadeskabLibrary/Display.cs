using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace LadeskabLibrary
{
    public class Display : IDisplay
    {


        public void ShowMessageConnectPhone()
        {
            Console.WriteLine("Tilslut telefon");
        }

        public void ShowMessageConnectionIsFailed()
        {
            Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }

        public void ShowMessageOccupiedLocker()
        {
            Console.WriteLine("Skabet er låst. Brug dit RFID tag til at låse op.");
        }

        public void ShowMessageCorrectId()
        {
            Console.WriteLine("Korrekt ID. Du kan tage din telefon ud af skabet og lukke døren");
        }

        public void ShowMessageWrongId()
        {
            Console.WriteLine("Forkert RFID tag");
        }

        public void ShowMessageScanRfid()
        {
            Console.WriteLine("Indlæs Rfidtag");
        }

        public void ShowStatusChargingIsOverloaded()
        {
            Console.WriteLine("Fejl under opladning. Ladning af telefon er stoppet. Kontakt servicepersonale");
        }

        public void ShowStatusPhoneIsCharging()
        {
            Console.WriteLine("Telefonen oplades");
        }

        public void ShowStatusPhoneIsFullyCharged()
        {
            Console.WriteLine("Telefon er fyldt opladet.");
        }
    }
}
