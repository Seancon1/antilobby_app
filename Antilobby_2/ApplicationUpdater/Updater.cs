using System;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Antilobby_2.ApplicationUpdater
{
    class Updater
    {

        //public static string DIRECT_EXE_URL = "https://www.prestigecode.com/projects/antilobby/releases/current/Antilobby_release.exe";
        public static string DIRECT_EXE_URL = "https://antilobby.prestigecode.com/download/latest";
        
        public Updater()
        {
            DoProcess();
        }

        public static void DoProcess()
        {
            /*
            PowerShell.Create().AddCommand("Get-Process")
                   .AddParameter("Name", "PowerShell")
                   .Invoke();
                   */

            Uri uri = new Uri(System.Environment.CurrentDirectory);
            string placeToSave = uri.AbsoluteUri.ToString();

            //solution from stackoverflow https://stackoverflow.com/a/29185200
            //Saves newest file from online to current directory
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(DIRECT_EXE_URL, System.Environment.CurrentDirectory + "/Antilobby_release.exe");
                long timeNumber = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                File.Replace("Antilobby" + "_release" + ".exe", System.AppDomain.CurrentDomain.FriendlyName, "" + timeNumber);
                //Now, to move the backup file to the Windows UserProfile directory
                //Check if directory is made, make it if not
                var path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\.Antilobby\\backups\\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path); //Create directory
                }

                //Put path into new Uri object for flexibility
                Uri uriTest = new Uri((string) path);
                string absolutePath = @uriTest.AbsolutePath + "" + timeNumber; 

                //Delete old backup if it's already there
                /**
                if (File.Exists(absolutePath))
                {
                    File.SetAttributes(absolutePath, FileAttributes.Normal); //takes away access denied error?
                    //File.Delete(absolutePath);
                }
    **/
                //File.Move("Antilobby_2.exe.bac", absolutePath);
                File.Move(""+timeNumber, absolutePath);
                File.SetAttributes(absolutePath, FileAttributes.Normal); //takes away the access denied error?

            }



            //start a replace file action, TESTING
            //File.Replace("Antilobby" + "_new" + ".txt", "Antilobby_2.txt", "Antilobby_2.txt.bac");
            //File.Replace(System.AppDomain.CurrentDomain.FriendlyName + "_new" + ".txt", System.AppDomain.CurrentDomain.FriendlyName, "Antilobby.exe.bac");

        }

    }

    
}
