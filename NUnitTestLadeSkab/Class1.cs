using System;
using System.Collections.Generic;
using System.Text;
using LadeskabLibrary;
using LadeSkabNUnitTest;
using NUnit.Framework;

namespace NUnitTestLadeSkab
{
    class TestChargeControl
    {
       private FakeUsbCharger fakeUsbCharger;
       

        [SetUp]
        public void Setup()
        {
            ChargeControl _chargeControl = new ChargeControl(fakeUsbCharger);
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

    }
}
