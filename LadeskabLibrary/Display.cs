using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;

namespace LadeskabLibrary
{
    public class Display : IDisplay
    {


        public void ShowConnectPhone()
        {
            Console.WriteLine("Tilslut telefon");
        }

        public void ShowConnectionIsFailed()
        {
            Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }

        public void ShowOccupiedLocker()
        {
            Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
        }

        public void ShowCorrectId()
        {
            Console.WriteLine("Tag din telefon ud af skabet og luk døren");
        }

        public void ShowWrongId()
        {
            Console.WriteLine("Forkert RFID tag");
        }

        public void ShowScanRfid()
        {
            Console.WriteLine("Indlæs Rfidtag");
        }

    }
}
