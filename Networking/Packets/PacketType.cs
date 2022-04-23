using System;
using System.Collections.Generic;
using System.Text;

namespace Networking.Packets
{
    public enum PacketType : uint
    {
        RequestJoin = 1,    // Client Request to join a game
        AcceptJoin,         // Server accepts join
        AcceptJoinAck,      // Client acknowledges the AcceptJoin
        Heartbeat,          // Client tells Server its alive (before GameStart)
        HeartbeatAck,       // Server acknowledges Client's Heartbeat (before GameStart)
        GameStart,          // Server tells Clients game is starting
        GameStartAck,       // Client acknowledges the GameStart
        PaddlePosition,     // Client tell Server position of the their paddle
        GameState,          // Server tells Clients ball & paddle position, and scores
        PlaySoundEffect,    // Server tells the clinet to play a sound effect
        Bye,                // Either Server or Client tells the other to end the connection
    }
}
