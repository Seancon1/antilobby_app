using Antilobby_2.Utils;
using AntiLobby_2;
using System;

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
            tickCount = 0;
            this.user = user;
            id = "null"; //fill id as null for replacement shortly after creation
            logger = new Logger(this, user); //create a logger instance
            processList = new ProcessList(this, user);
            alertList = new Alert.AlertList(processList); //processList must not be null before linking, aka create processList link before calling this
            user.Token = readUserToken(); //set token of user that has been passed to this session instance
            this.user.Token = readUserToken();//gets token if located on machine locally, otherwise set null !! LOGGER instance must be called 
            logger.getSessionIDFromAPI(); //this will replace null originally placed
            State = false; //start in a false state
            Paused = false; //start not paused
        }

        public string Id { get => id; set => id = value; }
        public int TickCount { get => tickCount; set => tickCount = value; }
        public bool State { get => state; set => state = value; }
        public bool Paused { get => state; set => state = value; }

        /*
         * String activeProcess - string that represents the current active process 
         * */

        public void incrementTick(String activeProcess)
        {
            tickCount++;
            /*
             * Set State to true. This value is defined as a way to tell us if we have done any incrementation ever.
             * */
            State = true;

            //also increment all alert counts
            if (alertList == null) { }
            else
            {
                /*
                 * pass the activeProcess to the alertList incrementing so that AFK counter will increment
                 * */
                alertList.incrementAllTicks(activeProcess);
            }


            /*
             * Experimental Feature
             * Tracks apps that user interacted with and counts the amount of time they are open/having process time
             * */
            /*
            try
            {
                if(processList.ReturnAllItems() != null)
                foreach(ProcessItem processItem in processList.ReturnAllItems())
                {
                    var checkProcess = Process.GetProcessesByName(processItem.getName()).First() != null ? true: false;
                    if (checkProcess)
                    {
                        using (var process = Process.GetProcessesByName(processItem.getName()).First())
                        {
                            if (process != null && !process.HasExited )
                            {
                                processItem.logOpenTime();
                                //Debug.Print($"Logging OpenTime for this item({processItem.getName()}): Now has {processItem.getTimeOpen()} ticks");
                            }
                        }   
                    }
                }
            }
            catch(Exception error) { Debug.Print("Error logging process name idle time. May have been closed" + error); }
            //add tick if process name is still open but no actively engaged
            */

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
            ProcessList processList = new ProcessList(this, user); //create a new processList so we can use save to db function

            while (sessionConverter.getFlag() > 0)
            {
                foreach (var item in sessionConverter.GetFirstProcessList())
                {
                    //check flag
                    processList.addItem(new ProcessItem(item.getName(), item.getTime()));

                }
            }

            //if(everything is fine)

            processList.saveToDatabase();

        }

        public void setInMemoryUserToken(string token)
        {
            user.Token = token;
        }
        public string getInMemoryUserToken()
        {
            return user.Token;
        }
        public bool hasInMemoryUserToken()
        {
            try
            {
                return (user.Token.Length < 1 || user.Token == "null") ? false : true;
            }
            catch
            {
                return false;
            }

        }

        public void setInMemoryUserEmail(string email)
        {
            user.Email = email;
        }
        public string getInMemoryUserEmail()
        {
            return user.Email;
        }


        public void saveUserToken(string content)
        {
            logger.SaveOfflineGeneric("_UserToken.antilobby", new string[] { content });
        }

        public string readUserToken()
        {
            var contents = logger.readOfflineGeneric("_UserToken.antilobby");
            //System.Windows.Forms.MessageBox.Show("File contents:" + contents);

            if (contents.Length < 1)
            {
                return null;
            }
            else { return contents[0]; }
        }


        /**
         * Check device storage for token
         * 1) Find file
         * 2) Extract token
         * */
        public string fetchDeviceToken()
        {
            return "null";
        }

    }
}
