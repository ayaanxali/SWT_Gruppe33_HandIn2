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

        //[Test]
        //public void LockDoor_AvailableAndConnected_DoorIsLocked()
        //{
        //    int id = 1200;
        //    //chargeControl.IsConnected();
        //    //arrange
        //    //rfidReader.SetRfidTag(id);
        //    FakeUsbCharger usbCharger = new FakeUsbCharger();
        //    usbCharger.Connected = true;
        //    chargeControl.IsConnected();
        //    //stationControl._state = StationControl.LadeskabState.Available;
        //    //stationControl.RfidDetected(id);
            
        //    //FakeChargeControl fakeChargeControl = new FakeChargeControl(fakeUsb);

        //    //uut._state = StationControl.LadeskabState.Available;
        //    //fakeChargeControl.IsConnected();
            
        //    //_recievedDoorStatusEvent.Status = true;
        //    uut.LockDoor();
            
        //    //act
        //    // kald på rfiddetected-metoden, sæt tilstanden til available, charger er true, 

        //    Assert.That(fakeDoor.UnLockDoorIsActivated, Is.EqualTo(true));
            
        //}

    }
}