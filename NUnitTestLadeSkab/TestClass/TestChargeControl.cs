using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using LadeskabLibrary;
using LadeskabLibrary.AllInterfaces;
using NSubstitute;

namespace NUnitTestLadeSkab
{
    public class TestChargerControl
    {
        private FakeDoor fakeDoor;
        private FakeChargeControl fakeChargeControl;
        

        public IDoor Door;
        public IRfidReader rfidReader;
        public IDisplay display;
        public IChargeControl ChargeControl;
        public IUsbCharger UsbChargerSimo;
        public ILogFile logFile;
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
            stationControl = new StationControl(rfidReader,Door,fakeChargeControl,display,logFile);
            uut = new ChargeControl(UsbChargerSimo);
        }

        [Test]
        public void ChargeControl_PhoneIsConnected_StartCharging()
        {
            int id = 1200;
            stationControl._state = StationControl.LadeskabState.Available;
            fakeChargeControl.StartChargeIsActivated = true;

            stationControl.RfidDetected(id);

            uut.StartCharge();

            Assert.That(fakeChargeControl.StartChargeIsActivated, Is.True);
        }
        [Test]
        public void ChargeControl_PhoneIsDisconnected_StopCharging()
        {
            int id = 1200;
            stationControl._state = StationControl.LadeskabState.Locked;
            stationControl.RfidDetected(id);
            fakeChargeControl.StopChargeIsActivated = true;

            uut.StopCharge();

            Assert.That(fakeChargeControl.StopChargeIsActivated, Is.True);
        }

        [Test]
        public void Test1_ConnectedStatusIsFalse()
        {
            uut.ConnectedStatus = false;
            uut.IsConnected();

            Assert.That(uut.IsConnected, Is.False);

        }
        [Test]
        public void Test1_ConnectedStatusIsTrue()
        {
            uut.ConnectedStatus = true;
            uut.IsConnected();

            Assert.That(uut.IsConnected, Is.True);
        }



    }
    
}
