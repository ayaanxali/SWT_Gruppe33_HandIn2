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

    }
}
