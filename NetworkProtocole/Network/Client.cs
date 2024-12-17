using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetworkProtocole
{
    public class Client
    {
        private Socket _sender;
        private Socket _receiver;
        private IPEndPoint _clientIpEndPoint;
        public string IPEndPointToString => $"{_clientIpEndPoint.Address}:{_clientIpEndPoint.Port}";
        public bool IsStart { private set; get; } = false;

        public string BallPosition { private set; get; } = "0;0";
        public string PlayerPosition { private set; get; } = "0;0";
        public bool Serveraffle { private set; get; } = true;
        public string Bonus { private set; get; } = "";
        public bool TurnServe { private set; get; } = false;
        public bool IsBonus { private set; get; } = false;

        public Client(IPEndPoint iPEndPoint)
        {
            _clientIpEndPoint = iPEndPoint;

            _sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _sender.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));

            _receiver = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _receiver.Bind(iPEndPoint);
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
                    Task.Run(() => HandleSocklet(socket));
                }
            }
            catch
            {
            }
        }

        private async Task HandleSocklet(Socket socket)
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
            if (message.Contains("IsStart"))
            {
                IsStart = true;
            }
            else if (message.Contains("Ball"))
            {
                BallPosition = message.Split(' ')[1];
            }
            else if (message.Contains("PlayerPosition"))
            {
                PlayerPosition = message.Split(' ')[1];
            }
            else if (message.Contains("Serveraffle"))
            {
                Serveraffle = message.Split(' ')[1].Equals("True") ? true : false;
            }
            else if (message.Contains("Bonus"))
            {
                if (message.Split(' ').Length == 2)
                {
                    Bonus = message.Split(' ')[1];
                }
                else
                {
                    Bonus = "";
                }
            }  
            else if(message.Contains("TurnServe"))
            {
                TurnServe = message.Split(' ')[1].Equals("True");
            }
        }

        public void SetBonus(bool isBonus)
        {
            IsBonus = isBonus;
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

        public void PlayerSevre()
        {
            TurnServe = false;
        }

        public void Close()
        {
            _receiver.Close();
            _sender.Close();
        }
    }
}
