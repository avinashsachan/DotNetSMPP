using System.Configuration;

namespace TestApp
{
    public static class FromConfig
    {
        public static string Server
        { get { return ConfigurationManager.AppSettings["Server"].ToString(); } }

        public static int Port
        { get { return System.Convert.ToInt32(ConfigurationManager.AppSettings["Port"].ToString()); } }

        public static string SystemID
        { get { return ConfigurationManager.AppSettings["SystemID"].ToString(); } }

        public static string Password
        { get { return ConfigurationManager.AppSettings["Password"].ToString(); } }
    }

}
