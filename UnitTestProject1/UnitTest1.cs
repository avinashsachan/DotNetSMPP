using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var x = "avinash\n123";
            var x1 = DotNetSMPP.Util.ConvertToByteArray(x);
            var x2 = DotNetSMPP.Util.ConvertToString(x1);
            Assert.AreEqual(x, x2);
        }
    }
}
