using AntiLobby_2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antilobby_2
{
    //Holds user identifying information

    public class User
    {
        
        private string IP;
        private string MAC;
        private string userToken;
        private string email;

        public User()
        {
            this.IP = "" + MyUtils.GetIPAddress();
            this.MAC = "" + MyUtils.getMacAddress();
            this.email = "null";
        }

        public User(string inIP, string inMAC)
        {
            this.IP = inIP;
            this.MAC = inMAC;
            this.email = "null";
        }

        public string IP1 { get => IP; set => IP = value; }
        public string MAC1 { get => MAC; set => MAC = value; }
        public string Token { get => userToken; set => userToken = value; }
        public string Email { get => email; set => email = value; }

    }
}
