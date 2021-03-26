using System;
using System.IO;
using LadeskabLibrary.AllInterfaces;

namespace LadeskabLibrary
{
    public class LogFile : ILogFile
    {

        private string logFile = "logfile.txt"; // Navnet på systemets log-fil

        public void LockDoorLog(int Id)
        {
            var writer = File.AppendText(logFile);

            writer.WriteLine(DateTime.Today + ": Skab låst med RFID: {0}", Id);

            writer.Close();
            
        }

        public void UnLockDoorLog(int Id)
        {
            var writer = File.AppendText(logFile);

            writer.WriteLine(DateTime.Today + ": Skab låst op med RFID: {0}", Id);

            writer.Close();
        }
    }
}
