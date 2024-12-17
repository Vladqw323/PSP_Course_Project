using SharpDX;

namespace PingPongLibrary.Entity
{
    /// <summary>
    /// Абстрактный класс BonusFactory
    /// </summary>
    public abstract class BonusFactory
    {
        protected Vector2 vec;
        /// <summary>
        /// Абстрактный метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Generator</returns>
        public abstract Generator GetBonus();
    }
    /// <summary>
    /// Класс, описывающий увеличенного игрока
    /// </summary>
    public class BonusBigPlayer : BonusFactory
    {
        public BonusBigPlayer(Vector2 vec) : base()
        {
            this.vec = vec;
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Generator</returns>
        public override Generator GetBonus()
        {
            return new BigPlayer(vec);
        }
    }
    /// <summary>
    /// Класс, описывающий уменьшенного игрока
    /// </summary>
    public class BonusSmallPlayer : BonusFactory
    {
        public BonusSmallPlayer(Vector2 vec) : base()
        {
            this.vec = vec;
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Generator</returns>
        public override Generator GetBonus()
        {
            return new SmallPlayer(vec);
        }
    }
    /// <summary>
    /// Класс, описывающий игрока с увеличенной скоростью
    /// </summary>
    public class BonusHighSpeedPlayer : BonusFactory
    {
        public BonusHighSpeedPlayer(Vector2 vec) : base()
        {
            this.vec = vec;
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Generator</returns>
        public override Generator GetBonus()
        {
            return new HighSpeedPlayer(vec);
        }
    }
    /// <summary>
    /// Класс, описывающий игрока с уменьшенной скоростью
    /// </summary>
    public class BonusLowSpeedPlayer : BonusFactory
    {
        public BonusLowSpeedPlayer(Vector2 vec) : base()
        {
            this.vec = vec;
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Generator</returns>
        public override Generator GetBonus()
        {
            return new LowSpeedPlayer(vec);
        }
    }
    /// <summary>
    /// Класс, описывающий движение игрока по горизонтали
    /// </summary>
    public class BonusMoveXPlayer : BonusFactory
    {
        public BonusMoveXPlayer(Vector2 vec) : base()
        {
            this.vec = vec;
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает реализацию абстрактного класса Generator</returns>
        public override Generator GetBonus()
        {
            return new MoveXPlayer(vec);
        }
    }

    /// <summary>
    /// Абстрактный класс Generator
    /// </summary>
    public abstract class Generator
    {
        protected Bonus bonus;
        /// <summary>
        /// Абстрактный метод
        /// </summary>
        /// <returns>Возвращает объект класса Bonus</returns>
        public abstract Bonus GetGenerator();
    }
    /// <summary>
    /// Класс, описывающий увеличенного игрока
    /// </summary>
    public class BigPlayer : Generator
    {
        public BigPlayer(Vector2 vec) : base()
        {
            bonus = new Bonus(vec, TypeBonus.Big);
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает объект класса Bonus</returns>
        public override Bonus GetGenerator()
        {
            return bonus;
        }
    }
    /// <summary>
    /// Класс, описывающий уменьшенного игрока
    /// </summary>
    public class SmallPlayer : Generator
    {
        public SmallPlayer(Vector2 vec) : base()
        {
            bonus = new Bonus(vec, TypeBonus.Small);
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает объект класса Bonus</returns>
        public override Bonus GetGenerator()
        {
            return bonus;
        }
    }
    /// <summary>
    /// Класс, описывающий игрока с увеличенной скоростью
    /// </summary>
    public class HighSpeedPlayer : Generator
    {
        public HighSpeedPlayer(Vector2 vec) : base()
        {
            bonus = new Bonus(vec, TypeBonus.HighSpeed);
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает объект класса Bonus</returns>
        public override Bonus GetGenerator()
        {
            return bonus;
        }
    }
    /// <summary>
    /// Класс, описывающий игрока с уменьшенной скоростью
    /// </summary>
    public class LowSpeedPlayer : Generator
    {
        public LowSpeedPlayer(Vector2 vec) : base()
        {
            bonus = new Bonus(vec, TypeBonus.LowSpeed);
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает объект класса Bonus</returns>
        public override Bonus GetGenerator()
        {
            return bonus;
        }
    }
    /// <summary>
    /// Класс, описывающий движение игрока по горизонтали
    /// </summary>
    public class MoveXPlayer : Generator
    {
        public MoveXPlayer(Vector2 vec) : base()
        {
            bonus = new Bonus(vec, TypeBonus.MoveX);
        }
        /// <summary>
        /// Переопределённый метод
        /// </summary>
        /// <returns>Возвращает объект класса Bonus</returns>
        public override Bonus GetGenerator()
        {
            return bonus;
        }
    }
}