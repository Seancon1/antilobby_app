using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AntiLobby_2;
using Antilobby_2.Utils;
namespace Antilobby_2
{
    /*
     * Session holds ALL information that is used to identify a session in an application instance
     * Total time, User and an ID incase we have multiple sessions in an application instance
     * Alerts also included in session
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

        /*
         * String activeProcess - string that represents the current active process 
         * */

        public void incrementTick(String activeProcess)
        {
            this.tickCount++;
            //also increment all alert counts
            if (alertList == null) { } else
            {
                /*
                 * pass the activeProcess to the alertList incrementing so that AFK counter will increment
                 * */
                alertList.incrementAllTicks(activeProcess);
            }
            
        }

        public void saveSession()
        {
            logger.saveSessionAsync();
        }

        public void saveSessionOffline()
        {
            logger.SaveOffline(this);
        }

        public String fetchOfflineStorage()
        {
            return logger.fetchOfflineStorage();
        }

        public void saveAllOfflineStorage()
        {
            String offlineStorage = logger.fetchOfflineStorage(); //fetch text from file
            SessionConverter sessionConverter = new SessionConverter(offlineStorage); //put text into converter
            ProcessList processList = new ProcessList(this, this.user); //create a new processList so we can use save to db function

            while(sessionConverter.getFlag() > 0)
            {
                foreach(var item in sessionConverter.GetFirstProcessList())
                    {
                        //check flag
                        processList.addItem(new ProcessItem(item.getName(), item.getTime()));
                
                    }
            }
            
            //if(everything is fine)

            processList.saveToDatabase();
            
        }

    }
}
