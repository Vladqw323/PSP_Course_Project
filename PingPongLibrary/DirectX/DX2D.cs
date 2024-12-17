using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using SharpDX.WIC;
using SharpDX.Windows;
using System.Collections.Generic;

namespace PingPongLibrary.DirectX
{
    /// <summary>
    /// Класс, реализующий основную логику DirectX
    /// </summary>
    public class DX2D
    {
        /// <summary>
        /// Фабрика для создания 2D объектов
        /// </summary>
        public SharpDX.Direct2D1.Factory Factory { get; private set; }
        /// <summary>
        /// Фабрика для работы с текстом
        /// </summary>
        public SharpDX.DirectWrite.Factory WriteFactory { get; private set; }
        /// <summary>
        /// "Цель" отрисовки
        /// </summary>
        public WindowRenderTarget RenderTarget { get; private set; }
        /// <summary>
        /// Фабрика для работы с изображениями (WIC = Windows Imaging Component)
        ///  </summary>
        public ImagingFactory ImagingFactory { get; private set; }
        /// <summary>
        /// Формат текста для вывода счёта и разыгровки
        /// </summary>
        public TextFormat TextFormatStats { get; private set; }
        /// <summary>
        /// Формат текста для вывода победителя в конце игры 
        /// </summary>
        public TextFormat TextFormatEndGame { get; private set; }
        /// <summary>
        /// Формат текста для вывода сообщения: какую кнопку нужно нажать, чтобы начать новую игру
        /// </summary>
        public TextFormat TextFormatStartNewGame { get; private set; }
        /// <summary>
        /// Кисть для сообщения: какую кнопку нужно нажать, чтобы начать новую игру
        /// </summary>
        public Brush RedBrush { get; private set; }
        /// <summary>
        /// Кисть для отрисовки счёта, разыгровки и победителя в конце игры
        /// </summary>
        public Brush QuanBrush { get; private set; }

        // Коллекция спрайтов
        // Есть конечно атлас спрайтов SpriteBatch
        // https://docs.microsoft.com/en-us/windows/win32/api/d2d1_3/nn-d2d1_3-id2d1spritebatch
        // но при его использовании не поддерживается антиалиасинг, можно использовать только AntialiasMode.Aliased
        // Источник: https://issue.life/questions/55093299
        // Таким образом он отлично подойдет, если спрайты вращать не надо
        public List<SharpDX.Direct2D1.Bitmap> Bitmaps { get; private set; }

        // В конструкторе создаем все объекты
        public DX2D(RenderForm form)
        {
            // Создание фабрик для 2D объектов и текста
            Factory = new SharpDX.Direct2D1.Factory();
            WriteFactory = new SharpDX.DirectWrite.Factory();

            // Инициализация "прямой рисовалки":
            //   Свойства отрисовщика
            RenderTargetProperties renderProp = new RenderTargetProperties()
            {
                DpiX = 0,
                DpiY = 0,
                MinLevel = FeatureLevel.Level_10,
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                Type = RenderTargetType.Hardware,
                Usage = RenderTargetUsage.None
            };
            //   Свойства отрисовщика, связанные с окном (дискриптор окна, размер в пикселях и способ представления результирующего изображения)
            HwndRenderTargetProperties winProp = new HwndRenderTargetProperties()
            {
                Hwnd = form.Handle,
                PixelSize = new Size2(form.ClientSize.Width, form.ClientSize.Height),
                PresentOptions = PresentOptions.None                                      // Immediately // None - vSync
            };
            //   Создание "цели" и задание свойств сглаживания
            RenderTarget = new WindowRenderTarget(Factory, renderProp, winProp);
            RenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            RenderTarget.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

            // Создание "картиночной" фабрики
            ImagingFactory = new ImagingFactory();

            // Задание форматов текста
            TextFormatStats = new TextFormat(WriteFactory, "Calibri", 52);
            TextFormatStats.ParagraphAlignment = ParagraphAlignment.Center;
            TextFormatStats.TextAlignment = TextAlignment.Center;
            TextFormatEndGame = new TextFormat(WriteFactory, "Ink Free", FontWeight.Bold, FontStyle.Italic, 170);
            TextFormatEndGame.ParagraphAlignment = ParagraphAlignment.Center;
            TextFormatEndGame.TextAlignment = TextAlignment.Center;
            TextFormatStartNewGame = new TextFormat(WriteFactory, "Ink Free", FontWeight.Normal, FontStyle.Italic, 50);
            TextFormatStartNewGame.ParagraphAlignment = ParagraphAlignment.Center;
            TextFormatStartNewGame.TextAlignment = TextAlignment.Center;
            // Создание кистей для текста
            RedBrush = new SolidColorBrush(RenderTarget, Color.Red);
            QuanBrush = new SolidColorBrush(RenderTarget, Color.Aquamarine);
        }

        /// <summary>
        /// Метод для загрузки изображения в коллекцию
        /// </summary>
        /// <param name="imageFileName">Путь к изображению</param>
        /// <returns>Индекс изображения в коллекции</returns>
        public int GetLoadBitmap(string imageFileName)
        {
            // Чтение изображения из bmp
            // System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap("image.bmp"); - для слабаков

            // Декодер формата
            BitmapDecoder decoder = new BitmapDecoder(ImagingFactory, imageFileName, DecodeOptions.CacheOnDemand);
            // Берем первый фрейм
            BitmapFrameDecode frame = decoder.GetFrame(0);
            // Также нужен конвертер формата 
            FormatConverter converter = new FormatConverter(ImagingFactory);
            converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
            // Вот теперь можно и bitmap
            SharpDX.Direct2D1.Bitmap bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(RenderTarget, converter);

            // Освобождаем неуправляемые ресурсы
            Utilities.Dispose(ref converter);
            Utilities.Dispose(ref frame);
            Utilities.Dispose(ref decoder);

            // Добавляем изображение в коллекцию
            if (Bitmaps == null) Bitmaps = new List<SharpDX.Direct2D1.Bitmap>(4);
            Bitmaps.Add(bitmap);
            return Bitmaps.Count - 1;
        }

        /// <summary>
        /// Освобождает неуправляемые ресурсы
        /// </summary>
        public void Dispose()
        {
            for (int i = Bitmaps.Count - 1; i >= 0; i--) // foreach здесь не пойдет, поскольку итератор нельзя передавать как ref
            {
                SharpDX.Direct2D1.Bitmap bitmap = Bitmaps[i];
                Bitmaps.RemoveAt(i);
                Utilities.Dispose(ref bitmap);
            }
        }
    }
}