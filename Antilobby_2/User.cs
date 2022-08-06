using AntiLobby_2;

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
            IP = "" + MyUtils.GetIPAddress();
            MAC = "" + MyUtils.getMacAddress();
            email = "null";
        }

        public User(string inIP, string inMAC)
        {
            IP = inIP;
            MAC = inMAC;
            email = "null";
        }

        public string IP1 { get => IP; set => IP = value; }
        public string MAC1 { get => MAC; set => MAC = value; }
        public string Token { get => userToken; set => userToken = value; }
        public string Email { get => email; set => email = value; }

    }
}
