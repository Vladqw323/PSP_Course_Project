using _PingPongLibrary._GameObject;

namespace PingPongLibrary.Entity.PlayerDecorator
{
    /// <summary>
    /// Класс, реализующий передвижение ракетки по оси абсцисс
    /// </summary>
    public class MoveXPlayer : PlayerDecorator
    {
        public MoveXPlayer(PlayerGame player) : base(player)
        {
            player.Player.Gorisont = true;
        }
    }
}