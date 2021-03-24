using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using LadeskabLibrary;
using LadeSkabNUnitTest;
using NSubstitute;

namespace NUnitTestLadeSkab
{
    public class TestChargerControl
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
        public StationControl stationControl;
        public ChargeControl uut;


        [SetUp]
        public void Setup()
        {
            Door = NSubstitute.Substitute.For<IDoor>();
            rfidReader = NSubstitute.Substitute.For<IRfidReader>();
            UsbChargerSimo = NSubstitute.Substitute.For<IUsbCharger>();
            

            uut = new ChargeControl(UsbChargerSimo);
        }

        [Test]
        public void ChargeControl_PhoneIsConnected_StartCharging()
        {
            //arrange
            //fakeChargeControl.IsConnected();

            //act
            //fakeUsb.StartCharge();
            
            //fakeUsb.SimulateConnected(true);
            //fakeUsb.Connected = fakeUsb.StartChargeIsActivated;
            //fakeUsb.StartChargeIsActivated = fakeChargeControl.IsConnected();
            //fakeChargeControl.IsConnected();
            bool var1 = true;
            UsbChargerSimo.SimulateConnected(var1);
            
            var1 = fakeChargeControl.IsConnected();
            
            //assert
            
            //UsbChargerSimo.SimulateConnected(true);
            //fakeUsb.StartChargeIsActivated = true;
            //UsbChargerSimo.Connected = true;

            Assert.That(fakeChargeControl.IsConnected,Is.EqualTo(var1));
            

            //fakeChargeControl.Received(1).StartCharge();
            

        }
        
        
    }
    
}
