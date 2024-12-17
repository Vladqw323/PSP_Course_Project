using SharpDX;

namespace PingPongLibrary.Entity
{
    /// <summary>
    /// Перечисление типов бонусов
    /// </summary>
    public enum TypeBonus
    {
        Big,
        HighSpeed,
        LowSpeed,
        Small,
        MoveX
    }
    /// <summary>
    /// Класс, реализующий графическое представление бонуса
    /// </summary>
    public class Bonus : GameObject
    {
        /// <summary>
        /// Тип бонуса
        /// </summary>
        public TypeBonus Type { get; private set; }
        public Bonus(Vector2 position, TypeBonus type) : base(position)
        {
            Type = type;
        }

        public void SetPosition(Vector2 position)
        {
            PositionOfCenter = position;
        }

        public void SetType(int id)
        {
            switch(id)
            {
                case (int)TypeBonus.Big: Type = TypeBonus.Big; break;
                case (int)TypeBonus.HighSpeed: Type = TypeBonus.HighSpeed; break;
                case (int)TypeBonus.LowSpeed: Type = TypeBonus.LowSpeed; break;
                case (int)TypeBonus.Small: Type = TypeBonus.Small; break;
                case (int)TypeBonus.MoveX: Type = TypeBonus.MoveX; break;
            }
        }
    }
}