using Networking.Packets;
using System;
using System.Net;

namespace Networking
{
    public enum PaddleSide : uint
    {
        None,
        Left,
        Right
    };

    // Type of collision with paddle
    public enum PaddleCollision
    {
        None,
        WithTop,
        WithFront,
        WithBottom
    };

    public class NetworkMessage
    {
        public IPEndPoint Sender { get; set; }
        public Packet Packet { get; set; }
        public DateTime ReceiveTime { get; set; }
    }
}
