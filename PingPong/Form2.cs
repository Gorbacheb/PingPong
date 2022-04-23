using Networking;
using Networking.Packets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PingPong
{
    public enum ClientState
    {
        NotConnected,
        EstablishingConnection,
        WaitingForGameStart,
        InGame,
        GameOver,
    }

    public partial class Form2 : Form
    {
        // Network stuff
        private UdpClient _udpClient;
        public readonly string ServerHostname;
        public readonly int ServerPort;

        // Time measurement
        private DateTime _lastPacketReceivedTime = DateTime.MinValue;     // From Client Time
        private DateTime _lastPacketSentTime = DateTime.MinValue;         // From Client Time
        private long _lastPacketReceivedTimestamp = 0;                    // From Server Time
        private TimeSpan _heartbeatTimeout = TimeSpan.FromSeconds(20);
        private TimeSpan _sendPaddlePositionTimeout = TimeSpan.FromMilliseconds(1000f / 30f);  // How often to update the server

        // Messaging
        private Thread _networkThread;
        private ConcurrentQueue<NetworkMessage> _incomingMessages = new ConcurrentQueue<NetworkMessage>();
        private ConcurrentQueue<Packet> _outgoingMessages = new ConcurrentQueue<Packet>();

        // Game objects
        private Paddle _left;
        private Paddle _right;
        private Paddle _ourPaddle;
        private float _previousY;

        // State stuff
        private ClientState _state = ClientState.NotConnected;
        private ThreadSafe<bool> _running = new ThreadSafe<bool>(false);
        private ThreadSafe<bool> _sendBye = new ThreadSafe<bool>(false);


        Random rnd = new Random();

        public Form2(string hostname, int port)
        {
            ServerHostname = hostname;
            ServerPort = port;
            _udpClient = new UdpClient(ServerHostname, ServerPort);
            InitializeComponent();
            _left = new Paddle(Properties.Resources.player_left, PaddleSide.Left, this);
            _right = new Paddle(Properties.Resources.player_right, PaddleSide.Right, this);
            Start();
        }

        int cnt = 0;
        private void Form2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                // Player wants to quit, send a ByePacket (if we're connected)
                if ((_state == ClientState.EstablishingConnection) || (_state == ClientState.WaitingForGameStart) || (_state == ClientState.InGame))
                {
                    // Will trigger the network thread to send the Bye Packet
                    _sendBye.Value = true;
                }

                // Will stop the network thread
                _running.Value = false;
                _state = ClientState.GameOver;
                this.Close();
            }

            // Check Up & Down keys

            int locY = _ourPaddle.Position.Y;

            if (e.KeyCode == Keys.Up)
                locY -= GameGeometry.PaddleSpeed;
            else if (e.KeyCode == Keys.Down)
                locY += GameGeometry.PaddleSpeed;

            // bounds checking
            if (locY < 0)
                locY = 0;
            else if (locY + GameGeometry.PaddleSize.Y > GameGeometry.PlayArea.Y)
                locY = GameGeometry.PlayArea.Y - GameGeometry.PaddleSize.Y;
            _ourPaddle.Position = new Point(_ourPaddle.Position.X, locY);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            HeartBeatText.Text = DateTime.Now.ToString() + " " + _state.ToString();
            // Check for time out with the server
            if (_timedOut())
                _state = ClientState.GameOver;

            // Get message
            NetworkMessage message;
            bool haveMsg = _incomingMessages.TryDequeue(out message);

            // Check for Bye From server
            if (haveMsg && (message.Packet.Type == PacketType.Bye))
            {
                // Shutdown the network thread (not needed anymore)
                _running.Value = false;
                _state = ClientState.GameOver;
            }

            switch (_state)
            {
                case ClientState.EstablishingConnection:
                    _sendRequestJoin(TimeSpan.FromSeconds(1));
                    if (haveMsg)
                        _handleConnectionSetupResponse(message.Packet);
                    break;

                case ClientState.WaitingForGameStart:
                    // Send a heartbeat
                    _sendHeartbeat(TimeSpan.FromSeconds(0.2));

                    if (haveMsg)
                    {
                        switch (message.Packet.Type)
                        {
                            case PacketType.AcceptJoin:
                                // It's possible that they didn't receive our ACK in the previous state
                                _sendAcceptJoinAck();
                                break;

                            case PacketType.HeartbeatAck:
                                // Record ACK times
                                _lastPacketReceivedTime = message.ReceiveTime;
                                if (message.Packet.Timestamp > _lastPacketReceivedTimestamp)
                                    _lastPacketReceivedTimestamp = message.Packet.Timestamp;
                                break;

                            case PacketType.GameStart:
                                // Start the game and ACK it
                                _sendGameStartAck();
                                _state = ClientState.InGame;
                                break;
                        }

                    }
                    break;

                case ClientState.InGame:
                    // Send a heartbeat
                    _sendHeartbeat(TimeSpan.FromSeconds(0.2));

                    // update our paddle
                    //_previousY = _ourPaddle.Position.Y;
                    //_ourPaddle.ClientSideUpdate(gameTime);
                    _sendPaddlePosition(_sendPaddlePositionTimeout);

                    if (haveMsg)
                    {
                        switch (message.Packet.Type)
                        {
                            case PacketType.GameStart:
                                // It's possible the server didn't receive our ACK in the previous state
                                _sendGameStartAck();
                                break;

                            case PacketType.HeartbeatAck:
                                // Record ACK times
                                _lastPacketReceivedTime = message.ReceiveTime;
                                if (message.Packet.Timestamp > _lastPacketReceivedTimestamp)
                                    _lastPacketReceivedTimestamp = message.Packet.Timestamp;
                                break;

                            case PacketType.GameState:
                                // Update the gamestate, make sure its the latest
                                if (message.Packet.Timestamp > _lastPacketReceivedTimestamp)
                                {
                                    _lastPacketReceivedTime = message.ReceiveTime;
                                    _lastPacketReceivedTimestamp = message.Packet.Timestamp;

                                    GameStatePacket gsp = new GameStatePacket(message.Packet.GetBytes());
                                    _left.Score = gsp.LeftScore;
                                    _right.Score = gsp.RightScore;
                                    PBoxBall.Location = gsp.BallPosition;

                                    networkPaddleText.Text = "Paddle: " + gsp.LeftY + ": " + gsp.RightY + "\n ";
                                    //_ball.Position = gsp.BallPosition;

                                    // Update what's not our paddle
                                    cnt++;
                                    if (_ourPaddle.Side == PaddleSide.Left)
                                    {
                                        _right.Position = new Point(_right.Position.X, gsp.RightY);
                                    }
                                    else
                                    {
                                        _left.Position = new Point(_left.Position.X, gsp.LeftY);
                                    }
                                       
                                }

                                break;

                            case PacketType.PlaySoundEffect:

                                break;
                        }
                    }

                    break;

                case ClientState.GameOver:
                    
                    break;
            }
            Draw();
        }

        private void ballTimer_Tick(object sender, EventArgs e)
        {

        }

        public void Start()
        {
            _running.Value = true;
            _state = ClientState.EstablishingConnection;

            // Start the packet receiving/sending Thread
            _networkThread = new Thread(new ThreadStart(_networkRun));
            _networkThread.Start();
        }


        #region Graphical Functions
        protected void Draw()
        {
            // Draw different things based on the state
            switch (_state)
            {
                case ClientState.EstablishingConnection:
                    Text = String.Format("Pong -- Connecting to {0}:{1}", ServerHostname, ServerPort);
                    break;

                case ClientState.WaitingForGameStart:
                    Text = String.Format("Pong -- Waiting for 2nd Player");
                    break;

                case ClientState.InGame:
                    // Change the window title
                    _updateWindowTitleWithScore();
                    break;
                case ClientState.GameOver:
                    _updateWindowTitleWithScore();
                    break;
            }
        }

        private void _updateWindowTitleWithScore()
        {
            if (_state == ClientState.GameOver)
            {
                Text = "Игра окончена";
            }
            else
            {
                string fmt = (_ourPaddle.Side == PaddleSide.Left) ? "[{0}] -- Pong -- {1}" : "{0} -- Pong -- [{1}]";
                this.Text = string.Format(fmt, _left.Score, _right.Score);
            }

        }
        #endregion // Graphical Functions

        #region Network Functions
        // This function is meant to be run in its own thread
        // and will populate the _incomingMessages queue
        private void _networkRun()
        {
            while (_running.Value)
            {
                bool canRead = _udpClient.Available > 0;
                int numToWrite = _outgoingMessages.Count;

                // Get data if there is some
                if (canRead)
                {
                    // Read in one datagram
                    IPEndPoint ep = new IPEndPoint(IPAddress.Any, 0);
                    byte[] data = _udpClient.Receive(ref ep);              // Blocks

                    // Enque a new message
                    NetworkMessage nm = new NetworkMessage();
                    nm.Sender = ep;
                    nm.Packet = new Packet(data);
                    nm.ReceiveTime = DateTime.Now;

                    _incomingMessages.Enqueue(nm);

                    //Console.WriteLine("RCVD: {0}", nm.Packet);
                }

                // Write out queued
                for (int i = 0; i < numToWrite; i++)
                {
                    // Send some data
                    Packet packet;
                    bool have = _outgoingMessages.TryDequeue(out packet);
                    if (have)
                        packet.Send(_udpClient);

                    //Console.WriteLine("SENT: {0}", packet);
                }

                // If Nothing happened, take a nap
                if (!canRead && (numToWrite == 0))
                    Thread.Sleep(1);
            }

            // Check to see if a bye was requested, one last operation
            if (_sendBye.Value)
            {
                ByePacket bp = new ByePacket();
                bp.Send(_udpClient);
                Thread.Sleep(1000);     // Needs some time to send through
            }
        }

        // Queues up to send a single Packet to the server
        private void _sendPacket(Packet packet)
        {
            _outgoingMessages.Enqueue(packet);
            _lastPacketSentTime = DateTime.Now;
            //HeartBeatText.Text = DateTime.Now.ToString() + "  " + _lastPacketSentTime.ToString() + " " + _state;
        }

        // Sends a RequestJoinPacket,
        private void _sendRequestJoin(TimeSpan retryTimeout)
        {
            // Make sure not to spam them
            if (DateTime.Now >= (_lastPacketSentTime.Add(retryTimeout)))
            {
                RequestJoinPacket gsp = new RequestJoinPacket();
                _sendPacket(gsp);
            }
        }

        // Acks the AcceptJoinPacket
        private void _sendAcceptJoinAck()
        {
            AcceptJoinAckPacket ajap = new AcceptJoinAckPacket();
            _sendPacket(ajap);
        }

        // Responds to the Packets where we are establishing our connection with the server
        private void _handleConnectionSetupResponse(Packet packet)
        {
            // Check for accept and ACK
            if (packet.Type == PacketType.AcceptJoin)
            {
                // Make sure we haven't gotten it before
                if (_ourPaddle == null)
                {
                    // See which paddle we are
                    AcceptJoinPacket ajp = new AcceptJoinPacket(packet.GetBytes());
                    if (ajp.Side == PaddleSide.Left)
                        _ourPaddle = _left;
                    else if (ajp.Side == PaddleSide.Right)
                        _ourPaddle = _right;
                    else
                        throw new Exception("Error, invalid paddle side given by server.");     // Should never hit this, but just incase
                }

                // Send a response
                _sendAcceptJoinAck();

                // Move the state
                _state = ClientState.WaitingForGameStart;
            }
        }

        // Sends a HearbeatPacket to the server
        private void _sendHeartbeat(TimeSpan resendTimeout)
        {
            // Make sure not to spam them
            if (DateTime.Now >= (_lastPacketSentTime.Add(resendTimeout)))
            {
                HeartbeatPacket hp = new HeartbeatPacket();
                _sendPacket(hp);
            }
        }

        // Acks the GameStartPacket
        private void _sendGameStartAck()
        {
            GameStartAckPacket gsap = new GameStartAckPacket();
            _sendPacket(gsap);
        }

        // Sends the server our current paddle's Y Position (if it's changed)
        private void _sendPaddlePosition(TimeSpan resendTimeout)
        {
            localPaddleText.Text = _ourPaddle.Position.Y.ToString();
            // Don't send anything if there hasn't been an update
            //if (_previousY == _ourPaddle.Position.Y)
            //    return;

            // Make sure not to spam them
            if (DateTime.Now >= (_lastPacketSentTime.Add(resendTimeout)))
            {
                PaddlePositionPacket ppp = new PaddlePositionPacket();
                ppp.Y = _ourPaddle.Position.Y;

                _sendPacket(ppp);
            }
        }

        // Returns true if out connection to the server has timed out or not
        // If we haven't recieved a packet at all from them, they're not timed out
        private bool _timedOut()
        {
            // We haven't recorded it yet
            if (_lastPacketReceivedTime == DateTime.MinValue)
                return false;

            // Do math
            return (DateTime.Now > (_lastPacketReceivedTime.Add(_heartbeatTimeout)));
        }
        #endregion // Network Functions

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {

            _sendBye.Value = true;
            _running.Value = false;
            _state = ClientState.GameOver;
        }
    }
}
