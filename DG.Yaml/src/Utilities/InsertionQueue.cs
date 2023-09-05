using DG.Common.Exceptions;
using System.Collections.Generic;

namespace DG.Yaml.Utilities
{
    /// <summary>
    /// A queue that allows direct insertion, and operates on a First-In-First-Out basis.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class InsertionQueue<T>
    {
        private readonly List<T> _items;

        public int Count => _items.Count;

        public InsertionQueue()
        {
            _items = new List<T>();
        }

        public void Append(T item)
        {
            _items.Add(item);
        }

        public void AppendRange(IEnumerable<T> items)
        {
            _items.AddRange(items);
        }

        public void InsertAt(int index, T item)
        {
            _items.Insert(index, item);
        }

        /// <summary>
        /// Takes the first item from the queue.
        /// </summary>
        /// <returns></returns>
        public T Take()
        {
            ThrowIf.Collection(_items, "queue").IsEmpty($"Cannot execute {nameof(Take)} operation on empty queue.");
            var first = _items[0];
            _items.RemoveAt(0);
            return first;
        }

        public T Peek()
        {
            ThrowIf.Collection(_items, "queue").IsEmpty($"Cannot execute {nameof(Peek)} operation on empty queue.");
            return _items[0];
        }

        public T this[int index]
        {
            get
            {
                ThrowIf.Collection(_items, "queue").CountLessThan(index + 1);
                return _items[index];
            }
        }
    }
}
