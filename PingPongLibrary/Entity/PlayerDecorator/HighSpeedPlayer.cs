using _PingPongLibrary._GameObject;

namespace PingPongLibrary.Entity.PlayerDecorator
{
    /// <summary>
    /// Класс, реализующий увеличение скорости ракетки
    /// </summary>
    public class HighSpeedPlayer : PlayerDecorator
    {
        public HighSpeedPlayer(PlayerGame player) : base(player)
        {
            player.Player.Speed = player.Player.Speed * 1.2f;
        }
    }
}