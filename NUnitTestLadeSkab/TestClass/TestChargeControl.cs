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
        private FakeChargeControl fakeChargeControl;
        

        public IDoor Door;
        public IRfidReader rfidReader;
        public IDisplay display;
        public IUsbCharger FakeCharger;
        public ChargeControl uut;
        public FakeUsbCharger FakeUsbCharger;


        [SetUp]
        public void Setup()
        {
            //Door = NSubstitute.Substitute.For<IDoor>();
            //rfidReader = NSubstitute.Substitute.For<IRfidReader>();
            display = NSubstitute.Substitute.For<IDisplay>();
            FakeUsbCharger = new FakeUsbCharger();
            FakeCharger = NSubstitute.Substitute.For<IUsbCharger>();
            uut = new ChargeControl(FakeUsbCharger,display);
        }

        [Test]
        public void ChargeControl_PhoneIsConnected_StartCharging()
        {
            //arrange
            FakeUsbCharger.SimulateOverload(false);

            //act
            uut.StartCharge(); 

            //assert
            Assert.That(FakeUsbCharger.CurrentValue, Is.EqualTo(500));
        }
        [Test]
        public void ChargeControl_PhoneIsDisconnected_StopCharging()
        {
            FakeUsbCharger.SimulateOverload(false);

            //FakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs() {Current = newCurrent});
            uut.StopCharge();

            //Assert.That(fakeChargeControl.StopChargeIsActivated, Is.True);
            //Assert.That(fakeChargeControl.ConnectedStatus,Is.False);
            Assert.That(FakeUsbCharger.CurrentValue, Is.EqualTo(0.0));
        }

        [Test]
        public void ChargeControl_PhoneIsConnected_ShowPhoneIsChargingIsReceivedOne()
        {
            FakeUsbCharger.SimulateOverload(false);

            uut.StartCharge();

            display.Received(1).ShowStatusPhoneIsCharging();
        }
        [Test]
        public void ChargeControl_PhoneIsDisconnected_ShowPhoneIsFullyChargedIsReceivedOne()
        {
            FakeUsbCharger.SimulateOverload(false);

            uut.StopCharge();

            display.Received(1).ShowStatusPhoneIsFullyCharged();


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
        public void ChargeControl_FuseAndOverloadIsTrue_ShowStatusChargingIsOverloadedIsReceivedOne(double var1)
        {
            //FakeUsbCharger.SimulateConnected(true);
            FakeUsbCharger.SimulateOverload(true);
            uut.StartCharge();
            //FakeUsbCharger.CurrentValueEvent += Raise.EventWith(new CurrentEventArgs {Current = var1});
            //FakeCharger.CurrentValueEvent += Raise.EventWith(this,new CurrentEventArgs { Current = var1 });
            //eventet i faken virker ikke og skal fikses før vi kan komme videre. 

            //Assert.That(FakeUsbCharger.CurrentValue,Is.EqualTo(var1));

            //uut.Fuse();

            //Assert.That(FakeUsbCharger.CurrentValue,Is.EqualTo(var1));

            //FakeCharger.Received(1).StopCharge();

            //display.DidNotReceive().ShowStatusChargingIsOverloaded();
            //display.DidNotReceive().ShowStatusChargingIsOverloaded();
            display.Received(1).ShowStatusChargingIsOverloaded();

        }

        [TestCase(500)]
        [TestCase(499)]
        public void ChargeControl_FuseAndOverloadIsFalse_ShowStatusPhoneIsChargingIsReceivedOne(double var1)
        {
            FakeUsbCharger.SimulateOverload(false);
            uut.StartCharge();
            //FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //display.DidNotReceive().ShowStatusChargingIsOverloaded();
            //display.DidNotReceive().ShowStatusChargingIsOverloaded();
            display.Received(1).ShowStatusPhoneIsCharging();
            display.Received(1).ShowStatusPhoneIsCharging();
            //Assert.That(FakeUsbCharger.CurrentValue,Is.EqualTo(var1));
        }

        [TestCase(500)]
        public void ChargeControl_StartCharge_CurrentValueIs500(double var1)
        {
            uut.StartCharge();
            Assert.That(FakeUsbCharger.CurrentValue,Is.EqualTo(var1));
            Assert.That(FakeUsbCharger.Connected,Is.True);
        }
        [TestCase(750)]
        public void ChargeControl_StartCharge_CurrentValueIsOverloadedCurrent(double var1)
        {
            FakeUsbCharger.SimulateOverload(true);
            uut.StartCharge();
            Assert.That(FakeUsbCharger.CurrentValue, Is.EqualTo(var1));
        }
        [TestCase(0.0)]
        public void ChargeControl_StopCharge_CurrentValueIs0(double var1)
        {
            FakeUsbCharger.SimulateOverload(true);
            uut.StopCharge();
            Assert.That(FakeUsbCharger.CurrentValue, Is.EqualTo(var1));
        }
        [TestCase(0.0)]
        public void ChargeControl_StopChargeAndConnectionIsFalse_CurrentValueIs0(double var1)
        {
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });
            FakeUsbCharger.SimulateConnected(false);
            FakeUsbCharger.SimulateOverload(false);
            Assert.That(FakeCharger.CurrentValue, Is.EqualTo(var1));
            //FakeCharger.Received(1).StartCharge();
        }

    }
}
