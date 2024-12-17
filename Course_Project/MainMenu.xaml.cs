using NetworkProtocole;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PingPongWPF
{
    /// <summary>
    /// Логика взаимодействия для MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        private object _socket = null;
        public MainMenu()
        {
            InitializeComponent();
            Loaded += LoadedHandler;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _socket = new Server();
            NavigationService.Navigate(new ServerSuccess((Server)_socket));
        }

        private async void JoinButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _socket = new Client(new System.Net.IPEndPoint(System.Net.IPAddress.Parse("127.0.0.1"), 8080));
                ((Client)_socket).Send($"ClientIP {((Client)_socket).IPEndPointToString}");
                NavigationService.Navigate(new ClientSuccess((Client)_socket, true));
            }
            catch
            {
                NavigationService.Navigate(new ClientSuccess(null, false));
            }
        }

        private void LoadedHandler(object sender, RoutedEventArgs e)
        {
            if (_socket != null)
            {
                if(typeof(Server) == _socket.GetType())
                {
                    ((Server)_socket).Close();
                }
                else
                {
                    ((Client)_socket).Close();
                }
            }
        }
    }
}
