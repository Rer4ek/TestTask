using System;
using System.IO;
using System.Text;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private readonly Stream _localStream;
        private readonly StreamReader _streamReader;

        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof { get; private set; }

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = true;

            _localStream = new FileStream(fileFullPath, FileMode.Open);
            _streamReader = new StreamReader(_localStream, Encoding.Default);
        }

        public void Close()
        {
            _localStream?.Close();
            _streamReader?.Close();
        }

        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            if (IsEof)
            {
                Close();
                throw new IOException("read the file after end");
            }

            int buffer = _streamReader.Read();

            if (buffer == -1)
            {
                IsEof = true;
                return '\0';
            }

            return (char)buffer;
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                IsEof = true;
                return;
            }

            _localStream.Position = 0;
            IsEof = false;
        }
    }
}
