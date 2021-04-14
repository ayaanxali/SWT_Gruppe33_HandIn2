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
           // fakeChargeControl = new FakeChargeControl(UsbChargerSimo,FakeDisplay);
            //stationControl = new StationControl(rfidReader,Door,fakeChargeControl,display,logFile);
            uut = new ChargeControl(FakeUsbCharger,display);
        }

        [Test]
        public void ChargeControl_PhoneIsConnected_StartCharging()
        {
            
            FakeUsbCharger.SimulateOverload(false);

            uut.StartCharge(); 

            Assert.That(FakeUsbCharger.CurrentValue, Is.EqualTo(500));
        }
        [Test]
        public void ChargeControl_PhoneIsConnected_ShowPhoneIsChargingIsReceivedOne()
        {
            FakeUsbCharger.SimulateOverload(false);

            uut.StartCharge();

            display.Received(1).ShowStatusPhoneIsCharging();
        }

        [Test]
        public void ChargeControl_PhoneIsDisconnected_StopCharging()
        {
            FakeUsbCharger.SimulateOverload(false);
          

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

            display.Received(1).ShowStatusChargingIsOverloaded();

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

        [TestCase(501)]
        [TestCase(500)]
        [TestCase(499)]
        public void Fuse_Overload(int var1)
        {
            //FakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() {Current = var1});
            //eventet i faken virker ikke og skal fikses før vi kan komme videre. 

            display.Received(1).ShowStatusChargingIsOverloaded(); 
            display.DidNotReceive().ShowStatusChargingIsOverloaded();
            display.DidNotReceive().ShowStatusChargingIsOverloaded();

        }

        [TestCase(500)]
        public void Fuse_Charging(int var1)
        {

        }
        
    }
}
