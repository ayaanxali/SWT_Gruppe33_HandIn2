using System.IO;
using System.Runtime.CompilerServices;
using LadeskabLibrary;
using NUnit.Framework;

namespace NUnitTestLadeSkab
{
    public class TestDisplay
    {
        private Display uut;
        private StringWriter stringWriter;

        [SetUp]
        public void Setup()
        {
            stringWriter = new StringWriter();
            System.Console.SetOut(stringWriter);
            uut = new Display();
        }

        [Test]
        public void Display_ShowConnectPhone_ContainsCorrectString()
        {
            uut.ShowMessageConnectPhone();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Tilslut telefon"));

        }

        [Test]
        public void Display_ShowScanRfid_ContainsCorrectString()
        {
            uut.ShowMessageScanRfid();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Indlæs Rfidtag"));
            
        }
        [Test]
        public void Display_ShowConnectionIsFailed_ContainsCorrectString()
        {
            uut.ShowMessageConnectionIsFailed();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Din telefon er ikke ordentlig tilsluttet. Prøv igen."));

        }
        [Test]
        public void Display_ShowCorrectId_ContainsCorrectString()
        {
            uut.ShowMessageCorrectId();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Korrekt ID. Du kan tage din telefon ud af skabet og lukke døren"));

        }
        [Test]
        public void Display_ShowWrongId_ContainsCorrectString()
        {
            uut.ShowMessageWrongId();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Forkert RFID tag"));

        }
        [Test]
        public void Display_ShowPhoneIsCharging_ContainsCorrectString()
        {
            uut.ShowMessageOccupiedLocker();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Skabet er låst. Brug dit RFID tag til at låse op."));

        }
        [Test]
        public void Display_ShowStatusChargingIsOverloaded_ContainsCorrectString()
        {
            uut.ShowStatusChargingIsOverloaded();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Fejl under opladning. Ladning af telefon er stoppet. Kontakt servicepersonale"));

        }
        [Test]
        public void Display_ShowStatusPhoneIsCharging_ContainsCorrectString()
        {
            uut.ShowStatusPhoneIsCharging();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Telefonen oplades"));

        }
        [Test]
        public void Display_ShowStatusPhoneIsFullyCharged_ContainsCorrectString()
        {
            uut.ShowStatusPhoneIsFullyCharged();
            stringWriter.ToString();

            Assert.That(stringWriter.ToString(), Does.Contain("Telefon er fyldt opladet."));

        }

    }
}