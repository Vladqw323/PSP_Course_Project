using _PingPongLibrary._GameObject;
using PingPongLibrary.Entity;
using System.Collections.Generic;

namespace PingPongLibrary.GameObject
{
    /// <summary>
    /// Класс, реализующий объект заднего фона
    /// </summary>
    public class BackgroundGame : GameObject2
    {
        public BackgroundGame(Background gameObject, Sprite sprite) : base(gameObject, new List<Sprite>() { sprite })
        {
            _scale = 1;
        }
    }
}