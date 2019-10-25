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

        //System.Media.SystemSounds.Hand.Play();

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

        public void play()
        {
            System.Media.SystemSounds.Hand.Play();
        }

    }
}
