using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    public class FakeDisplay : IDisplay
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
            Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
        }

        public void ShowMessageCorrectId()
        {
            Console.WriteLine("Tag din telefon ud af skabet og luk døren");
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
            Console.WriteLine("Der er sket en mulig kortslutning, ladning skal stoppes.");
        }

        public void ShowStatusPhoneIsCharging()
        {
            Console.WriteLine("Telefonen oplades nu");
        }

        public void ShowStatusPhoneIsFullyCharged()
        {
            Console.WriteLine("Der lades ikke. Tjek om opladeren er rigtig forbundet til telefonen.");
        }
    }
}