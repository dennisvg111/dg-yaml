using DG.Common.Exceptions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DG.Yaml
{
    public class LineReader
    {
        private const int _bufferSize = 1024;

        private readonly Stream _stream;
        private readonly byte[] _buffer = new byte[_bufferSize];
        private readonly Dictionary<int, long> _lineOffsets;

        private long _currentLineOffset;
        private int _bytesRead;
        private int _bufferIndex;

        private int _lineNumber = 0;

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

            ResetLine();

            _lineOffsets = new Dictionary<int, long>();
            _lineOffsets[0] = _currentLineOffset;
        }

        private void ResetLine()
        {
            _currentLineOffset = _stream.Position;
            _bytesRead = 0;
        }

        /// <summary>
        /// Reads a line of characters from the current <see cref="Stream"/>.
        /// </summary>
        /// <returns>The next line from the current stream, or <see langword="null"/> if the end of the stream is reached.</returns>
        public string ReadLine()
        {
            long bytesInLine = 0;
            bool found = false;

            StringBuilder lineBuilder = new StringBuilder();
            while (!found)
            {
                if (_bytesRead <= 0)
                {
                    // Read next block
                    _bufferIndex = 0;
                    _bytesRead = _stream.Read(_buffer, 0, _bufferSize);
                    if (_bytesRead == 0)
                    {
                        if (lineBuilder.Length > 0)
                        {
                            break;
                        }
                        return null;
                    }
                }

                for (int max = _bufferIndex + _bytesRead; _bufferIndex < max;)
                {
                    char ch = (char)_buffer[_bufferIndex];
                    _bytesRead--;
                    _bufferIndex++;
                    bytesInLine++;

                    if (ch == '\0' || ch == '\n')
                    {
                        found = true;
                        break;
                    }
                    else if (ch == '\r')
                    {
                        continue;
                    }
                    lineBuilder.Append(ch);
                }
            }

            _lineNumber++;
            _currentLineOffset += bytesInLine;
            _lineOffsets[_lineNumber] = _currentLineOffset;
            return lineBuilder.ToString();
        }

        public void SeekLine(int lineNumber, SeekOrigin origin)
        {
            if (_lineOffsets.ContainsKey(lineNumber))
            {
                _stream.Seek(_lineOffsets[lineNumber], origin);
                _lineNumber = lineNumber;
                ResetLine();
                return;
            }

            int highestLine = _lineOffsets.Keys.Max();
            SeekLine(highestLine, origin);

            while (ReadLine() != null)
            {
                if (lineNumber == _lineNumber)
                {
                    return;
                }
            }
        }

        private bool _disposed;
        /// <summary>
        /// Free resources
        /// </summary>
        public void Dispose()
        {
            if (!_disposed)
            {
                _stream.Close();
                _stream.Dispose();
                _disposed = true;
            }
        }
    }
}
