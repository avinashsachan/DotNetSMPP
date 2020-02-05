using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                //TestTx();
                //TestRx();
                //TestTxRx();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }



        //public static SMPPClient smsc = new DotNetSMPP.SMPPClient(FromConfig.Server, FromConfig.Port, FromConfig.Username,
        //    FromConfig.Password, FromConfig.SystemType, DotNetSMPP.BindMode.bind_transceiver);


        //private static void TestTx()
        //{
        //    var mno = 1234567890;
        //    var txt = "Hello";


        //    var resp = smsc.Bind();


        //    Console.WriteLine("SMPP Bind Request Result = {0}", resp);

        //    if (resp)
        //    {
        //        //here now create a submit sm
        //        var sourceAddr = "1234";
        //        var distinationAddr = "5678";
        //        var msg = "abcd";
                

                
        //           var result = smsc.DataSm(sourceAddr, distinationAddr,msg);
        //           Console.WriteLine("Submit_SM Result = {0}", result);
                
        //        var o = 0;
        //        while (o < 60)
        //        {
        //            o++;

        //            System.Threading.Thread.Sleep(2 * 1000);
        //        }


        //        resp = smsc.UnBind();
        //        Console.WriteLine("SMPP Unbind Request Result = {0}", resp);
        //    }



        //    ///Console.WriteLine(resp);

        //    //System.Threading.Thread.Sleep(1000 * 300);
        //    Console.WriteLine("");

        //}




        private static void TestTxRx()
        {
            throw new NotImplementedException();
        }

        private static void TestRx()
        {
            throw new NotImplementedException();
        }
    }
}
