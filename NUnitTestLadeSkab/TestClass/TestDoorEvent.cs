using System.Runtime.InteropServices.ComTypes;
using LadeskabLibrary;
using LadeskabLibrary.AllInterfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace NUnitTestLadeSkab
{
    class TestDoor
    {
        private Door uut;
        private ChangeDoorStatusEvent _recievedDoorStatusEvent = null;
        private StationControl stationControl;
        private FakeChargeControl fakeChargeControl;
        private FakeDoor fakeDoor;
        

        private IDisplay display;
        private IRfidReader rfidReader;
        private IUsbCharger usbCharger;
        private IDoor door;
        private ILogFile logFile;

       
        [SetUp]
        public void Setup()
        {
            
            rfidReader = NSubstitute.Substitute.For<IRfidReader>();
            display = NSubstitute.Substitute.For<IDisplay>();
            usbCharger = NSubstitute.Substitute.For<IUsbCharger>();
            fakeChargeControl = new FakeChargeControl(usbCharger);
            door = NSubstitute.Substitute.For<IDoor>();
            logFile = NSubstitute.Substitute.For<ILogFile>();
            fakeDoor = new FakeDoor();

            stationControl = new StationControl(rfidReader,door,fakeChargeControl,display,logFile);
            

            uut = new Door();
            uut.DoorChangedEvent += (e, args) => { _recievedDoorStatusEvent = args; };

        }

        [TestCase(true)]
        public void SetDoorStatus_NewDoorStatusOpenToClosed_EventFired(bool var1)
        {
            uut.oldStatus = var1;
            uut.SetDoorStatus(false); // door is closed
            Assert.That(_recievedDoorStatusEvent.Status, Is.False);
        }
        [TestCase(false)]
        public void SetDoorStatus_NewDoorStatusClosedToOpen_EventFired(bool var1)
        {
            uut.oldStatus = var1;
            uut.SetDoorStatus(true); // door is open
            Assert.That(_recievedDoorStatusEvent.Status, Is.True);
            
        }

        [Test]
        public void SetDoorStatus_NewDoorStatusEqualToOldStatus_EventNotFired()
        {
            uut.oldStatus = true;
            uut.SetDoorStatus(true); // door is open
            Assert.That(_recievedDoorStatusEvent, Is.Null);
        }

        [Test]
        public void SetDoorStatus_NewDoorStatus_CorrectNewDoorStatusRecieved()
        {
            uut.oldStatus = true;
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
        public void Door_DoorIsLocked_LockDoorIsActivatedIsTrue()
        {
            //act
            stationControl.RfidDetected(id:1200);
            stationControl._state = StationControl.LadeskabState.Available;
            
            fakeChargeControl.StartCharge();
            uut.SetDoorStatus(false);
            
            fakeDoor.LockDoor();

            Assert.That(fakeDoor.LockDoorIsActivated, Is.True);

        }

        [Test]
        public void Door_DoorIsUnlocked_UnlockDoorIsActivated()
        {
            stationControl.RfidDetected(id: 1200);
            stationControl._state = StationControl.LadeskabState.Locked;
            fakeChargeControl.StopCharge();
            uut.SetDoorStatus(false);
            
            fakeDoor.UnlockDoor();

            Assert.That(fakeDoor.UnLockDoorIsActivated, Is.True);


        }
    }
}