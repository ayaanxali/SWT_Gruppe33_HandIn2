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
        public IDisplay display;
        public IUsbCharger FakeCharger;
        public ChargeControl uut;
       


        [SetUp]
        public void Setup()
        {
            display = NSubstitute.Substitute.For<IDisplay>();
            FakeCharger = NSubstitute.Substitute.For<IUsbCharger>();
            uut = new ChargeControl(FakeCharger,display);
        }

        
        [TestCase(499)]
        [TestCase(500)]
        public void ChargeControl_WhenCurrentIsHigh_StartCharging(double current)
        {
            //arrange
            uut.StartCharge();

            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = current });

            //assert
            FakeCharger.Received(1).StartCharge();
            FakeCharger.Received(1).StartCharge();
        }
        [TestCase(5)]
        [TestCase(1)]
        public void ChargeControl_WhenCurrentIsLow_StopCharging(double var1)
        {
            //arrange
            uut.StopCharge();

            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });
            
            //assert
            FakeCharger.Received(1).StopCharge();
            FakeCharger.Received(1).StopCharge();
        }

        [TestCase(500)]
        [TestCase(499)]
        [TestCase(6)]
        public void ChargeControl_WhenCurrentIsHigh_ShowPhoneIsChargingIsReceivedOne(double current)
        {
            //arrange
            uut.StartCharge();

            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = current });

            //assert
            display.Received(1).ShowStatusPhoneIsCharging();
            display.Received(1).ShowStatusPhoneIsCharging();
            display.Received(1).ShowStatusPhoneIsCharging();
        }
       
        [TestCase(4)]
        [TestCase(5)]
        public void ChargeControl_WhenCurrentIsHigh_ShowPhoneIsChargingDidNotReceived(double current)
        {
            //arrange
            uut.StartCharge();

            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = current });

            //assert
            display.DidNotReceive().ShowStatusPhoneIsCharging();
            display.DidNotReceive().ShowStatusPhoneIsCharging();
        }

        [TestCase(5)]
        [TestCase(1)]
        public void ChargeControl_WhenCurrentIsFiveOrUnder_ShowPhoneIsFullyChargedIsReceivedOne(double current)
        {
            //arrange
            uut.StopCharge();

            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = current });

            //assert
            display.Received(1).ShowStatusPhoneIsFullyCharged();
            display.Received(1).ShowStatusPhoneIsFullyCharged();
        }
        [TestCase(6)]
        [TestCase(0)]
        public void ChargeControl_WhenCurrentIsFiveOrUnder_ShowPhoneIsFullyChargedDidNotReceivedOne(double current)
        {
            //arrange
            uut.StopCharge();

            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = current });

            //assert
            display.DidNotReceive().ShowStatusPhoneIsFullyCharged();
            display.DidNotReceive().ShowStatusPhoneIsFullyCharged();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void IsConnected_MethodReturnsConnectedStatus_IsEqualToSimulatedConnected(bool Connected)
        {
            //act
            FakeCharger.Connected.Returns(Connected);

            uut.IsConnected();

            //assert
            Assert.That(uut.IsConnected, Is.EqualTo(Connected));
        }

        [TestCase(501)]
        public void ChargeControl_WhenCurrentIsOver500_ShowStatusChargingIsOverloadedReceivedOne(double current)
        {
            //arrange
            uut.StartCharge();
            
            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this,new CurrentEventArgs { Current = current });
            
            //assert
            display.Received(1).ShowStatusChargingIsOverloaded();
        }

        [TestCase(500)]
        [TestCase(499)]
        public void ChargeControl_WhenCurrent500orUnder_ShowStatusChargingIsOverLoadedDidNotReceivedOne(double var1)
        {
            //arrange
            uut.StartCharge();

            //act
            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            display.DidNotReceive().ShowStatusChargingIsOverloaded();
            display.DidNotReceive().ShowStatusChargingIsOverloaded();
        }

        [TestCase(501)]
        public void ChargeControl_StartCharge_CurrentValueIsOverloadedCurrent(double var1)
        {
            //act
            uut.StartCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            FakeCharger.Received(1).StopCharge();
        }
        [TestCase(0.0)]
        public void ChargeControl_StopCharge_CurrentValueIs0(double var1)
        {
            //act
            uut.StopCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });
            
            //assert
            Assert.That(FakeCharger.CurrentValue, Is.EqualTo(var1));
        }

        [TestCase(0.0)]
        public void ChargeControl_StopChargeAndConnectionIsFalse_CurrentValueIs0(double var1)
        {
            //act
            uut.StopCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });
            
            //assert
            Assert.That(FakeCharger.CurrentValue, Is.EqualTo(var1));
        }

    }
}
