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
            //arrange
            int id = 1200;

            //act
            uut.LockDoorLog(id);

            //assert
            Assert.That(File.Exists(logFileName));
        }

        [Test]
        public void LogFile_RfidtagIsDetected_FileisUnlogged()
        {
            //arrange
            int id = 1200;

            //act
            uut.UnLockDoorLog(id);

            //assert
            Assert.That(File.Exists(logFileName));
        }

        [Test]
        public void LockDoorLog_WriteLogToFile_FileContainsWhatsWritten()
        {
            int id = 1200;
            File.Delete(logFileName);
            uut.LockDoorLog(id);
            string readId;
            //DateTime Time = new DateTime(2021, 03, 24, 09, 41, 49);
            // 24 - 03 - 2021 09:41:49
            DateTime Time = DateTime.Now;
            using (StreamReader reader = new StreamReader(File.OpenRead(logFileName)))
            {
                //readId = id.ToString();
                readId = reader.ReadLine();
            }
            Assert.That(readId, Does.Contain(Time + ": Skab låst med RFID: " + id));
        }
        //[Test]
        //public void UnLockDoorLog_WriteLogToFile_FileContainsWhatsWritten()
        //{
        //    int id = 1200;
        //    uut.UnLockDoorLog(id);
        //    string readId;
        //    DateTime Time = new DateTime(2021, 03, 24, 09, 41, 49);
        //    // 24 - 03 - 2021 09:41:49
        //    using (StreamReader reader = new StreamReader(File.OpenRead(logFileName)))
        //    {
        //        //readId = id.ToString();
        //        readId = reader.ReadLine();
        //    }
        //    Assert.That(readId, Does.Contain(Time + ": Skab låst med RFID: " + id));
        //}

        [Test]
        public void LogFile_RfidtagIsDetected_CanLog()
        {
            int id = 1200;
            uut.LockDoorLog(id);
            Assert.DoesNotThrow(() => uut.LockDoorLog(id));
        }

    }
}
