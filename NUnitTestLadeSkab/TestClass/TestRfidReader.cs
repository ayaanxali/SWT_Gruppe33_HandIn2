using LadeskabLibrary;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Serialization;
using NUnit.Framework;

namespace NUnitTestLadeSkab
{
    class TestRfidReaderEvent
    {
        private RfidReader uut;
        private RfidDetectedEventArgs _recievedRfidEventArgs;

        [SetUp]
        public void Setup()
        {
            _recievedRfidEventArgs = null;

            uut = new RfidReader();
            //uut.SetRfidTag(1200);

            uut.RfidReaderEvent += (e, args) => { _recievedRfidEventArgs = args; }; 
        }

        [Test]
        public void SetRfidTag_NewRfidDetected_EventFired()
        {
            uut.SetRfidTag(1200); 
            Assert.That(_recievedRfidEventArgs,Is.Not.Null);
        }

        [Test]
        public void SetRfidTag_RfidTagSetToNewValue_CorrectNewRefidTagRecieved()
        {
            uut.SetRfidTag(1200);
            Assert.That(_recievedRfidEventArgs.Id,Is.EqualTo(1200));
        }
    }
}