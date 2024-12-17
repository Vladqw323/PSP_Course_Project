using PingPongLibrary.Entity;
using SharpDX;
using System.Collections.Generic;
using System.Text;

namespace _PingPongLibrary._GameObject
{
    /// <summary>
    /// Класс, реализующий объект игрока
    /// </summary>
    public class PlayerGame : GameObject2
    {
        /// <summary>
        /// Позволяет взаимодействовать с логикой данного игрового объекта
        /// </summary>
        public Player Player { get; private set; }
        /// <summary>
        /// Служит для определения времени действия бонуса на игрока
        /// </summary>
        public float Time { get; set; }
        public PlayerGame(Player gameObject, List<Sprite> sprites) : base(gameObject, sprites) { Player = gameObject; }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Player.PositionOfCenter.X},{Player.PositionOfCenter.Y};{ActiveSprite.Width},{ActiveSprite.Heigth};{Player.Score};{Player.TurnServe}");

            return sb.ToString();   
        }

        public void SetInfo(string str)
        {
            string[] parts = str.Split(';');

            if (parts.Length == 4)
            {
                string[] positionParts = parts[0].Split(',');
                string[] spriteSetting = parts[1].Split(',');

                if (positionParts.Length == 2 && spriteSetting.Length == 2)
                {
                    Player.SetPosition(new Vector2(float.Parse(positionParts[0]), float.Parse(positionParts[1])));
                    ActiveSprite.Resize(new Size2(int.Parse(spriteSetting[0]), int.Parse(spriteSetting[1])));
                    Player.SetScore(int.Parse(parts[2]));
                    Player.SetServe(parts[3].Equals("True"));
                }
            }
        }
    }
}