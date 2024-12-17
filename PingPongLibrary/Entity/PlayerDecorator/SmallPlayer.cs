using _PingPongLibrary._GameObject;

namespace PingPongLibrary.Entity.PlayerDecorator
{
    /// <summary>
    /// Класс, реализующий уменьшение ракетки
    /// </summary>
    public class SmallPlayer : PlayerDecorator
    {
        public SmallPlayer(PlayerGame player) : base(player)
        {
            player.Resize(new SharpDX.Size2((int)player.ActiveSprite.Width, (int)player.ActiveSprite.Heigth - 20));
        }
    }
}