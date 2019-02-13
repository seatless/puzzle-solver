using System;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleResolver.Service
{
    public class PuzzleNodeList : List<PuzzleNode>, ICloneable
    {
        public PuzzleNodeList()
        {
        }

        public PuzzleNodeList(IEnumerable<PuzzleNode> nodes)
            : base(nodes)
        {
        }

        internal new PuzzleNode this[int expectedValue] => GetNodeByExpectedValue(expectedValue);

        public PuzzleNode GetNodeByExpectedValue(int expectedValue) =>
            this.SingleOrDefault(node => node.ExpectedValue == expectedValue);
        
        public PuzzleNode GetNodeByActualValue(int actualValue) =>
            this.SingleOrDefault(node => node.Value == actualValue);

        public object Clone() => new PuzzleNodeList(this.Select(node => node.Clone() as PuzzleNode));

        public override string ToString() => string.Join("", this.Select(node => node.Value.ToString()));
        
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 19;
                foreach (var node in this)
                {
                    hash = hash * 31 + node.GetHashCode();
                }
                return hash;
            }
        }
    }
}