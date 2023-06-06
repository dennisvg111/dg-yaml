using DG.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DG.Yaml
{
    public class LineReader : IDisposable
    {
        private const int _bufferSize = 1024;

        private readonly Stream _stream;
        private readonly BinaryReader _reader;
        private readonly byte[] _buffer = new byte[_bufferSize];
        private readonly Dictionary<int, long> _lineOffsets;

        private long _currentLineOffset;
        private int _totalBytesRead;
        private int _bytesAvailable;
        private int _bufferIndex;

        private int _lineNumber;

        /// <summary>
        /// The zero-based index of the next line that wil be read using <see cref="ReadLine"/>.
        /// </summary>
        public int CurrentLineNumber => _lineNumber;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="stream">Stream</param>
        public LineReader(Stream stream)
        {
            ThrowIf.Parameter.IsNull(stream, nameof(stream));
            ThrowIf.Stream(stream, nameof(stream)).CannotRead();
            ThrowIf.Stream(stream, nameof(stream)).CannotSeek();
            _stream = stream;
            _reader = new BinaryReader(_stream, Encoding.UTF8, true);

            ResetCurrentLine(0);

            _lineOffsets = new Dictionary<int, long>();
            _lineOffsets[0] = _currentLineOffset;
        }

        private void ResetCurrentLine(int lineNumber)
        {
            _lineNumber = lineNumber;
            _currentLineOffset = _stream.Position;
            _bytesAvailable = 0;
        }

        /// <summary>
        /// Reads a line of characters from the current <see cref="Stream"/>.
        /// </summary>
        /// <returns>The next line from the current stream, or <see langword="null"/> if the end of the stream is reached.</returns>
        public string ReadLine()
        {
            long bytesInLine = 0;

            StringBuilder lineBuilder = new StringBuilder();
            while (TryGetNextCharacter(out char ch))
            {
                bytesInLine++;

                if (ch == '\0' || ch == '\n')
                {
                    return lineBuilder.ToString();
                }
                if (ch == '\r')
                {
                    continue;
                }

                lineBuilder.Append(ch);
            }
            return lineBuilder.ToString();
        }

        private bool TryGetNextCharacter(out char ch)
        {
            ch = '\0';
            int numRead = Math.Min(4, (int)(_stream.Length - _stream.Position));
            if (numRead == 0)
            {
                return false;
            }
            byte[] bytes = _reader.ReadBytes(numRead);
            char[] chars = Encoding.UTF8.GetChars(bytes);

            if (chars.Length == 0)
            {
                return false;
            }

            int usedBytes = Encoding.UTF8.GetByteCount(new char[] { chars[0] });

            _stream.Position -= (numRead - usedBytes);

            ch = chars[0];
            SavePositionOnNewline(ch);
            return true;
        }

        private void SavePositionOnNewline(char ch)
        {
            if (ch == '\0' || ch == '\n')
            {
                _lineNumber++;
                _lineOffsets[_lineNumber] = _stream.Position;
            }
        }

        /// <summary>
        /// Sets the position of the current <see cref="LineReader"/> to the start of the given line>.
        /// </summary>
        /// <param name="lineNumber">The zero-based index of a line.</param>
        /// <returns>A value indicating if the end of the <see cref="Stream"/> was reached before the line number was found.</returns>
        /// <exception cref="ArgumentException"></exception>
        public bool SeekLine(int lineNumber)
        {
            ThrowIf.Number(lineNumber, nameof(lineNumber)).IsNegative();

            if (_lineOffsets.ContainsKey(lineNumber))
            {
                _stream.Seek(_lineOffsets[lineNumber], SeekOrigin.Begin);
                ResetCurrentLine(lineNumber);
                return true;
            }

            SeekLastLine();
            return TryReadUntill(lineNumber);
        }

        private void SeekLastLine()
        {
            int highestLine = _lineOffsets.Keys.Max();
            SeekLine(highestLine);
        }

        private bool TryReadUntill(int lineNumber)
        {
            while (ReadLine() != null)
            {
                if (lineNumber == _lineNumber)
                {
                    return true;
                }
            }
            return false;
        }

        #region IDisposable support
        private bool _disposed;
        /// <summary>
        /// Releases all resources used by the <see cref="LineReader"/>, including the underlying <see cref="Stream"/>.
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _stream.Dispose();
                _disposed = true;
            }
        }
        #endregion
    }
}
