using _PingPongLibrary._GameObject;

namespace PingPongLibrary.Entity.PlayerDecorator
{
    /// <summary>
    /// Класс, реализующий увеличение ракетки
    /// </summary>
    public class BigPlayer : PlayerDecorator
    {
        public BigPlayer(PlayerGame player) : base(player)
        {
            player.Resize(new SharpDX.Size2((int)player.ActiveSprite.Width, (int)player.ActiveSprite.Heigth + 20));
        }
    }
}