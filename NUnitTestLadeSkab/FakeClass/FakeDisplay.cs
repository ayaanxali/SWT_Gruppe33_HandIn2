using System;
using LadeskabLibrary;

namespace NUnitTestLadeSkab
{
    class FakeDisplay : IDisplay
    {
        public bool ShowScanIsActivated = false;
        public void ShowConnectPhone()
        {

        }
        public void ShowConnectionIsFailed()
        {
        }
        public void ShowOccupiedLocker()
        {

        }

        public void ShowCorrectId()
        {

        }

        public void ShowWrongId()
        {

        }
        public void ShowScanRfid()
        {
            ShowScanIsActivated = true;
        }

        public void ShowMessage(string text)
        {
            Console.WriteLine(text);
        }
    }
}