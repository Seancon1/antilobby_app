using System;

namespace Antilobby_2.Alert
{
    class AlertPlay
    {
        private Boolean active;

        //System.Media.SystemSounds.Hand.Play();

        public AlertPlay()
        {
            active = false;
        }

        public void turnOff()
        {
            active = false;
        }

        public void turnOn()
        {
            active = true;
        }

        public void play()
        {
            System.Media.SystemSounds.Hand.Play();
        }

    }
}
