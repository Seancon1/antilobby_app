using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2
{
    public enum TimeConstant : Int32
    {
        // Time represented in seconds
        SECOND = 1,
        MINUTE = 60,
        HOUR = 3600,
        DAY = 86400
    }

    public class ProjConstants
    {
        public const int HOUR = 0;
        public const int MINUTE = 60;
        public const int SECOND = 1000;
        public const int MILLISECOND = 1000;

        ProjConstants()
        {
        }
    }

}
