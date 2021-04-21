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
            string myIP = "0.0.0.0";
            try
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST 
                myIP = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Unable to getIP. " + exception.ToString());
            }
            return myIP;
        }

        public static string GetIPAddress()
        {
            String address = "null";

            try
            {
                
                WebRequest request = WebRequest.Create("https://www.prestigecode.com/get/myip");
                using (WebResponse response = request.GetResponse())
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    address = stream.ReadToEnd();
                }

            } catch (Exception exception)
            {
                Console.WriteLine("Unable to GetIPAddress. " + exception.ToString());
            }

            return address;
        }

        public static String getSessionID()
        {
            //Random rand = new Random(DateTime.Now.Millisecond);
            //return rand.Next().ToString();
            
             
            Random random = new Random();
            StringBuilder id = new StringBuilder();

            while(id.Length < 16)
            {
                id.Append(random.Next(0, 9));
            }

            return id.ToString();
            
        }

        /*
         * 
         * Fetched from stackoverflow, saved here incase needed
         * */
        public static string getMacAddress()
        {
            const int MIN_MAC_ADDR_LENGTH = 12;
            string macAddress = string.Empty;
            long maxSpeed = -1;

            try
            {
                //Got this from some person on stackoverflow
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

                

            } catch (Exception exception)
            {
                Console.WriteLine("Unable to getMacAddress. " + exception.ToString());
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

    /*
    public void WriteToFile(string programCloseTime)
    {
        time endingtime = new time();
        string path = @"D:\Google Drive\#zpersonal\LOGS\Antilobby\Log.txt";
        if (File.Exists(@"D:\Google Drive\#zpersonal\LOGS\Antilobby\Log.txt"))
        {
            //string content = File.ReadAllText(@"C:\Users\Sean\Documents\ComputerLog\Log.txt");

            string newContent = "Session: " + MasterStartTime + " to " + endingtime.getTimeNow() + " ||  Time Spend: " + main.getConvertTime(main.TickCount) + Environment.NewLine;
            //label1.Text = newContent;
            //File.WriteAllText(path, newContent);
            File.AppendAllText(path, newContent);

        }
    }
    */

    

}
