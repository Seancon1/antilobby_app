using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Antilobby_2.AutoUpdate
{
    class AutoUpdate
    {
        public double fetchedVersion;
        double programVersionNum;
        FetchUpdateVersion FetchUpdateVersion = new FetchUpdateVersion();


        /*
         * Grabs the version passed from the base program's hardcoded version number (int)
         * then compares it to the website version code
         * either UPDATES or stays put, depending on how they compare
         **/
        public AutoUpdate(double versionNumber)
        {

            this.programVersionNum = versionNumber;
        }

        public bool IsOutDated()
        {
            //Compare hardcoded version with webversion
            if(programVersionNum < FetchUpdateVersion.GetVersion())
            {
                return true;
            }
            return false;
        }

        public double getNewVersion()
        {
            this.fetchedVersion = FetchUpdateVersion.GetVersion();
            return FetchUpdateVersion.GetVersion();
        }


    }
}
