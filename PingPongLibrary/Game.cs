using _PingPongLibrary._GameObject;
using NetworkProtocole;
using PingPongLibrary.DirectX;
using PingPongLibrary.Entity;
using PingPongLibrary.Entity.PlayerDecorator;
using PingPongLibrary.GameObject;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX.Windows;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PingPongLibrary
{
    /// <summary>
    /// Класс, содержащий основную логику игры, связывает логические компоненты объектов и компоненты отрисовки
    /// </summary>
    public class Game
    {
        private static float _unitsPerHeight = 20.0f;

        /// <summary>
        /// Окно программы
        /// </summary>
        public RenderForm RenderForm { get; private set; }

        // Инфраструктурные объекты
        /// <summary>
        /// Экземпляр класса DX2D
        /// </summary>
        public DX2D DX2D { get; private set; }
        private InputManager _dInput;

        WindowRenderTarget _target;
        Size2F _targetSize;

        // Клиетская область порта отрисовки в устройство-независимых пикселях
        private RectangleF _clientRect;

        /// <summary>
        /// Коэффициент масштабирования
        /// </summary>
        public float Scale { get; private set; }

        // Помощник для работы со временем
        private TimeHelper _timeHelper;

        // Остальные приватные поля, используемые для разных целей
        private BackgroundGame _background;
        private PlayerGame _firstPlayer;
        private PlayerGame _secondPlayer;
        private WallGame _topWall;
        private WallGame _botWall;
        private BallGame _ball;
        private int _ballIndex;
        private int _giftIndex;
        private int _playerIndex;
        private bool _serveraffle;
        private bool _serve;
        private Random _rnd;
        private BonusGame _bonus;
        private BonusFactory _bonusFactory;
        private Generator _generator;
        private bool _giftdraw;
        private PhysicsEngine _physicsEngine;
        private bool _gameEnd;

        /// <summary>
        /// Значение перезапуска приложения
        /// </summary>
        public bool GameReturn { get; private set; }

        Drawer _drawer;
        private object _socket;


        // В конструкторе создаем форму, инфраструктурные объекты, подгружаем спрайты, создаем помощник для работы со временем
        // В конце вызываем ресайзинг формы для вычисления масштаба и установки пределов по горизонтали и вертикали
        public Game(object socket)
        {
            _socket = socket;

            RenderForm = new RenderForm("Ping Pong");
            RenderForm.WindowState = FormWindowState.Maximized;
            RenderForm.IsFullscreen = true;

            DX2D = new DX2D(RenderForm);
            _dInput = new InputManager(RenderForm);

            _target = DX2D.RenderTarget;
            _targetSize = _target.Size;
            // Индексы наших картинок
            int backgroundIndex = DX2D.GetLoadBitmap("..\\..\\..\\table_2060_1300.png");
            _playerIndex = DX2D.GetLoadBitmap("..\\..\\..\\paddle.png");
            _ballIndex = DX2D.GetLoadBitmap("..\\..\\..\\ball.png");
            _giftIndex = DX2D.GetLoadBitmap("..\\..\\..\\gift.png");
            // Инициализая наших отображаемых объектов
            _background = new BackgroundGame(new Entity.Background(new Vector2(RenderForm.ClientSize.Width / 2, RenderForm.ClientSize.Height / 2)), new Sprite(backgroundIndex, (int)RenderForm.Width,
            (int)RenderForm.Height, 0));

            if (typeof(Client) == socket.GetType())
            {
                _firstPlayer = new PlayerGame(new Player(new Vector2(1720, 540)), new List<Sprite> { new Sprite(_playerIndex, 15, 80, 0) });
                _secondPlayer = new PlayerGame(new Player(new Vector2(200, 540)), new List<Sprite> { new Sprite(_playerIndex, 15, 80, 0) });
            }
            else
            {
                _firstPlayer = new PlayerGame(new Player(new Vector2(200, 540)), new List<Sprite> { new Sprite(_playerIndex, 15, 80, 0) });
                _secondPlayer = new PlayerGame(new Player(new Vector2(1720, 540)), new List<Sprite> { new Sprite(_playerIndex, 15, 80, 0) });
            }

            _topWall = new WallGame(new Wall(new Vector2(_targetSize.Width / 2, 0)), new List<Sprite> { new Sprite(0, (int)_targetSize.Width, 10, 0) });
            _botWall = new WallGame(new Wall(new Vector2(_targetSize.Width / 2, _targetSize.Height)), new List<Sprite> { new Sprite(0, (int)_targetSize.Width, 10, 0) });

            _ball = new BallGame(new Ball(new Vector2(_targetSize.Width / 2, _targetSize.Height / 2)), new List<Sprite> { new Sprite(_ballIndex, 32, 32, 0) });

            GameReturn = false;
            _gameEnd = false;
            _physicsEngine = new PhysicsEngine();
            _rnd = new Random();
            _giftdraw = false;
            _serve = false;
            _serveraffle = true;
            _drawer = new Drawer(DX2D);
            _timeHelper = new TimeHelper();
            RenderForm_Resize(this, null);
            float scale = DX2D.RenderTarget.Size.Width / DX2D.RenderTarget.PixelSize.Width;


            Recive();
            Send();
        }
        /// <summary>
        /// Вызываются методы отображения объектов на игровом поле, методы колли-зии, методы проверки окончания игры, а также в данном методе заключена логика передвижения игроков
        /// </summary>
        private void RenderCallback()
        {
            if (!_gameEnd)
            {
                // Вызываем обновление состояния "временного" помощника и объектов ввода
                _timeHelper.Update();
                _dInput.UpdateKeyboardState();

                // Для расчетов масштаба берем не клиентскую область формы, которая в честных пикселях, а RenderTarget-а
                WindowRenderTarget target = DX2D.RenderTarget;
                Size2F targetSize = target.Size;
                _clientRect.Width = targetSize.Width;
                _clientRect.Height = targetSize.Height;

                // Начинаем вывод графики
                _target.BeginDraw();
                // Перво-наперво - очистить область отображения
                _target.Clear(Color.Black);

                // Прорисовка наших объектов
                _drawer.Draw(_background);

                _drawer.DrawObject(_firstPlayer);
                _drawer.DrawObject(_secondPlayer);
                _drawer.Draw(_ball);

                EndGame();

                // Движение игроков (ракеток) с условием на коллизию
                if (_dInput.KeyboardUpdated)
                {
                    ControlMethod(Key.W, Key.S, Key.A, Key.D, _firstPlayer);

                    if (_firstPlayer.Player.TurnServe && _dInput.KeyboardState.IsPressed(Key.Space) && !_serveraffle)
                    {
                        if (typeof(Client) == _socket.GetType())
                        {
                            ((Client)_socket).Send("Serve True");
                            ((Client)_socket).PlayerSevre();
                            _firstPlayer.Player.SetServe(false);
                        }
                        else
                        {
                            _serve = true;
                        }
                    }
                }

                if (typeof(Server) == _socket.GetType())
                {
                    if (((Server)_socket).Serve)
                    {
                        _serve = true;
                        ((Server)_socket).PlayerServe();
                    }
                }

                if (!_serveraffle)
                {
                    if (typeof(Server) == _socket.GetType())
                    {
                        _drawer.ScoreDraw(_firstPlayer.Player.Score, _secondPlayer.Player.Score);
                    }
                    else
                    {
                        _drawer.ScoreDraw(_secondPlayer.Player.Score, _firstPlayer.Player.Score);
                    }

                    if (_giftdraw)
                    {
                        _drawer.Draw(_bonus);
                        if (GetGiftCol())
                        {
                            if (typeof(Client) == _socket.GetType())
                            {
                                ((Client)_socket).SetBonus(false);
                            }
                        };
                    }
                    else
                    {
                        if (typeof(Server) == _socket.GetType())
                        {
                            GiftSpawn();
                        }
                    }
                }
                else
                {
                    _drawer.ServeDraw();
                }

                PlayerReturnPos(_firstPlayer);
                PlayerToNormal(_firstPlayer);

                BallCol();
                BallColWalls();
                BallIsOut();

                if (typeof(Server) == _socket.GetType())
                {
                    // Передвижение шарика
                    if (!_serveraffle && !_serve)
                    {
                        PingPongServe();
                    }
                    else
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            _ball.Ball.Move();
                        }
                    }
                }

                _target.EndDraw();
            }

            Recive();
            Send();

            if (_dInput.KeyboardUpdated)
            {
                _dInput.UpdateKeyboardState();
                if (_dInput.KeyboardState.IsPressed(Key.Escape) && _gameEnd == true)
                {
                    RenderForm.Close();

                    GameReturn = true;
                }
            }
        }

        private void PlayerToNormal(PlayerGame player)
        {
            if (player.Time <= _timeHelper.Time && player.Player.GiftLimit)
            {
                player = new NormalPlayer(player);
            }
        }
        private void PlayerReturnPos(PlayerGame player)
        {
            if (!player.Player.Gorisont)
            {
                for (int i = 0; i < 10 && player.Player.GetReturnPosition(); i++) { }
            }
        }
        private void ControlMethod(Key keyUp, Key keyDown, Key keyLeft, Key keyRight, PlayerGame player)
        {
            if (_dInput.KeyboardState.IsPressed(keyUp) && !GetPlayerColWalls(player, Direction.Up))
            {
                for (int i = 0; i < 10 && !GetPlayerColWalls(player, Direction.Up); i++)
                {
                    player.Player.Move(Direction.Up);
                }
            }
            if (_dInput.KeyboardState.IsPressed(keyDown) && !GetPlayerColWalls(player, Direction.Down))
            {
                for (int i = 0; i < 10 && !GetPlayerColWalls(player, Direction.Down); i++)
                {
                    player.Player.Move(Direction.Down);
                }
            }
            if (_dInput.KeyboardState.IsPressed(keyLeft) && !GetPlayerColWalls(player, Direction.Left) && player.Player.Gorisont)
            {
                for (int i = 0; i < 10 && !GetPlayerColWalls(player, Direction.Left); i++)
                {
                    player.Player.Move(Direction.Left);
                }
            }
            if (_dInput.KeyboardState.IsPressed(keyRight) && !GetPlayerColWalls(player, Direction.Right) && player.Player.Gorisont)
            {
                for (int i = 0; i < 10 && !GetPlayerColWalls(player, Direction.Right); i++)
                {
                    player.Player.Move(Direction.Right);
                }
            }
        }

        /// <summary>
        /// Реализована логика привязки шарика к игроку, который будет подавать
        /// </summary>
        private void PingPongServe()
        {
            if (_firstPlayer.Player.TurnServe)
            {
                if (typeof(Server) == _socket.GetType())
                {
                    _ball.SetPosition(new Vector2(_firstPlayer.Rect.Right + _ball.Rect.Width / 2, _firstPlayer.Rect.Center.Y));
                }
                else
                {
                    _ball.SetPosition(new Vector2(_firstPlayer.Rect.Left - _ball.Rect.Width / 2, _firstPlayer.Rect.Center.Y));
                }

                _firstPlayer.Player.Gorisont = true;
            }
            else if (_secondPlayer.Player.TurnServe)
            {
                if (typeof(Server) == _socket.GetType())
                {
                    _ball.SetPosition(new Vector2(_secondPlayer.Rect.Left - _ball.Rect.Width / 2, _secondPlayer.Rect.Center.Y));
                }
                else
                {
                    _ball.SetPosition(new Vector2(_secondPlayer.Rect.Right + _ball.Rect.Width / 2, _secondPlayer.Rect.Center.Y));
                }

                _secondPlayer.Player.Gorisont = true;
            }
            _ball.Ball.Speed = 1;
            //_ball.Ball.Speed = 0;
            //_ball.Ball.Direction = new Vector2(0, 0);
            //_firstPlayer.Player.ServeMoving();
        }

        /// <summary>
        /// Реализуется логика коллизии игрока со стенами
        /// </summary>
        /// <param name="player">Объект класса PlayerGame</param>
        /// <param name="key">Направление движения игрока</param>
        /// <returns>Возвращает столкновение игрока со стеной</returns>
        private bool GetPlayerColWalls(PlayerGame player, Direction key)
        {
            RectangleF pl = new RectangleF();
            switch (key)
            {
                case Direction.Up:
                    pl = new RectangleF(player.Rect.Left, player.Rect.Top - 1, player.Rect.Width, player.Rect.Height);
                    break;
                case Direction.Down:
                    pl = new RectangleF(player.Rect.Left, player.Rect.Top + 1, player.Rect.Width, player.Rect.Height);
                    break;
            }
            /*_dx2d.RenderTarget.DrawRectangle(pl, DX2D.RedBrush);*/
            if (pl.Intersects(_topWall.Rect)) { player.Player.Move(Direction.Down); return true; }
            if (pl.Intersects(_botWall.Rect)) { player.Player.Move(Direction.Up); return true; }
            return false;
        }
        /// <summary>
        /// Реализует коллизию шарика со стенами
        /// </summary>
        private void BallColWalls()
        {
            if (_ball.Rect.Intersects(_topWall.Rect)) { _physicsEngine.UpdateBall(_ball); }
            if (_ball.Rect.Intersects(_botWall.Rect)) { _physicsEngine.UpdateBall(_ball); }
        }
        /// <summary>
        /// Определяет коллизию шарика с игроком
        /// </summary>
        private void BallCol()
        {
            /*_dx2d.RenderTarget.DrawRectangle(pl, DX2D.RedBrush);*/
            if (_ball.Rect.Intersects(_firstPlayer.Rect)) { _ball.Ball.Direction = (typeof(Server) == _socket.GetType()) ? new Vector2(1, 0) : new Vector2(-1, 0); _physicsEngine.UpdatePlayer(_ball, _firstPlayer); }
            if (_ball.Rect.Intersects(_secondPlayer.Rect)) { _ball.Ball.Direction = (typeof(Server) == _socket.GetType()) ? new Vector2(-1, 0) : new Vector2(1, 0); _physicsEngine.UpdatePlayer(_ball, _secondPlayer); }
        }
        /// <summary>
        /// Реализует логику коллизии игрока и бонуса
        /// </summary>
        /// <returns>Возвращает столкновение игрока с бонусом</returns>
        private bool GetGiftCol()
        {
            if (_bonus.Rect.Intersects(_firstPlayer.Rect)) { _giftdraw = false; GiftUsage((_bonus.Bonus.Type == TypeBonus.Small || _bonus.Bonus.Type == TypeBonus.LowSpeed) ? _secondPlayer : _firstPlayer); return true; }
            if (_bonus.Rect.Intersects(_secondPlayer.Rect)) { _giftdraw = false; GiftUsage((_bonus.Bonus.Type == TypeBonus.Small || _bonus.Bonus.Type == TypeBonus.LowSpeed) ? _firstPlayer : _secondPlayer); return true; }
            return false;
        }

        /// <summary>
        /// Запускает таймер действия бонуса, а также определяет его тип
        /// </summary>
        /// <param name="player">Объект класса PlayerGame</param>
        private void GiftUsage(PlayerGame player)
        {
            player.Time = _timeHelper.Time + 5f; player.Player.GiftOn();
            switch (_bonus.Bonus.Type)
            {
                case TypeBonus.Big:
                    player = new Entity.PlayerDecorator.BigPlayer(player);
                    break;
                case TypeBonus.HighSpeed:
                    player = new Entity.PlayerDecorator.HighSpeedPlayer(player);
                    break;
                case TypeBonus.LowSpeed:
                    player = new Entity.PlayerDecorator.LowSpeedPlayer(player);
                    break;
                case TypeBonus.Small:
                    player = new Entity.PlayerDecorator.SmallPlayer(player);
                    break;
                case TypeBonus.MoveX:
                    player = new Entity.PlayerDecorator.MoveXPlayer(player);
                    break;
            }
        }
        /// <summary>
        /// Определяет вышел ли мяч за пределы поля или нет
        /// </summary>
        private void BallIsOut()
        {
            if (_ball.Rect.Center.X < 0)
            {
                if (!_serveraffle)
                {
                    if (typeof(Server) == _socket.GetType())
                        _secondPlayer.Player.Goal();
                    else
                        _firstPlayer.Player.Goal();
                    _serve = false;
                    ChoiceServe();
                }
                else
                {
                    _serveraffle = false;
                    if (typeof(Server) == _socket.GetType())
                        _secondPlayer.Player.Serve();
                    else
                        _firstPlayer.Player.Serve();
                }
                PingPongServe();
            }
            if (_ball.Rect.Center.X > _targetSize.Width)
            {
                if (!_serveraffle)
                {
                    if (typeof(Server) == _socket.GetType())
                        _firstPlayer.Player.Goal();
                    else
                        _secondPlayer.Player.Goal();
                    _serve = false;
                    ChoiceServe();
                }
                else
                {
                    _serveraffle = false;
                    if (typeof(Server) == _socket.GetType())
                        _firstPlayer.Player.Serve();
                    else
                        _secondPlayer.Player.Goal();
                }
                PingPongServe();
            }
        }
        /// <summary>
        /// Реализует логику определения очереди подачи
        /// </summary>
        private void ChoiceServe()
        {
            if (_firstPlayer.Player.Score >= 10 && _secondPlayer.Player.Score >= 10)
            {
                if (_secondPlayer.Player.TurnServe)
                {
                    _ball.Ball.Direction = new Vector2(1, 0);
                    _secondPlayer.Player.Serve();
                    _firstPlayer.Player.Serve();
                }
                else
                {
                    _ball.Ball.Direction = new Vector2(-1, 0);
                    _secondPlayer.Player.Serve();
                    _firstPlayer.Player.Serve();
                }
            }
            else if ((_firstPlayer.Player.Score + _secondPlayer.Player.Score) > 0)
            {
                if (_secondPlayer.Player.TurnServe)
                {
                    _ball.Ball.Direction = new Vector2(1, 0);
                    if ((_firstPlayer.Player.Score + _secondPlayer.Player.Score) % 2 == 0)
                    {
                        _secondPlayer.Player.Serve();
                        _firstPlayer.Player.Serve();
                    }
                }
                else
                {
                    _ball.Ball.Direction = new Vector2(-1, 0);
                    if ((_firstPlayer.Player.Score + _secondPlayer.Player.Score) % 2 == 0)
                    {
                        _secondPlayer.Player.Serve();
                        _firstPlayer.Player.Serve();
                    }
                }
            }

            if (typeof(Server) == _socket.GetType())
            {
                if (_secondPlayer.Player.TurnServe)
                {
                    ((Server)_socket).Send($"TurnServer {_secondPlayer.Player.TurnServe}");
                }
            }
        }
        /// <summary>
        /// Содержится логика по случайной генерации бонуса
        /// </summary>
        private void GiftSpawn()
        {
            if (_rnd.Next(1000) == 100)
            {
                switch (_rnd.Next(2))
                {
                    case 0: if (!_firstPlayer.Player.GiftLimit) ChoiceGift(_firstPlayer); break;
                    case 1: if (!_secondPlayer.Player.GiftLimit) ChoiceGift(_secondPlayer); break;
                }
            }
        }

        /// <summary>
        /// Метод, реализующий случайный выбор генерируемого бонуса
        /// </summary>
        /// <param name="player">Объект класса PlayerGame</param>
        private void ChoiceGift(PlayerGame player)
        {
            Vector2 position = new Vector2(player.Player.Position.X, _rnd.Next((int)_targetSize.Height - 200) + 100);
            switch (_rnd.Next(5))
            {
                case 0:
                    _bonusFactory = new BonusBigPlayer(position);
                    _generator = _bonusFactory.GetBonus();
                    _bonus = new BonusGame(_generator.GetGenerator(), new Sprite(_giftIndex, 32, 32, 0));
                    break;
                case 1:
                    _bonusFactory = new BonusSmallPlayer(position);
                    _generator = _bonusFactory.GetBonus();
                    _bonus = new BonusGame(_generator.GetGenerator(), new Sprite(_giftIndex, 32, 32, 0));
                    break;
                case 2:
                    _bonusFactory = new BonusHighSpeedPlayer(position);
                    _generator = _bonusFactory.GetBonus();
                    _bonus = new BonusGame(_generator.GetGenerator(), new Sprite(_giftIndex, 32, 32, 0));
                    break;
                case 3:
                    _bonusFactory = new BonusLowSpeedPlayer(position);
                    _generator = _bonusFactory.GetBonus();
                    _bonus = new BonusGame(_generator.GetGenerator(), new Sprite(_giftIndex, 32, 32, 0));
                    break;
                case 4:
                    _bonusFactory = new BonusMoveXPlayer(position);
                    _generator = _bonusFactory.GetBonus();
                    _bonus = new BonusGame(_generator.GetGenerator(), new Sprite(_giftIndex, 32, 32, 0));
                    break;
            }
            _giftdraw = true;
        }
        /// <summary>
        /// Реализует логику окончания игры
        /// </summary>
        private void EndGame()
        {
            bool flag = true;
            if (_firstPlayer.Player.Score >= 11 && (_firstPlayer.Player.Score - _secondPlayer.Player.Score) >= 2 || _firstPlayer.Player.Score == 7 && _secondPlayer.Player.Score == 0)
            {

                _drawer.ResultDraw((typeof(Server) == _socket.GetType()) ? flag : flag);
                _gameEnd = true;
            }

            if (_secondPlayer.Player.Score >= 11 && (_secondPlayer.Player.Score - _firstPlayer.Player.Score) >= 2 || _secondPlayer.Player.Score == 7 && _firstPlayer.Player.Score == 0)
            {
                flag = false;

                _drawer.ResultDraw((typeof(Server) == _socket.GetType()) ? flag : flag);
                _gameEnd = true;
            }
        }

        private void Recive()
        {
            if (typeof(Server) == _socket.GetType())
            {
                _secondPlayer.SetInfo(((Server)_socket).PlayerPosition);
            }
            else
            {
                _secondPlayer.SetInfo(((Client)_socket).PlayerPosition);
                _ball.SetInfo(((Client)_socket).BallPosition);

                _serveraffle = ((Client)_socket).Serveraffle;

                if (!((Client)_socket).IsBonus && string.IsNullOrEmpty(((Client)_socket).Bonus))
                {
                    Vector2 position = new Vector2(0, 0);
                    _bonusFactory = new BonusBigPlayer(position);
                    _generator = _bonusFactory.GetBonus();
                    _bonus = new BonusGame(_generator.GetGenerator(), new Sprite(_giftIndex, 32, 32, 0));

                    _bonus.SetInfo(((Client)_socket).Bonus);
                    _giftdraw = true;

                    ((Client)_socket).SetBonus(true);
                }
                else
                {
                    _giftdraw = false;
                }

                if (((Client)_socket).TurnServe)
                {
                    _firstPlayer.Player.SetServe(((Client)_socket).TurnServe);
                }
            }
        }

        private void Send()
        {
            if (typeof(Server) == _socket.GetType())
            {
                ((Server)_socket).Send($"Ball {_ball.GetInfo()}");
                ((Server)_socket).Send($"PlayerPosition {_firstPlayer.GetInfo()}");
                ((Server)_socket).Send($"Serveraffle {_serveraffle}");

                if (_giftdraw && _bonus != null)
                {
                    ((Server)_socket).Send($"Bonus {_bonus.GetInfo()}");
                }
                else
                {
                    ((Server)_socket).Send($"Bonus");
                }
            }
            else
            {
                ((Client)_socket).Send($"PlayerPosition {_firstPlayer.GetInfo()}");
            }
        }

        /// <summary>
        /// Определяет масштабируемость игрового поля относительно изменения раз-мера игрового окна
        /// </summary>
        /// <param name="sender">Объект</param>
        /// <param name="e">Событие</param>
        private void RenderForm_Resize(object sender, EventArgs e)
        {
            // При ресайзинге обновляем размер области отображения и масштаб
            int width = RenderForm.ClientSize.Width;
            int height = RenderForm.ClientSize.Height;
            DX2D.RenderTarget.Resize(new Size2(width, height));
            _clientRect.Width = DX2D.RenderTarget.Size.Width;
            _clientRect.Height = DX2D.RenderTarget.Size.Height;
            Scale = _clientRect.Height / _unitsPerHeight;
        }
        /// <summary>
        /// Отображает игровое поле и вызывает метод RenderCallback
        /// </summary>
        public void Run()
        {
            RenderForm.Resize += RenderForm_Resize;
            RenderLoop.Run(RenderForm, RenderCallback);
        }
        /// <summary>
        /// Освобождает неуправляемые ресурсы игрового приложения
        /// </summary>
        public void Dispose()
        {
            _dInput.Dispose();
            DX2D.Dispose();
            RenderForm.Dispose();
        }
    }
}