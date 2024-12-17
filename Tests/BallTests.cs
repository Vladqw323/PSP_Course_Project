using Microsoft.VisualStudio.TestTools.UnitTesting;
using PingPongLibrary.Entity;
using SharpDX;

namespace Tests
{
    /// <summary>
    /// Класс тестирования для класса Ball
    /// </summary>
    [TestClass]
    public class BallTests
    {
        /// <summary>
        /// Тестирование метода Move
        /// </summary>
        [TestMethod]
        public void MoveTest()
        {
            Ball ball = new Ball(new SharpDX.Vector2(200, 200));
            Vector2 vector2 = ball.PositionOfCenter;
            ball.Direction = new Vector2(1, 0);
            ball.Move();
            Assert.AreEqual(vector2.X + 1, ball.PositionOfCenter.X);
            vector2 = ball.PositionOfCenter;
            ball.Direction = new Vector2(-1, 0);
            ball.Move();
            Assert.AreEqual(vector2.X - 1, ball.PositionOfCenter.X);
        }
    }
}