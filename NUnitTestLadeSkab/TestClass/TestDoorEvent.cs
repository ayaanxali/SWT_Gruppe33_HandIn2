using System.IO;
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
        private ChangeDoorStatusEvent _recievedDoorStatusEvent;
        private FakeChargeControl fakeChargeControl;
        private StringWriter stringWriter;
        
        private IDisplay display;
        private IUsbCharger usbCharger;


        [SetUp]
        public void Setup()
        {
            display = NSubstitute.Substitute.For<IDisplay>();
            usbCharger = NSubstitute.Substitute.For<IUsbCharger>();
            fakeChargeControl = new FakeChargeControl(usbCharger,display);
            stringWriter = new StringWriter();

            uut = new Door();
            
            System.Console.SetOut(stringWriter);
            uut.DoorChangedEvent += (e, args) => { _recievedDoorStatusEvent = args; };
        }

        [TestCase(true)]
        public void SetDoorStatus_NewDoorStatusOpenToClosed_EventFired(bool var1)
        {
            //arrange
            uut.oldStatus = var1;

            //act
            uut.SetDoorStatus(false); // door is closed

            //assert
            Assert.That(_recievedDoorStatusEvent.Status, Is.False);
        }

        [TestCase(false)]
        public void SetDoorStatus_NewDoorStatusClosedToOpen_EventFired(bool var1)
        {
            //arrange
            uut.oldStatus = var1;

            //act
            uut.SetDoorStatus(true); // door is open
            
            //assert
            Assert.That(_recievedDoorStatusEvent.Status, Is.True);
        }

        [Test]
        public void Door_UnlockDoorMethodIsActivated_DoorIsUnlocked()
        {
            //arrange
            fakeChargeControl.IsConnected();

            //act
            uut.SetDoorStatus(true);
            uut.UnlockDoor();

            //assert
            Assert.That(uut.oldStatus, Is.True);
        }

        [Test]
        public void Door_LockDoorMethodIsActivated_DoorIsLocked()
        {
            //act
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
            stringWriter.ToString();
            
            //assert
            Assert.That(stringWriter.ToString(), Does.Contain("Døren er låst"));
        }
        [Test]
        public void SetDoorStatus_DoorIsLocked_UnlockDoorIsActivated()
        {
            //act
            uut.UnlockDoor();
            stringWriter.ToString();

            //assert
            Assert.That(stringWriter.ToString(), Does.Contain("Døren er åben"));
        }

        [Test]
        public void Door_DoorIsLocked_LockDoorIsActivatedIsTrue()
        {
            //act
            fakeChargeControl.StartCharge();
            uut.SetDoorStatus(true);
            
            uut.LockDoor();

            //assert
            Assert.That(_recievedDoorStatusEvent.Status,Is.True);
        }

        [Test]
        public void Door_DoorIsUnlocked_UnlockDoorIsActivated()
        {
            //act
            fakeChargeControl.StopCharge();
            uut.SetDoorStatus(true);
            
            uut.UnlockDoor();

            //assert
            Assert.That(_recievedDoorStatusEvent.Status,Is.True);
        }
    }
}