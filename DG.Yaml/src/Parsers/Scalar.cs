using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DG.Yaml.Parsers
{
    public class Scalar
    {
        private readonly static StringBuilder _builder = new StringBuilder();

        private readonly List<char> _buffer;

        public int Length => _buffer.Count;

        public Scalar()
        {
            _buffer = new List<char>(1024);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Write(char c)
        {
            _buffer.Add(c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            _buffer.Clear();
        }

        public override string ToString()
        {
            lock (_builder)
            {
                _builder.Clear();
                foreach (char c in _buffer)
                {
                    _builder.Append(c);
                }
                return _builder.ToString();
            }
        }
    }
}
