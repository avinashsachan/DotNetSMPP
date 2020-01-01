using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestClient
{
    public class FromConfig
    {
        public static string Server
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["server"].ToString();
            }
        }

        public static int Port
        {
            get
            {
                return Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["port"].ToString());
            }
        }

        public static string Username
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["user"].ToString();
            }
        }


        public static string Password
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["pass"].ToString();
            }
        }


        public static string SystemType
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["systemType"].ToString();
            }
        }
    }
}
