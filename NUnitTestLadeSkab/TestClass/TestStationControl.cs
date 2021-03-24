using System;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;
using LadeSkab;
using LadeskabLibrary;
using LadeSkabNUnitTest;
using NSubstitute;
using NuGet.Frameworks;

namespace NUnitTestLadeSkab
{
    public class TestStationControl
    {
       // private FakeDoor fakeDoor;
        //private FakeRfidReader fakeRfidReader;
        private FakeChargeControl fakeChargeControl;
       // private FakeUsbCharger fakeUsb;

       
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
            ChargeControl = NSubstitute.Substitute.For<IChargeControl>();
            
            fakeChargeControl = new FakeChargeControl(UsbChargerSimo);
            uut = new StationControl(rfidReader, Door,fakeChargeControl,display);
            //var uut2 = new StationControl(rfidReader,fakeDoor,fakeChargeControl,display);
        }

        [Test]
        public void SwitchCaseAvailable_ChargerIsConnected_MethodDoorIsLockedRecieved1()
        {
            int id = 1200;
            uut._state = StationControl.LadeskabState.Available;
            //UsbChargerSimo.Connected = true;
            fakeChargeControl.ConnectedStatus = true;
            
            uut.RfidDetected(id);

            Door.Received(1).LockDoor();
        }
        [Test]
        public void SwitchCaseAvailable_ChargerIsConnected_MethodStartChargeRecieved1()
        {
            int id = 1200;
            uut._state = StationControl.LadeskabState.Available;
            fakeChargeControl.ConnectedStatus = true;

            uut.RfidDetected(id);

            Assert.That(fakeChargeControl.StartChargeIsActivated, Is.True);
            
        }
        [Test]
        public void SwitchCaseAvailable_ChargerIsConnected_MethodShowOccupiedLockerRecievedOne()
        {
            int id = 1200;
            uut._state = StationControl.LadeskabState.Available;
            //UsbChargerSimo.Connected = true;
            fakeChargeControl.ConnectedStatus = true;

            uut.RfidDetected(id);

           // Door.Received(1).LockDoor();
            display.Received(1).ShowOccupiedLocker();
            //Assert.That(fakeChargeControl.StartChargeIsActivated, Is.True);

        }
        [Test]
        public void SwitchCaseAvailable_ChargerIsNotConnected_MethodShowConnectionIsFailedRecievedOne()
        {
            int id = 1200;
            uut._state = StationControl.LadeskabState.Available;
            //UsbChargerSimo.Connected = true;
            fakeChargeControl.ConnectedStatus = false;

            uut.RfidDetected(id);

            display.Received(1).ShowConnectionIsFailed();
        }

        [Test]
        public void SwitchCaseAvailable_ChargerIsConnectedSwitchCaseChanged_SwitchToLocked()
        {
            int id = 1200;
            uut._state = StationControl.LadeskabState.Available;
            //UsbChargerSimo.Connected = true;
            fakeChargeControl.ConnectedStatus = true;

            uut.RfidDetected(id);

            Assert.That(uut._state,Is.EqualTo(uut._state= StationControl.LadeskabState.Locked));
        }

        [TestCase(1200)]
        public void SwitchCaseLocked_IdIsEqualToOldId_StopChargeIsActivatedIsTrue(int Id)
        {
            //arrange
            uut._state = StationControl.LadeskabState.Locked;
            uut._oldId = Id; 

            //act
            uut.RfidDetected(Id);

            //assert
            Assert.That(fakeChargeControl.StopChargeIsActivated, Is.True);

        }
        [TestCase(1200,1000)]
        public void SwitchCaseLocked_IdIsNotEqualToOldId_StopChargeIsActivatedIsFalse(int OldId,int NewId)
        {
            //arrange
            uut._state = StationControl.LadeskabState.Locked;
            uut._oldId = OldId;

            //act
            uut.RfidDetected(NewId);

            //assert
            Assert.That(fakeChargeControl.StopChargeIsActivated, Is.False);

        }
        [TestCase(1200)]
        public void SwitchCaseLocked_IdIsEqualToOldId_MethodDoorUnLockedRecieved1(int id)
        {
            uut._state = StationControl.LadeskabState.Locked;
            uut._oldId = id;   

            uut.RfidDetected(id);

            Door.Received(1).UnlockDoor();
        }
        [TestCase(1200)]
        public void SwitchCaseLocked_IdIsEqualToOldId_MethodShowCorrectIdRecievedIs1(int id)
        {
            uut._state = StationControl.LadeskabState.Locked;
            uut._oldId = id;

            uut.RfidDetected(id);

            display.Received(1).ShowCorrectId();
        }
        [TestCase(1200, 1000)]
        public void SwitchCaseLocked_IdIsNotEqualToOldId_MethodShowWrongIdRecievedIs1(int OldId, int NewId)
        {
            //arrange
            uut._state = StationControl.LadeskabState.Locked;
            uut._oldId = OldId;

            //act
            uut.RfidDetected(NewId);

            //assert
           display.Received(1).ShowWrongId();

        }
        [TestCase(1200)]
        public void SwitchCaseLocked_SwitchCaseChangedWhenIdIsEqualToOldId_SwitchToAvailable(int Id)
        {
            uut._state = StationControl.LadeskabState.Locked;
            uut._oldId = Id;
            
            uut.RfidDetected(Id);

            Assert.That(uut._state, Is.EqualTo(uut._state = StationControl.LadeskabState.Available));
        }

        // Test om display-besked kommer, n�r d�ren lukkes
        [Test]
        public void SwitchCaseDoorOpen_MethodDoorOpenDisplayMessage_ShowConnectPhoneRecievedIs1()
        {
            int id = 1200;
            //arrange
            uut._state = StationControl.LadeskabState.DoorOpen;

            //act
            uut.RfidDetected(id);

            //assert
            display.Received(1).ShowConnectPhone();
        }
        [Test]
        public void SwitchCaseDoorOpen_MethodDoorClosedDisplayMessage_ShowRfidRecievedIs1()
        {
            int id = 1200;
            //arrange
            uut._state = StationControl.LadeskabState.DoorClosed;

            //act
            uut.RfidDetected(id);

            //assert
            display.Received(1).ShowScanRfid();
        }

        [Test]
        public void ChangedDoorStatus_SwitchCaseDoorOpen_Is()
        {

        }

        [Test]
        public void ChangedDoorStatus_SwitchCaseDoorClosed_is()
        {

        }

        [Test]
        public void DoorEvent()
        {
            
        }
    }
}