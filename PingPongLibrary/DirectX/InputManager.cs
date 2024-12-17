using SharpDX;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace PingPongLibrary.DirectX
{
    /// <summary>
    /// Реализует подключение клавиатуры и обновление её состояния
    /// </summary>
    public class InputManager
    {
        // Экземпляр объекта "прямого ввода"
        private DirectInput _directInput;

        // Поля и свойства, связанные с клавиатурой
        private Keyboard _keyboard;
        private KeyboardState _keyboardState;
        /// <summary>
        /// Определяет состояние операции с клавиатурой
        /// </summary>
        public KeyboardState KeyboardState { get => _keyboardState; }
        /// <summary>
        /// Определяет обновление состояния клавиатуры
        /// </summary>
        public bool KeyboardUpdated { get; private set; }
        private bool _keyboardAcquired;

        // В конструкторе создаем все объекты и пробуем получить доступ к клавиатуре
        public InputManager(RenderForm renderForm)
        {
            _directInput = new DirectInput();

            _keyboard = new Keyboard(_directInput);
            _keyboard.Properties.BufferSize = 16;
            _keyboard.SetCooperativeLevel(renderForm.Handle, CooperativeLevel.Foreground | CooperativeLevel.NonExclusive);
            AcquireKeyboard();
            _keyboardState = new KeyboardState();
        }

        /// <summary>
        /// Получение доступа к клавиатуре
        /// </summary>
        private void AcquireKeyboard()
        {
            try
            {
                _keyboard.Acquire();
                _keyboardAcquired = true;
            }
            catch (SharpDXException e)
            {
                if (e.ResultCode.Failure)
                    _keyboardAcquired = false;
            }
        }

        /// <summary>
        /// Обновление состояния клавиатуры
        /// </summary>
        public void UpdateKeyboardState()
        {
            // Если доступ не был получен, пробуем здесь
            if (!_keyboardAcquired) AcquireKeyboard();

            // Пробуем обновить состояние
            ResultDescriptor resultCode = ResultCode.Ok;
            try
            {
                _keyboard.GetCurrentState(ref _keyboardState);
                // Успех
                KeyboardUpdated = true;
            }
            catch (SharpDXException e)
            {
                resultCode = e.Descriptor;
                // Отказ
                KeyboardUpdated = false;
            }

            // В большинстве случаев отказ из-за потери фокуса ввода
            // Устанавливаем соответствующий флаг, чтобы в следующем кадре попытаться получить доступ
            if (resultCode == ResultCode.InputLost || resultCode == ResultCode.NotAcquired)
                _keyboardAcquired = false;
        }

        /// <summary>
        /// Освобождение неуправляемых ресурсов
        /// </summary>
        public void Dispose()
        {
            Utilities.Dispose(ref _keyboard);
            Utilities.Dispose(ref _directInput);
        }
    }
}