using _PingPongLibrary._GameObject;
using SharpDX;

namespace PingPongLibrary.Entity
{
    /// <summary>
    /// Перечисление направлений движения игрока
    /// </summary>
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    /// <summary>
    /// Класс, реализующий графическое представление игрока
    /// </summary>
    public class Player : GameObject
    {
        protected float width { get; set; } // ширина ракетки игрока
        protected float height { get; set; } // высота ракетки игрока
        /// <summary>
        /// Счёт игрока
        /// </summary>
        public int Score { get; private set; }
        /// <summary>
        /// Определяет возможность игрока перемещаться по горизонтали, если true то может
        /// </summary>
        public bool Gorisont { get; set; }
        /// <summary>
        /// Первоначальная позиция игрока
        /// </summary>
        public Vector2 Position { get; private set; }
        /// <summary>
        /// Определяет, сможет ли игрок получить бонус, если true, то нет
        /// </summary>
        public bool GiftLimit { get; private set; }
        /// <summary>
        /// Определяет подачу игроков, если true то игрок подаёт 
        /// </summary>
        public bool TurnServe { get; private set; }
        /// <summary>
        /// Поле для реализации скорости игрока
        /// </summary>
        public float Speed { get; set; }

        public Player(Vector2 position) : base(position)
        {
            Position = position;
            Gorisont = false;
            GiftLimit = false;
            width = 5;
            height = 20;
            Speed = 1;
            Score = 0;
            TurnServe = false;
        }
        /// <summary>
        /// Метод, реализующий очередь подач
        /// </summary>
        public void Serve()
        {
            if (TurnServe)
            {
                TurnServe = false;
            }
            else
            {
                TurnServe = true;
            }
        }

        public void SetServe(bool trueServe)
        {
            TurnServe = trueServe;
        }

        /*public void ServeMoving()
        {
            if (PositionOfCenter.X < Position.X + 200)
                _positionOfCenter.X += Speed;
        }*/
        /// <summary>
        /// Метод, реализующий движение игрока
        /// </summary>
        /// <param name="key">Направление движения игрока</param>
        public void Move(Direction key)
        {
            switch (key)
            {
                case Direction.Up:
                    _positionOfCenter.Y -= Speed;
                    break;
                case Direction.Down:
                    _positionOfCenter.Y += Speed;
                    break;
                case Direction.Right:
                    if (Gorisont && PositionOfCenter.X < Position.X + 200)
                        _positionOfCenter.X += Speed;
                    break;
                case Direction.Left:
                    if (Gorisont && PositionOfCenter.X > Position.X - 200)
                        _positionOfCenter.X -= Speed;
                    break;
            }
        }
        /// <summary>
        /// Метод, реализующий подсчёт очков
        /// </summary>
        public void Goal()
        {
            Score++;
        }
        /// <summary>
        /// Метод, реализующий возврат игрока на начальную позицию по оси абсцисс
        /// </summary>
        /// <returns>Непринадлежность игрока своей линии по завершению действия бонуса</returns>
        public bool GetReturnPosition()
        {
            if (PositionOfCenter.X < Position.X)
            {
                _positionOfCenter.X += 1;
                return true;
            }

            if (PositionOfCenter.X > Position.X)
            {
                _positionOfCenter.X -= 1;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Метод, реализующий запрет спавна бонусов
        /// </summary>
        public void GiftOn()
        {
            GiftLimit = true;
        }
        /// <summary>
        /// Метод, реализующий разрешение спавна бонусов
        /// </summary>
        public void GiftOff()
        {
            GiftLimit = false;
        }

        public void SetPosition(Vector2 newPosition)
        {
            _positionOfCenter = newPosition;
        }

        public void SetScore(int score)
        {
            Score = score;
        }
    }
}