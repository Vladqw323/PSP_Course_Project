using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkProtocole
{
    public class Server
    {
        private Socket _sender;
        private Socket _receiver;
        private IPEndPoint _clientIpEndPoint;

        public bool IsClient { get; private set; }
        public string PlayerPosition { private set; get; } = "0;0";
        public bool Serve { get; private set; } = false;

        public Server()
        {
            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _receiver.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));
            _receiver.Listen(1000);

            Listener();
        }

        private async void Listener()
        {
            try
            {
                while (true)
                {
                    Socket socket = await _receiver.AcceptAsync();
                    
                    if(!_sender.Connected)
                    {
                        Connect(socket);
                    }

                     Task.Run(() => HandleSocket(socket));
                }
            }
            catch
            {
            }
        }

        private async void Connect(Socket socket)
        {
            string message = await ReceiveAsync(socket);

            if (message.Contains("ClientIP"))
            {
                try
                {
                    string[] ip = message.Split(' ')[1].Split(':');
                    IPAddress address = IPAddress.Parse(ip[0]);
                    int port = int.Parse(ip[1]);

                    _sender.Connect(address, port);
                }
                catch
                {

                }
            }
        }

        private async Task HandleSocket(Socket socket)
        {
            try
            {
                byte[] buffer = new byte[1024];
                while (true)
                {
                    string message = await ReceiveAsync(socket);
                    ReceiveParseLogic(message);
                }
            }
            catch
            {
            }
        }

        private void ReceiveParseLogic(string message)
        {
            if (message.Contains("PlayerPosition"))
            {
                PlayerPosition = message.Split(' ')[1];
            }
            else if (message.Contains("Serve"))
            {
                Serve = message.Split(' ')[1].Equals("True");
            }
        }

        public void PlayerServe()
        {
            Serve = false;
        }

        public async Task<string> ReceiveAsync(Socket socket)
        {
            var lengthBuffer = new byte[4];

            socket.Receive(lengthBuffer);
            var messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            var messageBuffer = new byte[messageLength];
            int received = 0;

            while (received < messageLength)
            {
                received += await socket.ReceiveAsync(
                    new ArraySegment<byte>(messageBuffer, received, messageLength - received),
                    SocketFlags.None);
            }

            return Encoding.UTF8.GetString(messageBuffer);
        }

        public void Send(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            var lengthBytes = BitConverter.GetBytes(messageBytes.Length);

            var fullMessage = lengthBytes.Concat(messageBytes).ToArray();
            _sender.Send(fullMessage);
        }

        public void Close()
        {
            _receiver.Close();
            _sender.Close();
        }
    }
}
