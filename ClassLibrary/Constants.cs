using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary
{
    public class Constants
    {
        public static string MqHost
        {
            get
            {
                return "localhost";
            }
         
        }
        public static int MqPort
        {
            get
            {
                return 5672;
            }
          
        }

        public static string MqUserName
        {
            get
            {
                return "rollen";
            }
           
        }

        public static string MqPwd
        {
            get
            {
                return "root";
            }
           
        }
    }
}
