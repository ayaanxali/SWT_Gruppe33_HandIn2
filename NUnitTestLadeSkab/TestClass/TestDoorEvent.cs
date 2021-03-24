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
        private FakeRfidReader fakeRfidReader;
        private FakeChargeControl fakeChargeControl;
        private FakeDoor fakeDoor;
        private FakeUsbCharger fakeUsbCharger;
        //private Display display;

        private IChargeControl chargeControl;
        private IDisplay display;
        private IRfidReader rfidReader;
        private IUsbCharger usbCharger;
        private IDoor door;

       
        [SetUp]
        public void Setup()
        {
            
            rfidReader = NSubstitute.Substitute.For<IRfidReader>();
            display = NSubstitute.Substitute.For<IDisplay>();
            usbCharger = NSubstitute.Substitute.For<IUsbCharger>();
            fakeChargeControl = new FakeChargeControl(usbCharger);
            door = NSubstitute.Substitute.For<IDoor>();
            

            stationControl = new StationControl(rfidReader,door,fakeChargeControl,display);
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

        [Test]
        public void Door_UnlockDoorMethodIsActivated_DoorIsUnlocked()
        {
            //act
            stationControl._state = StationControl.LadeskabState.Locked;
            fakeChargeControl.StopChargeIsActivated = true;
            uut.SetDoorStatus(true);

            uut.UnlockDoor();

            //assert
            Assert.That(uut.oldStatus, Is.True);
            
        }

        [Test]
        public void Door_LockDoorMethodIsActivated_DoorIsLocked()
        {
            //act
            stationControl._state = StationControl.LadeskabState.Available;
            fakeChargeControl.StartChargeIsActivated = true;
            uut.SetDoorStatus(true);
            uut.LockDoor();

            //assert
            Assert.That(uut.oldStatus, Is.True);

        }

        [Test]
        public void SetDoorStatus_DoorIsUnlocked_LockDoorIsActivated()
        {
            //act
            uut.SetDoorStatus(false);
            uut.LockDoor();

            //assert
            Assert.That(uut.LockDoorIsActivated, Is.EqualTo(true));
            //Assert.That(uut.LockDoorIsActivated, Is.True);
        }
        [Test]
        public void SetDoorStatus_DoorIsLocked_UnlockDoorIsActivated()
        {
            //act
            uut.SetDoorStatus(false);
            uut.UnlockDoor();

            //assert
            Assert.That(uut.UnLockDoorIsActivated, Is.EqualTo(true));
            
        }

        [Test]
        public void Test1()
        {
            //act
            stationControl.RfidDetected(id:1200);
            stationControl._state = StationControl.LadeskabState.Available;
            //fakeChargeControl.StartChargeIsActivated = true;
            fakeChargeControl.StartCharge();
            uut.SetDoorStatus(false);
            uut.LockDoor();

            //assert
            Assert.That(uut.LockDoorIsActivated, Is.True);
            
        }

        [Test]
        public void Test2()
        {
            stationControl.RfidDetected(id: 1200);
            stationControl._state = StationControl.LadeskabState.Locked;
            fakeChargeControl.StopCharge();
            uut.SetDoorStatus(false);
            uut.UnlockDoor();

            Assert.That(uut.UnLockDoorIsActivated, Is.True);

            //fakeDoor.Received(1).LockDoor();
            //door.Received().LockDoor();

        }
    }
}