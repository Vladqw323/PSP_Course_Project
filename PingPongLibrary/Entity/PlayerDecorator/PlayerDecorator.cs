using _PingPongLibrary._GameObject;
using System.Collections.Generic;

namespace PingPongLibrary.Entity.PlayerDecorator
{
    /// <summary>
    /// Абстрактный класс, реализующий изменение ракетки
    /// </summary>
    public abstract class PlayerDecorator : PlayerGame
    {
        protected PlayerDecorator(PlayerGame player) : base(player.Player, new List<Sprite>() { player.ActiveSprite })
        {
        }
    }
}