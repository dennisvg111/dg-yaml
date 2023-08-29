using DG.Common.Exceptions;
using System;
using System.IO;
using System.Text;

namespace DG.Yaml
{
    public class CharacterReader : IDisposable, ICharacterReader
    {
        private readonly Stream _stream;
        private readonly BinaryReader _reader;
        private readonly Encoding _encoding;

        public long Position
        {
            get
            {
                return _stream.Position;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CharacterReader"/> with the given stream and encoding.
        /// </summary>
        /// <param name="stream">Stream</param>
        public CharacterReader(Stream stream, Encoding encoding)
        {
            ThrowIf.Parameter.IsNull(stream, nameof(stream));
            ThrowIf.Stream(stream, nameof(stream)).CannotRead();
            ThrowIf.Stream(stream, nameof(stream)).CannotSeek();
            ThrowIf.Parameter.IsNull(encoding, nameof(encoding));
            _stream = stream;
            _reader = new BinaryReader(_stream, Encoding.UTF8, true);
            _encoding = encoding;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="CharacterReader"/> with the given stream, and <see cref="Encoding.UTF8"/> as the default encoding.
        /// </summary>
        /// <param name="stream"></param>
        public CharacterReader(Stream stream) : this(stream, Encoding.UTF8)
        {

        }

        public void Seek(long offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
        }

        public int TryRead(int count, out char[] characters)
        {
            characters = new char[0];
            int numRead = Math.Min(4 * count, (int)(_stream.Length - _stream.Position));
            if (numRead == 0)
            {
                return 0;
            }

            byte[] bytes = _reader.ReadBytes(numRead);
            char[] chars = _encoding.GetChars(bytes);
            if (chars.Length == 0)
            {
                return 0;
            }

            int actualFound = Math.Min(count, chars.Length);
            characters = new char[actualFound];
            Array.Copy(chars, characters, actualFound);
            int usedBytes = _encoding.GetByteCount(characters);

            _stream.Position -= (numRead - usedBytes);
            return characters.Length;
        }

        public bool TryRead(out char character)
        {
            character = '\0';
            if (TryRead(1, out char[] characters) == 0)
            {
                return false;
            }
            character = characters[0];
            return true;
        }

        public bool TryPeek(out char character)
        {
            long position = _stream.Position;
            bool success = TryRead(out character);
            _stream.Position = position;
            return success;
        }

        public int TryPeek(int count, out char[] chars)
        {
            long position = _stream.Position;
            int numRead = TryRead(count, out chars);
            _stream.Position = position;
            return numRead;
        }

        #region IDisposable support
        private bool _disposed;
        /// <summary>
        /// Releases all resources used by the <see cref="CharacterReader"/>, including the underlying <see cref="Stream"/>.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _reader.Dispose();
                _stream.Dispose();
                _disposed = true;
            }
        }
        #endregion
    }
}
