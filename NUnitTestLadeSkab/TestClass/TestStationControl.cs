using System;
using System.Runtime.InteropServices.ComTypes;
using NUnit.Framework;
using LadeSkab;
using LadeskabLibrary;
using LadeskabLibrary.AllInterfaces;
using NSubstitute;
using NSubstitute.ReceivedExtensions;
using NuGet.Frameworks;

namespace NUnitTestLadeSkab
{
    public class TestStationControl
    {
        public IDoor Door;
        public IRfidReader rfidReader;
        public IDisplay display;
        public IChargeControl chargeControl;
        public IUsbCharger UsbChargerSimo;
        public ILogFile logFile;
        public StationControl uut;
        

        [SetUp]
        public void Setup()
        {
            Door = NSubstitute.Substitute.For<IDoor>();
            rfidReader = NSubstitute.Substitute.For<IRfidReader>();
            display = NSubstitute.Substitute.For<IDisplay>();
            UsbChargerSimo = NSubstitute.Substitute.For<IUsbCharger>();
            chargeControl = NSubstitute.Substitute.For<IChargeControl>();
            logFile = NSubstitute.Substitute.For<ILogFile>();
            
            uut = new StationControl(rfidReader, Door,chargeControl,display, logFile);
        }

        [Test]
        public void SwitchCaseAvailable_ChargerIsConnected_MethodDoorIsLockedRecieved1()
        {
            //arrange
            chargeControl.IsConnected().Returns(true);

            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            Door.Received(1).LockDoor();
        }
        [Test]
        public void SwitchCaseAvailable_ChargerIsConnected_MethodStartChargeRecieved1()
        {
            //arrange
            chargeControl.IsConnected().Returns(true);

            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            chargeControl.Received(1).IsConnected();
        }
        [Test]
        public void SwitchCaseAvailable_ChargerIsConnected_MethodShowPhoneIsChargingRecievedOne()
        {
            //arrange
            chargeControl.IsConnected().Returns(true);
            
            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs {Id = 1200});
            
            //assert
            display.Received(1).ShowMessageOccupiedLocker();
        }
        [Test]
        public void SwitchCaseAvailable_ChargerIsNotConnected_MethodShowConnectionIsFailedRecievedOne()
        {
            //arrange
            chargeControl.IsConnected().Returns(false);

            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            display.Received(1).ShowMessageConnectionIsFailed(); 
        }

        [Test]
        public void SwitchCaseLocked_IdIsEqualToOldId_StopChargeReceived1()
        {
            //arrange
            chargeControl.IsConnected().Returns(true);

            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            chargeControl.Received(1).StopCharge();
        }

        [Test]
        public void SwitchCaseLocked_IdIsNotEqualToOldId_StopChargeIsActivatedIsFalse()
        {
            //arrange
            chargeControl.IsConnected().Returns(true);

            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1000 });

            //assert
            chargeControl.DidNotReceive().StopCharge();
        }
        [Test]
        public void SwitchCaseLocked_IdIsEqualToOldId_MethodDoorUnLockedRecieved1()
        {
            //arrange
            chargeControl.IsConnected().Returns(true);

            //act
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            Door.Received(1).UnlockDoor();
        }
        [TestCase(1200)]
        public void SwitchCaseLocked_IdIsEqualToOldId_MethodShowCorrectIdRecievedIs1(int id)
        {
            chargeControl.IsConnected().Returns(true);

            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            display.Received(1).ShowMessageCorrectId();
        }
        [Test]
        public void SwitchCaseLocked_IdIsNotEqualToOldId_MethodShowWrongIdRecievedIs1()
        {
            chargeControl.IsConnected().Returns(true);

            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1000 });

            //assert
            display.Received(1).ShowMessageWrongId();
        }
        [Test]
        public void SwitchCaseLocked_SwitchCaseChangedWhenIdIsEqualToOldId_SwitchToAvailable()
        {
            chargeControl.IsConnected().Returns(true);

            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            chargeControl.IsConnected().Returns(false);
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1300 });

            //assert
            display.Received(1).ShowMessageConnectionIsFailed();
        }

        [Test]
        public void SwitchCaseDoorOpen_MethodDoorOpenShowDisplayMessage_ShowConnectPhoneReceived1()
        {
            Door.DoorChangedEvent += Raise.EventWith(new ChangeDoorStatusEvent {Status = true});
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            display.Received(1).ShowMessageConnectPhone();
        }

       
        [Test]
        public void DoorEvent_SetDoorStatusIsFalse_SwitchCaseIsDoorClosed()
        {
            Door.DoorChangedEvent += Raise.EventWith(new ChangeDoorStatusEvent { Status = false });
            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = 1200 });

            //assert
            display.Received(1).ShowMessageScanRfid();
        }

        [TestCase(1200,1200)]
        public void SwicthCaseIsAvailable_LockDoorLogMethod_ReceivedIs1(int logId, int rfidTag)
        {
            chargeControl.IsConnected().Returns(true);

            rfidReader.RfidReaderEvent += Raise.EventWith(new RfidDetectedEventArgs { Id = rfidTag });

            //assert
            logFile.Received(1).LockDoorLog(logId);
        }
    }
}