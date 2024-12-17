using _PingPongLibrary._GameObject;
using PingPongLibrary.Entity;
using SharpDX;
using System.Collections.Generic;
using System.Text;

namespace PingPongLibrary.GameObject
{
    /// <summary>
    /// Класс, реализующий объект бонуса
    /// </summary>
    public class BonusGame : GameObject2
    {
        /// <summary>
        /// Позволяет взаимодействовать с логикой данного игрового объекта
        /// </summary>
        public Bonus Bonus { get; private set; }
        public BonusGame(Bonus gameObject, Sprite sprites) : base(gameObject, new List<Sprite>() { sprites })
        {
            Bonus = gameObject;
            _scale = 1;
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{Bonus.PositionOfCenter.X},{Bonus.PositionOfCenter.Y};{(int)Bonus.Type}");
            return sb.ToString();
        }

        public void SetInfo(string str)
        {
            string[] parts = str.Split(';');

            if(parts.Length == 2)
            {
                string[] positionString = parts[0].Split(',');
                if(positionString.Length == 2)
                {
                    Bonus.SetPosition(new Vector2(int.Parse(positionString[0]), int.Parse(positionString[1])));
                    Bonus.SetType(int.Parse(parts[1]));
                }
            }
        }
    }
}