using PingPongLibrary.Entity;
using SharpDX;
using System.Collections.Generic;
using System.Text;

namespace _PingPongLibrary._GameObject
{
    /// <summary>
    /// Класс, реализующий объект мячика
    /// </summary>
    public class BallGame : GameObject2
    {
        /// <summary>
        /// Позволяет взаимодействовать с логикой данного игрового объекта
        /// </summary>
        public Ball Ball { get; private set; }
        public BallGame(Ball gameObject, List<Sprite> sprites) : base(gameObject, sprites) { Ball = gameObject; _scale = 1; }

        /// <summary>
        /// Метод, реализующий привязку мяча к игроку в момент подачи
        /// </summary>
        /// <param name="rect">Центр игрока для дальнейшей привязки к нему мяча</param>
        public void SetPosition(Vector2 rect)
        {
            Ball.PositionOfCenter = rect;
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"{Ball.PositionOfCenter.X};{Ball.PositionOfCenter.Y}");

            return sb.ToString();
        }

        public void SetInfo(string pos)
        {
            string[] coord = pos.Split(';');
            Vector2 vec = new Vector2(float.Parse(coord[0]), float.Parse(coord[1]));
            Ball.SetPosition(vec);
        }
    }
}