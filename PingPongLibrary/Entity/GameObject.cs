using SharpDX;

namespace PingPongLibrary.Entity
{
    /// <summary>
    /// Абстрактный класс, для обобщённой реализации игровых объектов
    /// </summary>
    public abstract class GameObject
    {
        protected Vector2 _positionOfCenter;
        /// <summary>
        /// Поле описывает позиционирование по осям X и Y
        /// </summary>
        public Vector2 PositionOfCenter { get => _positionOfCenter; set => _positionOfCenter = value; }

        public GameObject(Vector2 position)
        {
            _positionOfCenter = position;
        }
    }
}