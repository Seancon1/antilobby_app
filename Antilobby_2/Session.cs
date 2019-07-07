using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntiLobby_2;

namespace Antilobby_2
{
    /*
     * Session holds all information that is used to identify a session in an application instance
     * Total time, User and an ID incase we have multiple sessions in an application instance
     *   
     * */
    class Session
    {
        private string id;
        private int tickCount; //total time elapsed
        private User user; //user associated with session
        public ProcessList processList = null;
        private Logger logger = null;
        private Boolean state = true;
        public Alert.AlertList alertList = null;

        public Session(User user)
        {
            
            this.tickCount = 0;
            this.user = user;
            this.id = MyUtils.getSessionID(); //Create and assign new session ID immediately
            this.logger = new Logger(this, user); //create a logger instance
            this.processList = new ProcessList(this, user);
            this.alertList = new Alert.AlertList(processList); //processList must not be null before linking, aka create processList link before calling this
        }

        public string Id { get => this.id; set => this.id = value; }
        public int TickCount { get => tickCount; set => tickCount = value; }
        public bool State { get => state; set => state = value; }

        public void incrementTick()
        {
            this.tickCount++;
        }

        public void saveSession()
        {
            logger.saveSessionAsync();
        }

    }
}
