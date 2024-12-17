using SharpDX;
using System;

namespace PingPongLibrary.Entity
{
    /// <summary>
    /// Класс, представляющий сам спрайт игрового объекта
    /// </summary>
    public class Sprite
    {
        private static readonly float _Pi = (float)Math.PI;
        private static readonly float _2Pi = 2.0f * (float)Math.PI;

        private float _angle;
        /// <summary>
        /// Описывает угол поворота загружаемого спрайта
        /// </summary>
        public float Angle
        {
            get => _angle;
            set
            {
                _angle = value;
                if (_angle > _Pi) _angle -= _2Pi;
                else if (_angle < -_Pi) _angle += _2Pi;
            }
        }

        // Индекс картинки спрайта в коллекции
        /// <summary>
        /// Указывает на номер спрайта, загруженного в метод LoadBitmap класса DX2D
        /// </summary>
        public int BitmapIndex { get; private set; }
        /// <summary>
        /// Ширина спрайта в пикселях
        /// </summary>
        public float Width { get; private set; }
        /// <summary>
        /// Высота спрайта в пикселях
        /// </summary>
        public float Heigth { get; private set; }
        /// <summary>
        /// Для увеличения или уменьшения загруженного спрайта
        /// </summary>
        public Size2 Size { get => new Size2((int)Width, (int)Heigth); }
        // Положение центра спрайта в пикселях относительно верхнего левого края картинки
        private Vector2 _center;
        /// <summary>
        /// Описывает центр спрайта, как точку центра высоты и ширины
        /// </summary>
        public Vector2 Center { get => _center; }
        public Sprite(int bitmapIndex, int width, int heigth, float angle)
        {
            BitmapIndex = bitmapIndex;
            _angle = angle;
            Width = width;
            Heigth = heigth;

            _center.X = Width / 2.0f;
            _center.Y = Heigth / 2.0f;
        }
        /// <summary>
        /// Предназначен для смены размеров спрайта у игрового объекта
        /// </summary>
        /// <param name="size">Новый размер спрайта</param>
        public void Resize(Size2 size)
        {
            Width = size.Width;
            Heigth = size.Height;
        }
    }
}