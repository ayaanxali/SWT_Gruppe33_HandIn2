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
        //public FakeUsbCharger FakeUsbCharger;


        [SetUp]
        public void Setup()
        {
            //Door = NSubstitute.Substitute.For<IDoor>();
            //rfidReader = NSubstitute.Substitute.For<IRfidReader>();
            display = NSubstitute.Substitute.For<IDisplay>();
            //FakeUsbCharger = new FakeUsbCharger();
            FakeCharger = NSubstitute.Substitute.For<IUsbCharger>();
            //uut = new ChargeControl(FakeUsbCharger,display);
            uut = new ChargeControl(FakeCharger,display);
        }

        [TestCase(500)]
        [TestCase(499)]
        public void ChargeControl_PhoneIsConnected_StartCharging(double var1)
        {
            //act
            uut.StartCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            FakeCharger.Received(1).StartCharge();
        }
        [TestCase(5)]
        [TestCase(0)]
        public void ChargeControl_PhoneIsDisconnected_StopCharging(double var1)
        {
            //act
            uut.StopCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });
            
            //assert
            FakeCharger.Received(1).StopCharge();
        }

        [TestCase(500)]
        [TestCase(499)]
        public void ChargeControl_PhoneIsConnected_ShowPhoneIsChargingIsReceivedOne(double var1)
        {
            //act
            uut.StartCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            display.Received(1).ShowStatusPhoneIsCharging();
        }
        
        [TestCase(5)]
        [TestCase(0)]
        public void ChargeControl_PhoneIsDisconnected_ShowPhoneIsFullyChargedIsReceivedOne(double var1)
        {
            //act
            uut.StopCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            display.Received(1).ShowStatusPhoneIsFullyCharged();
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
        public void ChargeControl_FuseAndOverloadIsTrue_ShowStatusChargingIsOverloadedIsReceivedOne(double var1)
        {
            //act
            uut.StartCharge();
            
            FakeCharger.CurrentValueEvent += Raise.EventWith(this,new CurrentEventArgs { Current = var1 });
            
            //assert
            display.Received(1).ShowStatusChargingIsOverloaded();
        }

        [TestCase(500)]
        [TestCase(499)]
        [TestCase(6)]
        public void ChargeControl_FuseAndOverloadIsFalse_ShowStatusPhoneIsChargingIsReceivedOne(double var1)
        {
            //act
            uut.StartCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            display.DidNotReceive().ShowStatusChargingIsOverloaded();
            display.DidNotReceive().ShowStatusChargingIsOverloaded();
            display.Received(1).ShowStatusPhoneIsCharging();
        }

        [TestCase(4)]
        public void ChargeControl_FuseAndOverloadIsFalse_ShowStatusPhoneIsChargingDidNotReceivedOne(double var1)
        {
            //act
            uut.StartCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            display.DidNotReceive().ShowStatusPhoneIsCharging();
        }

        [TestCase(5)]
        [TestCase(4)]
        [TestCase(0)]
        public void ChargeControl_FuseAnd_ShowStatusPhoneIsFullyChargedIsReceivedOne(double var1)
        {
            //act
            uut.StartCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            display.Received(1).ShowStatusPhoneIsFullyCharged();
            display.Received(1).ShowStatusPhoneIsFullyCharged();
            display.Received(1).ShowStatusPhoneIsFullyCharged();
        }

        [TestCase(6)]
        public void ChargeControl_FuseAnd_ShowStatusPhoneIsFullyChargedDidNotReceivedOne(double var1)
        {
            //act
            uut.StartCharge();

            FakeCharger.CurrentValueEvent += Raise.EventWith(this, new CurrentEventArgs { Current = var1 });

            //assert
            display.DidNotReceive().ShowStatusPhoneIsFullyCharged();
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
