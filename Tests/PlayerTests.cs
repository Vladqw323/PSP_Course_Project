using Microsoft.VisualStudio.TestTools.UnitTesting;
using PingPongLibrary.Entity;
using SharpDX;

namespace Tests
{
    /// <summary>
    /// Класс тестирования класса Player
    /// </summary>
    [TestClass]
    public class PlayerTests
    {
        /// <summary>
        /// Тест метода Serve
        /// </summary>
        [TestMethod]
        public void ServeTest()
        {
            Player player = new Player(new SharpDX.Vector2(200, 200));
            Assert.IsFalse(player.TurnServe);

            player.Serve();
            Assert.IsTrue(player.TurnServe);
        }
        /// <summary>
        /// Тест метода Move
        /// </summary>
        [TestMethod]
        public void MoveTest()
        {
            Player player = new Player(new SharpDX.Vector2(200, 200));
            Vector2 vector2 = player.PositionOfCenter;
            player.Move(Direction.Up);
            Assert.AreEqual(vector2.Y - 1, player.PositionOfCenter.Y);

            vector2 = player.PositionOfCenter;
            player.Move(Direction.Down);
            Assert.AreEqual(vector2.Y + 1, player.PositionOfCenter.Y);

            player.Gorisont = true;
            vector2 = player.PositionOfCenter;
            player.Move(Direction.Right);
            Assert.AreEqual(vector2.X + 1, player.PositionOfCenter.X);

            vector2 = player.PositionOfCenter;
            player.Move(Direction.Left);
            Assert.AreEqual(vector2.X - 1, player.PositionOfCenter.X);
        }
        /// <summary>
        /// Тест метода Goal
        /// </summary>
        [TestMethod]
        public void GoalTest()
        {
            Player player = new Player(new SharpDX.Vector2(200, 200));
            int score = 1;
            player.Goal();
            Assert.AreEqual(score, player.Score);
        }
        /// <summary>
        /// Тест метода ReturnPosition
        /// </summary>
        [TestMethod]
        public void ReturnPositionTest()
        {
            Player player = new Player(new SharpDX.Vector2(200, 200));
            Assert.IsFalse(player.GetReturnPosition());

            Vector2 vector2 = player.PositionOfCenter;
            player.Gorisont = true;
            player.Move(Direction.Left);
            Assert.IsTrue(player.GetReturnPosition());
            Assert.AreEqual(vector2.X, player.PositionOfCenter.X);

            vector2 = player.PositionOfCenter;
            player.Move(Direction.Right);
            Assert.IsTrue(player.GetReturnPosition());
            Assert.AreEqual(vector2.X, player.PositionOfCenter.X);
        }
        /// <summary>
        /// Тест метода GiftOn
        /// </summary>
        [TestMethod]
        public void GiftOnTest()
        {
            Player player = new Player(new SharpDX.Vector2(200, 200));
            bool flag = true;
            player.GiftOn();
            Assert.AreEqual(flag, player.GiftLimit);
        }
        /// <summary>
        /// Тест метода GiftOff
        /// </summary>
        [TestMethod]
        public void GiftOffTest()
        {
            Player player = new Player(new SharpDX.Vector2(200, 200));
            bool flag = false;
            player.GiftOff();
            Assert.AreEqual(flag, player.GiftLimit);
        }
    }
}