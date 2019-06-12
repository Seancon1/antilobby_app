using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AntiLobby_2
{
    public class MyUtils
    {
        MyUtils()
        {

        }

        public static String getIP()
        {
            string hostName = Dns.GetHostName(); // Retrive the Name of HOST 
            string myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            return myIP;
        }

        public static string GetIPAddress()
        {
            String address = "";
            WebRequest request = WebRequest.Create("http://checkip.dyndns.org/");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);

            return address;
        }

        public static int getSessionID()
        {
            Random random = new Random();
            StringBuilder id = new StringBuilder();
            int returnValue;
            while(id.Length < 10)
            {
                id.Append(random.Next(0, 9));
            }

            int.TryParse(id.ToString(), out returnValue);

            return returnValue;
        }

    }

    public class Time
    {
        DateTime timeNow = DateTime.Now;
        public static string datePatt = @"M/d/yyyy hh:mm:ss tt";

        public string getTimeNow()
        {
            DateTime dispDt = timeNow;
            string dtString;
            dtString = dispDt.ToString(datePatt);
            return dtString;
        }
    }
}
