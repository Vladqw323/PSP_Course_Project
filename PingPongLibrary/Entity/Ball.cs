using SharpDX;
using System;

namespace PingPongLibrary.Entity
{
    /// <summary>
    /// Класс, реализующий графическое представление шарика
    /// </summary>
    public class Ball : GameObject
    {
        protected float width { get; set; }
        protected float height { get; set; }
        /// <summary>
        /// Направление движения мячика
        /// </summary>
        public Vector2 Direction;
        private Random _rnd;

        /// <summary>
        /// Поле для реализации скорости мяча
        /// </summary>
        public float Speed { get; set; }

        public Ball(Vector2 position) : base(position)
        {
            width = 5;
            height = 5;
            Speed = 1;
            _rnd = new Random();
            switch (_rnd.Next(2))
            {
                case 0: Direction = new Vector2(-1, 0); break;
                case 1: Direction = new Vector2(1, 0); break;
            }
        }

        /// <summary>
        /// Метод, реализующий передвижение шарика по игровому полю
        /// </summary>
        public void Move()
        {
            PositionOfCenter += Direction * Speed;
        }

        public void SetPosition(Vector2 vector)
        {
            PositionOfCenter = vector;
        }
    }
}