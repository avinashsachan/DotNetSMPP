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
                TestTx();
                //TestRx();
                //TestTxRx();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }



        private static void TestTx()
        {
            var mno = 1234567890;
            var txt = "Hello";


            var smsc = new DotNetSMPP.SMPPClient(FromConfig.Server, FromConfig.Port, FromConfig.Username,
                FromConfig.Password, FromConfig.SystemType, DotNetSMPP.CommnadType.bind_transmitter);
            var resp = smsc.Bind();


            Console.WriteLine("SMPP Bind Request Result = {0}", resp);

            if (resp)
            {
                //here now create a submit sm






                resp = smsc.UnBind();
                Console.WriteLine("SMPP Unbind Request Result = {0}", resp);
            }



            ///Console.WriteLine(resp);

            //System.Threading.Thread.Sleep(1000 * 300);
            Console.WriteLine("");

        }




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
