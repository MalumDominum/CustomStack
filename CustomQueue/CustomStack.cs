using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CustomStack
{
    public class CustomStack<T> : IReadOnlyCollection<T>
    {
        public class Node<T>
        {
            public CustomStack<T> Stack;
            public Node<T> Next;
            public Node<T> Previous;
            public T Data;
            public Node(T value) => Data = value;
            public Node(CustomStack<T> stack, T value)
            {
                Stack = stack;
                Data = value;
            }
            public void Invalidate()
            {
                Stack = null;
                Next = null;
                Previous = null;
            }
        }

        private Node<T> _head;
        public int Count { get; private set; }
        public delegate void StackHandler(object sender, StackEventArgs e);
        public event StackHandler Notify;

        /// <summary>
        /// Returns (without removing) the object from start of the Stack
        /// </summary>
        public T Peek => _head.Data;

        public CustomStack() { }

        /// <summary>
        /// Initializing Stack with the collection
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// Thrown when passing collection is null
        /// </exception>
        public CustomStack(IEnumerable<T> collection)
        {
            if (collection == null) throw new ArgumentNullException(nameof(collection));

            foreach (T obj in collection) Push(obj);
        }

        public IEnumerator<T> GetEnumerator() => new StackEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Put element(s) on last position in Stack
        /// </summary>
        public void Push(params T[] elements)
        {
            foreach (var element in elements)
            {
                var newNode = new Node<T>(this, element);
                if (_head == null)
                {
                    newNode.Next = newNode;
                    newNode.Previous = newNode;
                    _head = newNode;
                    ++Count;
                }
                else
                {
                    newNode.Next = _head;
                    newNode.Previous = _head.Previous;
                    _head.Previous.Next = newNode;
                    _head.Previous = newNode;
                    _head = newNode;
                    ++Count;
                }

                Notify?.Invoke(this, new StackEventArgs("Element " + element + " added to the Stack on " + (Count - 1) + " position"));
            }
        }

        /// <summary>
        /// Returns and removes the object from start of the Stack
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// Thrown when trying to Pop empty Stack
        /// </exception>
        public T Pop()
        {
            var node = _head;

            if (node == null)
                throw new InvalidOperationException(nameof(node),
                    new Exception("The stack is empty"));

            if (node.Next == node)
                _head = null;
            else
            {
                node.Next.Previous = node.Previous;
                node.Previous.Next = node.Next;
                if (_head == node)
                    _head = node.Next;
            }
            Notify?.Invoke(this, new StackEventArgs("Element " + node.Data + " removed from the stack"));
            node.Invalidate();
            --Count;

            if (Count == 0)
                Notify?.Invoke(this, new StackEventArgs("Stack empty"));

            return node.Data;
        }

        /// <summary>
        /// Delete all elements from the Stack
        /// </summary>
        public void Clear()
        {
            var headNode = _head;
            while (headNode != null)
            {
                var nextNode = headNode;
                headNode = headNode.Next;
                nextNode.Invalidate();
            }

            _head = null;
            Count = 0;
            Notify?.Invoke(this, new StackEventArgs("Stack empty"));
        }

        /// <summary>
        /// Check the Stack if it contains the passed element
        /// </summary>
        public bool Contains(T item)
        {
            if (Count == 0) return false;

            var node = _head;
            var comparer = EqualityComparer<T>.Default;
            while (!comparer.Equals(node.Data, item))
            {
                node = node.Next;
                if (node == _head)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Convert Stack into the Array
        /// </summary>
        public T[] ToArray()
        {
            if (Count == 0) 
                return Array.Empty<T>();

            var array = new T[Count];
            var node = _head;
            for (int i = 0; i < Count; i++)
            {
                array[i] = node.Data;
                node = node.Next;
            }
            
            return array.ToArray();
        }

        private class StackEnumerator : IEnumerator<T>
        {
            private readonly CustomStack<T> _stack;
            private Node<T> _node;
            private int _index;
            public T Current { get; private set; }

            object IEnumerator.Current
            {
                get
                {
                    if (_index == 0 || _index == _stack.Count + 1)
                        throw new InvalidOperationException();
                    return Current;
                }
            }
            public StackEnumerator(CustomStack<T> Stack)
            {
                _stack = Stack;
                _node = Stack._head;
                Current = default;
            }

            public bool MoveNext()
            {
                if (_node == null)
                {
                    _index = _stack.Count + 1;
                    return false;
                }

                ++_index;
                Current = _node.Data;
                _node = _node.Next;

                if (_node == _stack._head)
                    _node = null;

                return true;
            }

            public void Reset()
            {
                Current = default;
                _node = _stack._head;
                _index = 0;
            }

            public void Dispose() { }

        }
    }
}
