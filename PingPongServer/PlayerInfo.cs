using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace PingPongServer
{
    // Data structure for the server to manage clients
    public class PlayerInfo
    {
        public Paddle Paddle;
        public IPEndPoint Endpoint;
        public DateTime LastPacketReceivedTime = DateTime.MinValue;     // From Server Time
        public DateTime LastPacketSentTime = DateTime.MinValue;         // From Server Time
        public long LastPacketReceivedTimestamp = 0;                    // From Client Time
        public bool HavePaddle = false;
        public bool Ready = false;

        public bool IsSet
        {
            get { return Endpoint != null; }
        }

        public int Ping
        {
            get => (int)(LastPacketSentTime - LastPacketReceivedTime).TotalMilliseconds;
        }
    }
}
