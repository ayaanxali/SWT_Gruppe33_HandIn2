using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using LadeskabLibrary;
using LadeskabLibrary.AllInterfaces;
using NUnit.Framework;

namespace NUnitTestLadeSkab
{
    public class TestLogFile
    {
        private LogFile uut;
        private string logFileName = "logfile.txt";

        [SetUp]
        public void SetUp()
        {
            uut = new LogFile();
        }


        [Test]
        public void LogFile_RfidtagIsDetected_FileisLogged()
        {
            int id = 1200;
            uut.LockDoorLog(id);

            Assert.That(File.Exists(logFileName));
        }

        [Test]
        public void LogFile_RfidtagIsDetected_FileisUnlogged()
        {
            int id = 1200;

            uut.UnLockDoorLog(id);

            Assert.That(File.Exists(logFileName));
        }

        [Test]
        public void LogFile_RfidtagIsDetected_FileIsWritten()
        {
            int id = 1200;
            uut.LockDoorLog(id);
            string readId;
            DateTime time = DateTime.Today;

            using (StreamReader reader = new StreamReader(File.OpenRead(logFileName)))
            {
                //readId = id.ToString();
                readId = reader.ReadLine();
            }
            //Assert.That(stringWriter.ToString(), Does.Contain("Tilslut telefon"));
            Assert.That(readId,Does.Contain(time + ": Skab låst med RFID:" + id));
        }

        [Test]
        public void LogFile_RfidtagIsDetected_CanLog()
        {
            int id = 1200;
            uut.LockDoorLog(id);
            Assert.DoesNotThrow(() => uut.LockDoorLog(id));
        }

    }
}
