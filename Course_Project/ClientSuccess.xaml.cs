using NetworkProtocole;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace PingPongWPF
{
    /// <summary>
    /// Логика взаимодействия для ClientSuccess.xaml
    /// </summary>
    public partial class ClientSuccess : Page
    {
        const string SUCCESS_MESSAGE = "Успешное подключение. Ожидание ответа от сервер...";
        const string ERROR_MESSAGE = "Не удалось создавть подключение с сервером.";

        private Client _socket = null;

        public ClientSuccess(Client socket, bool state)
        {
            InitializeComponent();
            _socket = socket;
            LoadContext(state);
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void LoadContext(bool state)
        {
            text.Text = state ? SUCCESS_MESSAGE : ERROR_MESSAGE;

            if (_socket != null)
            {
                Task.Run(LoadServer);
            }
        }

        private async void LoadServer()
        {
            while(!_socket.IsStart) { await Task.Delay(100); }
            StartGame();
        }

        private void StartGame()
        {
            PingPongLibrary.Game game = new PingPongLibrary.Game(_socket);
            game.Run();
            while (game.GameReturn) { game = new PingPongLibrary.Game(_socket); game.Run(); }
            game.Dispose();

        }
    }
}
