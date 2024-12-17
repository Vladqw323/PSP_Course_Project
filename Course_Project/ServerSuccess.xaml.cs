using NetworkProtocole;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PingPongWPF
{
    /// <summary>
    /// Логика взаимодействия для ServerSuccess.xaml
    /// </summary>
    public partial class ServerSuccess : Page
    {
        private Server _socket = null;

        public ServerSuccess(Server socket)
        {
            InitializeComponent();
            _socket = socket;
            StartButton.IsEnabled = true;
            Task.Run(LoadClient);
            
        }

        private void LoadClient()
        {
            while (!_socket.IsClient) { }

            Application.Current.Dispatcher.Invoke(() =>
            {
                StartButton.IsEnabled = true;
            });
        }

        private void BackButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void StartButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _socket.Send("IsStart");
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
