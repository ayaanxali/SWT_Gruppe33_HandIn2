using System.Runtime.InteropServices.ComTypes;
using LadeskabLibrary;
using LadeSkabNUnitTest;
using NSubstitute;
using NUnit.Framework;

namespace NUnitTestLadeSkab
{
    class TestDoor
    {
        private Door uut;
        private ChangeDoorStatusEvent _recievedDoorStatusEvent;
        private StationControl stationControl;
        private FakeRfidReader rfidReader;
        private FakeChargeControl chargeControl;
        private FakeDoor fakeDoor;
        private FakeUsbCharger usbCharger;
        private Display display;

       
        [SetUp]
        public void Setup()
        {
            
            _recievedDoorStatusEvent = null;

            uut = new Door();
            uut.SetDoorStatus(true); //door is open

            uut.DoorChangedEvent += (e, args) => { _recievedDoorStatusEvent = args; };
        }

        [TestCase(true)]
        public void SetDoorStatus_NewDoorStatusOpenToClosed_EventFired(bool var1)
        {
            uut.oldStatus = var1;
            uut.SetDoorStatus(false); // door is closed
            Assert.That(_recievedDoorStatusEvent, Is.Not.Null);
        }
        [TestCase(false)]
        public void SetDoorStatus_NewDoorStatusClosedToOpen_EventFired(bool var1)
        {
            uut.oldStatus = var1;
            uut.SetDoorStatus(true); // door is open
            Assert.That(_recievedDoorStatusEvent, Is.Not.Null);
            // Assert.That(_recievedDoorStatusEvent, Is.Null);
        }

        [TestCase(true)]
        public void SetDoorStatus_NewDoorStatusEqualToOldStatus_EventNotFired(bool var1)
        {
            uut.oldStatus = var1;
            uut.SetDoorStatus(true); // door is open
            Assert.That(_recievedDoorStatusEvent, Is.Null);
        }

        [Test]
        public void SetDoorStatus_NewDoorStatus_CorrectNewDoorStatusRecieved()
        {
            uut.SetDoorStatus(false);
            Assert.That(_recievedDoorStatusEvent.Status, Is.EqualTo(false));
        }

        

    }
}