using System.IO;
using System.Net;
using System.Text;

namespace Antilobby_2.AutoUpdate
{
    class FetchUpdateVersion
    {

        public static string VERSION_URL = "https://antilobby.prestigecode.com/download/latest/version";

        public double newVersionNumber;

        public FetchUpdateVersion()
        {

        }

        /**
         * Connects to the online version number
         * returns the result
         * */
        public double GetVersion()
        {
            string result = null;
            double returnVersion;
            var myClient = new WebClient();
            Stream response = myClient.OpenRead(VERSION_URL);
            // The stream data is used here.
            StreamReader streamReader = new StreamReader(response);

            result = streamReader.ReadLine();
            response.Close();

            double.TryParse(result, out returnVersion);
            StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder
            newVersionNumber = returnVersion;//set class public int to this as well
            return returnVersion;
        }
    }
}
