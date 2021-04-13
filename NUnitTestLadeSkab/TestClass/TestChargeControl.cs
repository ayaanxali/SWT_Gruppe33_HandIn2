﻿using System;
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
        public FakeUsbCharger FakeUsbCharger;
        public FakeDisplay FakeDisplay;


        [SetUp]
        public void Setup()
        {
            Door = NSubstitute.Substitute.For<IDoor>();
            rfidReader = NSubstitute.Substitute.For<IRfidReader>();
           // UsbChargerSimo = NSubstitute.Substitute.For<IUsbCharger>();
            display = NSubstitute.Substitute.For<IDisplay>();
            FakeUsbCharger = new FakeUsbCharger();
            FakeDisplay = new FakeDisplay();
            fakeChargeControl = new FakeChargeControl(UsbChargerSimo,FakeDisplay);
            stationControl = new StationControl(rfidReader,Door,fakeChargeControl,display,logFile);
            uut = new ChargeControl(FakeUsbCharger,display);
        }

        [Test]
        public void ChargeControl_PhoneIsConnected_StartCharging()
        {
            //int id = 1200;
            //stationControl._state = StationControl.LadeskabState.Available;
            //fakeChargeControl.StartChargeIsActivated = true;
            FakeUsbCharger.SimulateOverload(false);

            //stationControl.RfidDetected(id);

            //FakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() {Current = 0.0});

            uut.StartCharge(); 

            Assert.That(FakeUsbCharger.CurrentValue, Is.EqualTo(500));
            //Assert.That(FakeUsbCharger.Connected,Is.EqualTo(true));
        }
        [Test]
        public void ChargeControl_PhoneIsConnected_ShowPhoneIsChargingIsReceivedOne()
        {
            FakeUsbCharger.SimulateOverload(false);

            uut.StartCharge();

            display.Received(1).ShowPhoneIsCharging();
            
        }

        [Test]
        public void ChargeControl_PhoneIsDisconnected_StopCharging()
        {
            //int id = 1200;
            //stationControl._state = StationControl.LadeskabState.Locked;
            //stationControl.RfidDetected(id);
            FakeUsbCharger.SimulateOverload(false);
            //fakeChargeControl.StopChargeIsActivated = true;
            //double newCurrent = 0.0;

            //FakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() {Current = newCurrent});
            uut.StopCharge();

            //Assert.That(fakeChargeControl.StopChargeIsActivated, Is.True);
            //Assert.That(fakeChargeControl.ConnectedStatus,Is.False);
            Assert.That(FakeUsbCharger.CurrentValue,Is.EqualTo(0.0));
        }
        [Test]
        public void ChargeControl_PhoneIsDisconnected_ShowPhoneIsNotChargingIsReceivedOne()
        {
            FakeUsbCharger.SimulateOverload(false);

            uut.StopCharge();

            display.Received(1).ShowPhoneIsNotCharging();

        }


        [TestCase(true,true)]
        [TestCase(false,false)]
        public void IsConnected_MethodReturnsConnectedStatus_IsEqualToSimulatedConnected(bool Connected, bool Result)
        {
            FakeUsbCharger.SimulateConnected(Connected);
            uut.IsConnected();

            Assert.That(uut.IsConnected, Is.EqualTo(Result));
            Assert.That(uut.IsConnected,Is.EqualTo(Result));

        }
        
    }

    public class FakeDisplay : IDisplay
    {
        public void ShowConnectPhone()
        {
            Console.WriteLine("Tilslut telefon");
        }

        public void ShowConnectionIsFailed()
        {
            Console.WriteLine("Din telefon er ikke ordentlig tilsluttet. Prøv igen.");
        }

        public void ShowOccupiedLocker()
        {
            Console.WriteLine("Skabet er låst og din telefon lades. Brug dit RFID tag til at låse op.");
        }

        public void ShowCorrectId()
        {
            Console.WriteLine("Tag din telefon ud af skabet og luk døren");
        }

        public void ShowWrongId()
        {
            Console.WriteLine("Forkert RFID tag");
        }

        public void ShowScanRfid()
        {
            Console.WriteLine("Indlæs Rfidtag");
        }

        public void ShowChargingIsOverloaded()
        {
            Console.WriteLine("Der er sket en mulig kortslutning, ladning skal stoppes.");
        }

        public void ShowPhoneIsCharging()
        {
            Console.WriteLine("Telefonen oplades nu");
        }

        public void ShowPhoneIsNotCharging()
        {
            Console.WriteLine("Der lades ikke. Tjek om opladeren er rigtig forbundet til telefonen.");
        }
    }
}
