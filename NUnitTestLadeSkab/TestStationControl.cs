using System;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;
using LadeSkab;
using LadeskabLibrary;
using LadeSkabNUnitTest;
using NSubstitute;

namespace NUnitTestLadeSkab
{
    public class TestStationControl
    {
        private FakeDoor fakeDoor;
        private FakeRfidReader fakeRfidReader;
        private FakeChargeControl fakeChargeControl;
        private FakeUsbCharger fakeUsb;
       
        public IDoor Door;
        public IRfidReader rfidReader;
        public IDisplay display;
        public IChargeControl ChargeControl;
        public IUsbCharger UsbChargerSimo;
        public StationControl uut;


        [SetUp]
        public void Setup()
        {
            Door = NSubstitute.Substitute.For<IDoor>();
            rfidReader = NSubstitute.Substitute.For<IRfidReader>();
            display = NSubstitute.Substitute.For<IDisplay>();
            UsbChargerSimo = NSubstitute.Substitute.For<IUsbCharger>();

            fakeChargeControl = new FakeChargeControl(UsbChargerSimo);
            uut = new StationControl(rfidReader, Door,ChargeControl,display);


        }

        [Test]
        public void StationControl_AvailableStateAndChargeIsConnected_DoorIsLocked()
        {
            int id = 1200;
            uut._state = StationControl.LadeskabState.Available;
            UsbChargerSimo.Connected = true;

            fakeDoor.DoorChangedEvent += Raise.EventWith(this, new ChangeDoorStatusEvent() {Status = true});


            fakeDoor.Received(1).LockDoor();
            //Door.Received(1).LockDoor();

            //arrange
            //rfidReader.SetRfidTag(id);

            ////FakeChargeControl fakeChargeControl = new FakeChargeControl(fakeUsb);

            //ChargeControl.IsConnected = true;
            ////ChargeControl.IsConnected = true;
            //uut.RfidDetected(id);

            //act
            // kald på rfiddetected-metoden, sæt tilstanden til available, charger er true, 


            //fakeUsb.SimulateConnected(true);
            //rfidReader.SetRfidTag(id);
            //uut.RfidDetected();

            //assert
            // Assert.That(fakeDoor.UnLockDoorIsActivated, Is.EqualTo(true));

            //rfidReader.Received(1).SetRfidTag(id);
            //UsbChargerSimo.Received(1).StartCharge();
        }

        // Test om display-besked kommer, når døren lukkes
        [Test]
        public void StationControl_DoorClosed_DisplayMessageShowRfid()
        {
            //arrange
            uut._state = StationControl.LadeskabState.DoorClosed;
            UsbChargerSimo.Connected = true;

            //act
            fakeDoor.DoorChangedEvent += Raise.EventWith(this, new ChangeDoorStatusEvent() { Status = true });

            //assert
            display.Received(1).ShowScanRfid();
            

        }

        
        // Test om telefonen er sluttet til opladeren - true/false
        // Test om RFID-tag læses
        // Test om telefonen oplades, når IsConnected() == true
        // Test om RFID-tag er forkert 
    }
}