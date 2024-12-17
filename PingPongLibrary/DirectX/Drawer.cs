using _PingPongLibrary._GameObject;
using SharpDX;
using SharpDX.Direct2D1;
using System;

namespace PingPongLibrary.DirectX
{
    /// <summary>
    /// Класс, реализующий отрисовку игровых объектов
    /// </summary>
    public class Drawer
    {
        private DX2D _dx2d;
        private Vector2 _translation;
        private Matrix3x2 _matrix;
        Matrix3x2 last;

        public Drawer(DX2D dx2d)
        {
            _dx2d = dx2d;
        }
        /// <summary>
        /// Метод, реализующий отображение статических игровых объектов
        /// </summary>
        /// <param name="gameObject">Объект класса GameObject2</param>
        /// <exception cref="Exception">Обработка исключения нулевого индекса</exception>
        public void DrawObject(GameObject2 gameObject)
        {
            if (_dx2d.Bitmaps[gameObject.ActiveSprite.BitmapIndex] == null) throw new Exception("bitmap was null");
            Bitmap bmp = _dx2d.Bitmaps[gameObject.ActiveSprite.BitmapIndex];
            RectangleF rect = gameObject.Rect;
            _dx2d.RenderTarget.DrawBitmap(_dx2d.Bitmaps[gameObject.ActiveSprite.BitmapIndex],
                                       rect,
                                       1,
                                       SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
        }
        /// <summary>
        /// Метод, реализующий отображение динамических игровых объектов
        /// </summary>
        /// <param name="gameObject">Объект класса GameObject2</param>
        public void Draw(GameObject2 gameObject)
        {
            float scale = gameObject.Scale;
            _translation.X = (gameObject.gameObject.PositionOfCenter.X - gameObject.ActiveSprite.Center.X);
            _translation.Y = (gameObject.gameObject.PositionOfCenter.Y - gameObject.ActiveSprite.Center.Y);
            //   Порядок перемножения матриц в "прямом иксе 2 измерения" перевернут с ног на голову:
            //   слева - вращение и масштаб (в произвольном порядке между собой), справа - перенос
            _matrix = Matrix3x2.Rotation(gameObject.ActiveSprite.Angle, gameObject.PositionOfRotation) *
                Matrix3x2.Scaling(scale, scale, Vector2.Zero) *
                Matrix3x2.Translation(_translation);

            // Получаем из инфраструктурного объекта "цель" отрисовки
            WindowRenderTarget r = _dx2d.RenderTarget;
            last = r.Transform;
            // Устанавливаем матрицу координатных преобразований
            r.Transform = _matrix;

            // Рисовка
            int opacity = 1;
            Bitmap bitmap = _dx2d.Bitmaps[gameObject.ActiveSprite.BitmapIndex];
            r.DrawBitmap(bitmap, opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
            r.Transform = last;
        }
        /// <summary>
        /// Метод, который отображает текущий счёт игроков
        /// </summary>
        /// <param name="score1">Счёт первого игрока</param>
        /// <param name="score2">Счёт второго игрока</param>
        public void ScoreDraw(int score1, int score2)
        {
            /*_dx2d.RenderTarget.DrawRectangle(new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 90, 0, 80, 50), _dx2d.QuanBrush);
            _dx2d.RenderTarget.DrawRectangle(new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 10, 0, 20, 50), _dx2d.QuanBrush);
            _dx2d.RenderTarget.DrawRectangle(new RectangleF((_dx2d.RenderTarget.Size.Width / 2) + 10, 0, 80, 50), _dx2d.QuanBrush);*/

            _dx2d.RenderTarget.DrawText($"{score1}", _dx2d.TextFormatStats, new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 90, 0, 80, 50), _dx2d.QuanBrush);
            _dx2d.RenderTarget.DrawText(":", _dx2d.TextFormatStats, new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 10, 0, 20, 50), _dx2d.QuanBrush);
            _dx2d.RenderTarget.DrawText($"{score2}", _dx2d.TextFormatStats, new RectangleF((_dx2d.RenderTarget.Size.Width / 2) + 10, 0, 80, 50), _dx2d.QuanBrush);
        }
        /// <summary>
        /// Метод, который отображает результат игры
        /// </summary>
        /// <param name="flag">Победа первого игрока</param>
        public void ResultDraw(bool flag)
        {
            /*_dx2d.RenderTarget.DrawRectangle(new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 90, 0, 80, 50), _dx2d.QuanBrush);
            _dx2d.RenderTarget.DrawRectangle(new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 10, 0, 20, 50), _dx2d.QuanBrush);
            _dx2d.RenderTarget.DrawRectangle(new RectangleF((_dx2d.RenderTarget.Size.Width / 2) + 10, 0, 80, 50), _dx2d.QuanBrush);*/
            if (flag)
            {
                _dx2d.RenderTarget.DrawText($"Player 1 is won", _dx2d.TextFormatEndGame, new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 750, (_dx2d.RenderTarget.Size.Height / 2) - 150, 1500, 150), _dx2d.QuanBrush);
            }
            else
            {
                _dx2d.RenderTarget.DrawText($"Player 2 is won", _dx2d.TextFormatEndGame, new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 750, (_dx2d.RenderTarget.Size.Height / 2) - 150, 1500, 150), _dx2d.QuanBrush);
            }
            _dx2d.RenderTarget.DrawText($"~Press ESC to start new game~", _dx2d.TextFormatStartNewGame, new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 750, (_dx2d.RenderTarget.Size.Height / 2) + 35, 1500, 75), _dx2d.RedBrush);
        }
        /// <summary>
        /// Метод, который отображает разыгровку перед началом игры
        /// </summary>
        public void ServeDraw()
        {
            //_dx2d.RenderTarget.DrawRectangle(new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 150, 0, 300, 50), _dx2d.QuanBrush);

            _dx2d.RenderTarget.DrawText($"Разыгровка", _dx2d.TextFormatStats, new RectangleF((_dx2d.RenderTarget.Size.Width / 2) - 150, 0, 300, 50), _dx2d.QuanBrush);
        }
    }
}