using _PingPongLibrary._GameObject;

namespace PingPongLibrary.Entity.PlayerDecorator
{
    /// <summary>
    /// Класс, реализующий уменьшение скорости ракетки
    /// </summary>
    public class LowSpeedPlayer : PlayerDecorator
    {
        public LowSpeedPlayer(PlayerGame player) : base(player)
        {
            player.Player.Speed = player.Player.Speed * 0.8f;
        }
    }
}