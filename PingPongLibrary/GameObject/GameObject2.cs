using PingPongLibrary.Entity;
using SharpDX;
using System.Collections.Generic;


namespace _PingPongLibrary._GameObject
{
    /// <summary>
    /// Абстрактный класс, для обобщённого отображения игровых объектов
    /// </summary>
    public abstract class GameObject2
    {
        protected GameObject _gameObject;
        /// <summary>
        /// Описывает класс игрового объекта
        /// </summary>
        public GameObject gameObject { get => _gameObject; }
        protected List<Sprite> _sprites;
        protected Sprite _activeSprite;
        protected Vector2 _positionOfRotation;
        /// <summary>
        /// Точка, относительно которой спрайт будет вращаться
        /// </summary>
        public Vector2 PositionOfRotation { get { return ActiveSprite.Center; } set => _positionOfRotation = value; }
        /// <summary>
        /// Логика спрайта, его ширина, высота, поворот и номер необходи-мого спрайта из коллекции спрайтов, что хранится в классе DX2D
        /// </summary>
        public Sprite ActiveSprite { get => _activeSprite; set => _activeSprite = value; }
        protected RectangleF _rect;
        /// <summary>
        /// Определяет прямоугольник объекта
        /// </summary>
        public RectangleF Rect { get => GetRect(); }

        protected float _scale;
        /// <summary>
        /// Представляет собой масштабирование объекта при отрисовки его спрайта на экране
        /// </summary>
        public float Scale { get => _scale; }

        private Size2 _size;
        public GameObject2(GameObject gameObject, List<Sprite> sprites)
        {
            _gameObject = gameObject;
            _sprites = new List<Sprite>();
            foreach (var sprite in sprites)
            {
                _sprites.Add(sprite);
            }
            _activeSprite = _sprites[0];
            _size = new Size2(15, 80);
        }
        /// <summary>
        /// Рассчитывает точку верхнего левого угла прямоугольника в виде разницы между позиционированием объекта на игровом поле и центром отображен-ного спрайта
        /// </summary>
        /// <returns>Возвращает прямоугольник объекта</returns>
        RectangleF GetRect()
        {
            _rect = new RectangleF(_gameObject.PositionOfCenter.X - ActiveSprite.Center.X, _gameObject.PositionOfCenter.Y - ActiveSprite.Center.Y, _activeSprite.Width, _activeSprite.Heigth);
            return _rect;
        }
        /// <summary>
        /// Предназначен для смены размеров спрайта у игрового объекта
        /// </summary>
        /// <param name="size">Новый размер спрайта</param>
        public void Resize(Size2 size)
        {
            foreach (var sprite in _sprites)
            {
                sprite.Resize(size);
            }
        }
        /// <summary>
        /// Предназначен для смены размеров спрайта у игрового объекта на определённый размер
        /// </summary>
        public void NormalSize()
        {
            foreach (var sprite in _sprites)
            {
                sprite.Resize(_size);
            }
        }
    }
}