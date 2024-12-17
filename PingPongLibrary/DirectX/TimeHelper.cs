using System;
using System.Diagnostics;

namespace PingPongLibrary.DirectX
{
    /// <summary>
    /// Предназначен для работы со временем
    /// </summary>
    public class TimeHelper
    { 
        // Таймер
        private Stopwatch _watch;

        // Текущее время в секундах
        /// <summary>
        /// Служит для подсчета времени
        /// </summary>
        public float Time { get; private set; }

        // В конструкторе создаем экземпляр таймера и вызываем метод Reset
        public TimeHelper()
        {
            _watch = new Stopwatch();
            Reset();
        }

        /// <summary>
        /// Рассчитывает значение поля _time
        /// </summary>
        public void Update()
        {
            // Обновление подсчитываемых значений; Должен вызываться в начале каждого кадра
            // Текущее значение счетчика тиков
            long ticks = _watch.Elapsed.Ticks;
            // Вычисляем текущее время и интервал между текущим и прошлым кадрами
            Time = (float)ticks / TimeSpan.TicksPerSecond;
        }

        /// <summary>
        /// Обнуляет значение поля _watch и запускает счетчик заново
        /// </summary>
        public void Reset()
        {
            _watch.Reset();
            _watch.Start();
        }
    }
}