using System;
using System.Collections.Generic;
using System.Text;

namespace LadeskabLibrary.AllInterfaces
{
    public interface ILogFile
    {
        public void LockDoorLog(int Id);
        public void UnLockDoorLog(int Id);
    }
}
