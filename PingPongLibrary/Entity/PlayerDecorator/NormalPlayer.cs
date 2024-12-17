using _PingPongLibrary._GameObject;

namespace PingPongLibrary.Entity.PlayerDecorator
{
    /// <summary>
    /// Класс, реализующий возврат игрока в первоначальное состояние
    /// </summary>
    public class NormalPlayer : PlayerDecorator
    {
        public NormalPlayer(PlayerGame player) : base(player)
        {
            player.NormalSize();
            player.Player.Speed = 1;
            player.Player.Gorisont = false;
            player.Player.GiftOff();
            player.Time = 0;
        }
    }
}