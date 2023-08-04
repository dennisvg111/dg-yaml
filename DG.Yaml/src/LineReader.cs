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
        private readonly CharacterReader _reader;

        private readonly Dictionary<int, long> _lineOffsets;
        private int _lineNumber = 0;

        /// <summary>
        /// The zero-based index of the next line that wil be read using <see cref="ReadLine"/>.
        /// </summary>
        public int CurrentLineNumber => _lineNumber;

        /// <summary>
        /// Initializes a new instance of <see cref="LineReader"/> with the given stream and encoding.
        /// </summary>
        /// <param name="stream">Stream</param>
        public LineReader(Stream stream, Encoding encoding)
        {
            ThrowIf.Parameter.IsNull(stream, nameof(stream));
            ThrowIf.Stream(stream, nameof(stream)).CannotRead();
            ThrowIf.Stream(stream, nameof(stream)).CannotSeek();
            ThrowIf.Parameter.IsNull(encoding, nameof(encoding));
            _reader = new CharacterReader(stream, encoding);

            _lineOffsets = new Dictionary<int, long>();
            _lineOffsets[0] = _reader.Position;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="LineReader"/> with the given stream, and <see cref="Encoding.UTF8"/> as the default encoding.
        /// </summary>
        /// <param name="stream"></param>
        public LineReader(Stream stream) : this(stream, Encoding.UTF8)
        {

        }

        /// <summary>
        /// Reads a line of characters from the current <see cref="Stream"/>.
        /// </summary>
        /// <returns>The next line from the current stream, or <see langword="null"/> if the end of the stream is reached.</returns>
        public string ReadLine()
        {
            long bytesInLine = 0;

            StringBuilder lineBuilder = new StringBuilder();
            while (_reader.TryRead(out char ch))
            {
                SavePositionOnNewline(ch);
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
            if (bytesInLine == 0)
            {
                return null;
            }
            return lineBuilder.ToString();
        }

        private void SavePositionOnNewline(char ch)
        {
            if (ch == '\0' || ch == '\n')
            {
                _lineNumber++;
                _lineOffsets[_lineNumber] = _reader.Position;
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
                _reader.Seek(_lineOffsets[lineNumber], SeekOrigin.Begin);
                _lineNumber = lineNumber;
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
                _reader.Dispose();
                _disposed = true;
            }
        }
        #endregion
    }
}
