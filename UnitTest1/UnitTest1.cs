using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestHelperConvertToBytesAndBytesToString()
        {
            var str = "Hello";

            var bt = Helper.HelperClass.ConvertStringToBytes(str);

            var st2 = Helper.HelperClass.ConvertBytesToString(bt);
                     Assert.AreEqual(str, st2);
        }

        [TestMethod]
        public void TestCommondBytesAndBytesToCommand()
        {
            var str = DotNetSMPP.CommnadType.bind_transceiver;

            var bt =  DotNetSMPP.Protocol.ProtocolHelper.GetCommondBytes(str) ;

            var st2 = DotNetSMPP.Protocol.ProtocolHelper.GetCommondFromBytes(bt);
         
            Assert.AreEqual(str, st2);
        }
    }
}
