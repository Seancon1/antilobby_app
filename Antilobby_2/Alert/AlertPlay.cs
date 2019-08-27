using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2.Alert
{
    class AlertPlay
    {
        private Boolean active;

        public AlertPlay()
        {
            this.active = false;
        }

        public void turnOff()
        {
            this.active = false;
        }

        public void turnOn()
        {
            this.active = true;
        }


    }
}
