using SharpDX;

namespace PingPongLibrary.Entity
{
    /// <summary>
    /// Класс, реализующий графическое представление стены
    /// </summary>
    public class Wall : GameObject
    {
        public Wall(Vector2 position) : base(position)
        {
        }
    }
}