using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2
{
    /*
     * Session holds all information that is used to identify a session in an application instance
     * Total time, User and an ID incase we have multiple sessions in an application instance
     *   
     * */
    class Session
    {
        private int id;
        private int tickCount; //total time elapsed
        private User user; //user associated with session

        public Session(User user)
        {
            this.id = 0;
            this.tickCount = 0;
            this.user = user;
        }

        public int Id { get => this.id; set => this.id = value; }
        public int TickCount { get => tickCount; set => tickCount = value; }

        public void saveSession()
        {
            /*
             pseudo

             if(session is present in database) {
                update session time
                update app timers
                }else{

                insert new values for session instance
                insert app timers
              
             */
            
        }
    


    }
}
