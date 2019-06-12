﻿using AntiLobby_2;
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

        public User()
        {
            this.IP = "" + 0;
            this.MAC = "" + 1;
        }

        public User(string inIP, string inMAC)
        {
            this.IP = inIP;
            this.MAC = inMAC;
        }

        public string IP1 { get => IP; set => IP = value; }
        public string MAC1 { get => MAC; set => MAC = value; }
    }
}