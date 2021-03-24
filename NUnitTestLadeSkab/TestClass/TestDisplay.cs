using System.Runtime.CompilerServices;
using LadeskabLibrary;
using NUnit.Framework;

namespace NUnitTestLadeSkab
{
    class TestDisplay
    {
        private FakeDisplay display;
        private StationControl stationC;

        private IUsbCharger usbCharger;
        private IRfidReader rfidReader;
        private IChargeControl chargeControl;
        private IDoor Door;

        [SetUp]
        public void Setup()
        {
            chargeControl = new ChargeControl(usbCharger);
            display = new FakeDisplay();   
            stationC = new StationControl(rfidReader,Door,chargeControl,display);
        }

        [Test]
        public void test1()
        {
            string myString ="Hallo";
            display.ShowMessage(myString);

           // Assert.That(myString, Is.EqualTo("Hello"));
            Assert.AreEqual("Tilslut telefon", "Hallo");

        }

        [Test]
        public void test2()
        {
            //stationC.DoorClosed();

            display.ShowConnectPhone();

            
            Assert.That(display.ShowScanIsActivated, Is.True);
        }

    }
}