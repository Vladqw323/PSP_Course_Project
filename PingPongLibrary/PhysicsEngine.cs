using _PingPongLibrary._GameObject;
using SharpDX;
using System;

namespace PingPongLibrary
{
    /// <summary>
    /// Класс, реализующий логику физики игры, то есть логика отскоков шарика от стен и игроков
    /// </summary>
    public class PhysicsEngine
    {
        /// <summary>
        /// Рассчитывает и обновляет угол отскока шарика от игрока
        /// </summary>
        /// <param name="ball">Объект класса BallGame</param>
        /// <param name="player">Объект класса PlayerGame</param>
        public void UpdatePlayer(BallGame ball, PlayerGame player)
        {
            // определяем на какую часть ракетки попал мяч
            float paddleHeight = player.Rect.Height;
            float paddleTop = player.Player.PositionOfCenter.Y - paddleHeight / 2f;
            float ballHeight = ball.Rect.Height;
            float ballTop = ball.Ball.PositionOfCenter.Y - ballHeight / 2f;
            float distanceFromTop = ballTop - paddleTop;
            float relativeIntersectY = distanceFromTop / paddleHeight * 2f;
            float angle = MathUtil.DegreesToRadians(45f) * relativeIntersectY;
            // вводим ограничения на угол движения мяча
            if (angle >= 1.3f) { angle = 1.3f; }
            if (angle <= -1.3f) { angle = -1.3f; }
            // изменяем направление движения мяча
            if (ball.Ball.Direction == new Vector2(-1, 0))
            {
                ball.Ball.Direction = new Vector2(-(float)Math.Cos(angle), (float)Math.Sin(angle));
            }
            else
            {
                ball.Ball.Direction = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            }

            // увеличиваем скорость мяча
            ball.Ball.Speed += 0.10f;
            //MessageBox.Show(angle.ToString());
        }
        /// <summary>
        /// Обновляет угол отскока шарика от стен
        /// </summary>
        /// <param name="ball">Объект класса BallGame</param>
        public void UpdateBall(BallGame ball)
        {
            ball.Ball.Direction = new Vector2(ball.Ball.Direction.X, -ball.Ball.Direction.Y);
        }
    }
}