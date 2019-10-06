using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
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
            WebRequest request = WebRequest.Create("https://www.prestigecode.com/projects/antilobby/checkIP.php");
            using (WebResponse response = request.GetResponse())
            using (StreamReader stream = new StreamReader(response.GetResponseStream()))
            {
                address = stream.ReadToEnd();
            }

            /**
             * 
             * Modified old method to use prestigecode and also got rid of trimming the response
             * webpage returns ONLY the ip of the current user so no need to trim html before returning value
            
            int first = address.IndexOf("Address: ") + 9;
            int last = address.LastIndexOf("</body>");
            address = address.Substring(first, last - first);
            */

            return address;
        }

        public static String getSessionID()
        {
            Random rand = new Random(DateTime.Now.Millisecond);
            return rand.Next().ToString();
            
             /*
            Random random = new Random();
            StringBuilder id = new StringBuilder();

            while(id.Length < 10)
            {
                id.Append(random.Next(0, 9));
            }

            return id.ToString();
            */
        }

        public static string getMacAddress()
        {
            //Got this from some person on stackoverflow
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                /*
                log.Debug(
                    "Found MAC Address: " + nic.GetPhysicalAddress() +
                    " Type: " + nic.NetworkInterfaceType);
                */
                string tempMac = nic.GetPhysicalAddress().ToString();
                if (nic.Speed > maxSpeed &&
                    !string.IsNullOrEmpty(tempMac) &&
                    tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                {
                    //log.Debug("New Max Speed = " + nic.Speed + ", MAC: " + tempMac);
                    maxSpeed = nic.Speed;
                    macAddress = tempMac;
                }
            }

            return macAddress;
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
