using System;
using System.Collections.Generic;

namespace PuzzleResolver.Service
{
    public class Node<T> : IEquatable<Node<T>>
    {
        public IList<Node<T>> Neighbors { get; }
        public T Value { get; set; }
        
        public Node(T value, IList<Node<T>> neighbors = default)
        {
            Value = value;
            Neighbors = neighbors ?? new List<Node<T>>();
        }

        #region Equals overrides
        
        public bool Equals(Node<T> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return EqualityComparer<T>.Default.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Node<T>) obj);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Value);
        }
        
        #endregion
    }
}