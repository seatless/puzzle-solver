using System;

namespace PuzzleResolver.Service
{
    public class PuzzleNode : Node<int>, ICloneable, IEquatable<PuzzleNode>
    {
        public PuzzleNode(PuzzleNode node)
            : this(node.Value, node.ExpectedValue)
        {
        }
        
        public PuzzleNode(int value, int expectedValue)
            : base(value)
        {;
            ExpectedValue = expectedValue;
        }

        public int ExpectedValue { get; }
        
        public bool IsEmpty() => Value == PuzzleConstants.EmptyValue;
        public bool IsFinal() => Value == ExpectedValue;
        public override string ToString() => Value.ToString();
        public object Clone() => new PuzzleNode(this);
        
        #region IEquatable implementation
        
        public bool Equals(PuzzleNode other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return base.Equals(other) && ExpectedValue == other.ExpectedValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PuzzleNode) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ ExpectedValue;
            }
        }
        
        #endregion
    }
}