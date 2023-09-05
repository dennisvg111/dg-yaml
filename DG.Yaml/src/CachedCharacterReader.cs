using DG.Yaml.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Text;

namespace DG.Yaml
{
    public class CachedCharacterReader : ICharacterReader
    {
        private const int _bufferSize = 1024;

        private readonly char[] _buffer;
        private readonly InsertionQueue<char> _cache;
        private readonly TextReader _reader;
        private bool _endReached;

        public CachedCharacterReader(TextReader reader)
        {
            _buffer = new char[_bufferSize];
            _reader = reader;
            _cache = new InsertionQueue<char>();
        }

        public CachedCharacterReader(Stream stream, Encoding encoding) : this(new StreamReader(stream, encoding)) { }

        public CachedCharacterReader(Stream stream) : this(stream, Encoding.UTF8) { }

        public CachedCharacterReader(string text) : this(new StringReader(text)) { }

        public bool TryRead(out char character)
        {
            CacheCharactersIfNeeded(1);
            if (_cache.Count == 0)
            {
                character = '\0';
                return false;
            }
            character = _cache.Take();
            return true;
        }

        public bool TryPeek(out char character)
        {
            CacheCharactersIfNeeded(1);
            if (_cache.Count == 0)
            {
                character = '\0';
                return false;
            }
            character = _cache.Peek();
            return true;
        }

        public int TryPeek(int count, out char[] chars)
        {
            CacheCharactersIfNeeded(count);
            int bufferSize = Math.Min(count, _cache.Count);

            chars = new char[bufferSize];
            for (int i = 0; i < bufferSize; i++)
            {
                chars[i] = _cache[i];
            }
            return bufferSize;
        }

        private void CacheCharactersIfNeeded(int neededAmount)
        {
            while (!_endReached && neededAmount > _cache.Count)
            {
                int amountRead = _reader.Read(_buffer, 0, _bufferSize);
                if (amountRead <= 0)
                {
                    _endReached = true;
                    return;
                }
                _cache.AppendRange(_buffer.Take(amountRead));
            }
        }
    }
}
