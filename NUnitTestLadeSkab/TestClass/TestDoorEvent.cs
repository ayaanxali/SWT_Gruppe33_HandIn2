﻿using System.IO;
using System.Runtime.InteropServices.ComTypes;
using LadeskabLibrary;
using LadeskabLibrary.AllInterfaces;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal.Execution;

namespace NUnitTestLadeSkab
{
    public class TestDoor
    {
        private Door uut;
        private ChangeDoorStatusEvent _recievedDoorStatusEvent = null;
        private StationControl stationControl;
        private FakeChargeControl fakeChargeControl;
        private StringWriter stringWriter;


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
            door = NSubstitute.Substitute.For<IDoor>();
            logFile = NSubstitute.Substitute.For<ILogFile>();

            fakeChargeControl = new FakeChargeControl(usbCharger,display);
            stationControl = new StationControl(rfidReader,door,fakeChargeControl,display,logFile);
            stringWriter = new StringWriter();
            uut = new Door();
            
            System.Console.SetOut(stringWriter);
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

        //[Test]
        //public void SetDoorStatus_NewDoorStatusEqualToOldStatus_EventNotFired()
        //{
        //    uut.oldStatus = true;

        //    uut.SetDoorStatus(true); // door is open

        //    Assert.That(_recievedDoorStatusEvent, Is.Null);
        //}

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
            fakeChargeControl.StopChargeIsActivated = true;
            uut.SetDoorStatus(true);

            uut.UnlockDoor();

            Assert.That(uut.oldStatus, Is.True);
            
        }

        [Test]
        public void Door_LockDoorMethodIsActivated_DoorIsLocked()
        {
           // stationControl._state = StationControl.LadeskabState.Available;
            fakeChargeControl.StartChargeIsActivated = true;
            uut.SetDoorStatus(true);

            uut.LockDoor();

            Assert.That(uut.oldStatus, Is.True);
        }

        [Test]
        public void SetDoorStatus_DoorIsUnlocked_LockDoorIsActivated()
        {
            
            uut.SetDoorStatus(false);
            
            uut.LockDoor();
            stringWriter.ToString();
            
            //assert
            Assert.That(stringWriter.ToString(), Does.Contain("Døren er låst"));
        }
        [Test]
        public void SetDoorStatus_DoorIsLocked_UnlockDoorIsActivated()
        {
            uut.SetDoorStatus(false);

            uut.UnlockDoor();
            stringWriter.ToString();
            //assert
            Assert.That(stringWriter.ToString(), Does.Contain("Døren er åben"));

        }

        [Test]
        public void Door_DoorIsLocked_LockDoorIsActivatedIsTrue()
        {
            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //stationControl._state = StationControl.LadeskabState.Available;
            
            fakeChargeControl.StartCharge();
            uut.SetDoorStatus(true);
            
            //fakeDoor.LockDoor();
            uut.LockDoor();

            //Assert.That(fakeDoor.LockDoorIsActivated, Is.True);
            Assert.That(_recievedDoorStatusEvent.Status,Is.True);

        }

        [Test]
        public void Door_DoorIsUnlocked_UnlockDoorIsActivated()
        {
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

           // stationControl._state = StationControl.LadeskabState.Locked;

            fakeChargeControl.StopCharge();
            uut.SetDoorStatus(true);
            
            //fakeDoor.UnlockDoor();
            uut.UnlockDoor();

            //Assert.That(fakeDoor.UnLockDoorIsActivated, Is.True);
            Assert.That(_recievedDoorStatusEvent.Status,Is.True);


        }
    }
}