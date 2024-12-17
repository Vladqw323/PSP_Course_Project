using _PingPongLibrary._GameObject;
using PingPongLibrary.Entity;
using System.Collections.Generic;

namespace PingPongLibrary.GameObject
{
    /// <summary>
    /// Класс, реализующий объект стены
    /// </summary>
    public class WallGame : GameObject2
    {
        /// <summary>
        /// Позволяет взаимодействовать с логикой данного игрового объекта
        /// </summary>
        public Wall Wall { get; private set; }
        public WallGame(Wall gameObject, List<Sprite> sprites) : base(gameObject, sprites) { Wall = gameObject; _scale = 1; }
    }
}