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

            fakeChargeControl = new FakeChargeControl(fakeUsb);
            uut = new StationControl(rfidReader, Door, fakeChargeControl,display);
            
            
        }

        [Test]
        public void Test1( )
        {
            uut._state = StationControl.LadeskabState.Available;
            fakeUsb.SimulateConnected(false);
            rfidReader.SetRfidTag(1200);
            //uut.RfidDetected();


            // Assert.That(fakeDoor.UnLockDoorIsActivated, Is.EqualTo(true));
            Door.Received(1).LockDoor();
            //UsbChargerSimo.Received(1).StartCharge();
        }

        // Test om display-besked kommer, når døren lukkes
        [Test]
        public void StationControl_DoorClosed_DisplayMessageShowRfid()
        {
            //arrange
            
            //act

            //assert

        }

        
        // Test om telefonen er sluttet til opladeren - true/false
        // Test om RFID-tag læses
        // Test om telefonen oplades, når IsConnected() == true
        // Test om RFID-tag er forkert 
    }

    class FakeChargeControl : IChargeControl
    {
        private IUsbCharger chargerSimulator;

        public FakeChargeControl(IUsbCharger chargerSimulator_)
        {
            chargerSimulator = chargerSimulator_;
        }
        public bool IsConnected()
        {
            bool isConnected = chargerSimulator.Connected;

            return isConnected;

        }

        public void StartCharge()
        {
            chargerSimulator.StartCharge();
        }

        public void StopCharge()
        {
            chargerSimulator.StopCharge();
        }
    }

    

    
}