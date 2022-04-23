using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    // Tells the client to begin sending data
    public class GameStartPacket : Packet
    {
        public string Nick
        {
            get { return BitConverter.ToString(Payload, 0); }
            set { Encoding.ASCII.GetBytes(value).CopyTo(Payload, 0); }
        }

        public GameStartPacket() : base(PacketType.GameStart)
        {
            Payload = new byte[30];
        }
    }
}
