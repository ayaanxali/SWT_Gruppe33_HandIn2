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
            display = NSubstitute.Substitute.For<IDisplay>();
            fakeChargeControl = new FakeChargeControl(UsbChargerSimo);
            stationControl = new StationControl(rfidReader,Door,fakeChargeControl,display);


            uut = new ChargeControl(UsbChargerSimo);
        }

        [Test]
        public void ChargeControl_PhoneIsConnected_StartCharging()
        {
            int id = 1200;
            stationControl.RfidDetected(id);
            stationControl._state = StationControl.LadeskabState.Available;
            fakeChargeControl.StartChargeIsActivated = true;

            uut.StartCharge();

            Assert.That(fakeChargeControl.StartChargeIsActivated, Is.True);


        }
        [Test]
        public void ChargeControl_PhoneIsDisconnected_StopCharging()
        {
            int id = 1200;
            stationControl.RfidDetected(id);
            stationControl._state = StationControl.LadeskabState.Locked;
            fakeChargeControl.StopChargeIsActivated = true;

            uut.StopCharge();

            Assert.That(fakeChargeControl.StopChargeIsActivated, Is.True);


        }




    }
    
}
