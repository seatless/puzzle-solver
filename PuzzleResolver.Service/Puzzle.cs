using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PuzzleResolver.Service
{
    public class Puzzle : IEnumerable<PuzzleNode>, ICloneable, IEquatable<Puzzle>
    {
        private static readonly KeyValueList<int, int> Connections = new KeyValueList<int, int>
        {
            {0, 3}, {0, 4}, {1, 2}, {1, 3}, {2, 4}, {3, 5}, 
            {4, 6}, {5, 7}, {6, 8}, {7, 8}, {7, 9}, {8, 9}
        };

        private PuzzleNodeList Nodes { get; }
        public List<int> MoveList { get; }

        public Puzzle(Puzzle puzzle)
        {
            MoveList = new List<int>(puzzle.MoveList);
            Nodes = puzzle.Nodes.Clone() as PuzzleNodeList;
            ConnectNodes(Connections, Nodes);
        }

        public Puzzle(IList<int> nodeValues, IList<int> expectedValues)
        {
            if (nodeValues == null)
                throw new ArgumentNullException(nameof(nodeValues));
            if (expectedValues == null)
                throw new ArgumentNullException(nameof(expectedValues));
            if (nodeValues.Count != expectedValues.Count)
                throw new ArgumentException(
                    $"Expected {expectedValues.Count} " +
                    $"input values for DiamondPuzzle initialization. Actual: {nodeValues.Count}.");

            MoveList = new List<int>();
            Nodes = new PuzzleNodeList();
            for (int i = 0; i < nodeValues.Count; i++)
            {
                Nodes.Add(
                    new PuzzleNode(nodeValues[i], expectedValues[i]));
            }

            if (Nodes.Count(node => node.IsEmpty()) != 1)
                throw new InvalidOperationException(
                    $"Invalid puzzle configuration: '{Nodes}'. Puzzle must have exactly one open site.");

            ConnectNodes(Connections, Nodes);
        }

        public void Connect(PuzzleNode from, PuzzleNode to)
        {
            from.Neighbors.Add(to);
            to.Neighbors.Add(from);
        }

        public static bool IsConnected(PuzzleNode leftNode, PuzzleNode rightNode)
        {
            return leftNode.Neighbors.Contains(rightNode) &&
                   rightNode.Neighbors.Contains(leftNode);
        }

        public void Move(int value)
        {
            var leftNode = Nodes.GetNodeByActualValue(value);
            var rightNode = Nodes.GetNodeByActualValue(PuzzleConstants.EmptyValue);

            if (!IsConnected(leftNode, rightNode))
                throw new InvalidOperationException(
                    $"Cannot move value {value} to empty spot. It's not connected to {PuzzleConstants.EmptyValue}.");

            leftNode.Value = PuzzleConstants.EmptyValue;
            rightNode.Value = value;
            
            MoveList.Add(value);
        }

        public Puzzle[] GetMoves()
        {
            var openNode = Nodes.Single(node => node.IsEmpty());
            var moves = new List<Puzzle>(openNode.Neighbors.Count);

            foreach (var neighborNode in openNode.Neighbors)
            {
                var movedPuzzle = new Puzzle(this);
                movedPuzzle.Move(neighborNode.Value);
                moves.Add(movedPuzzle);
            }

            return moves.ToArray();
        }
        
        public bool IsSolved() => Nodes.All(n => n.IsFinal());
        public int GetCost() => Nodes.Count(node => !node.IsFinal()); // TODO: replace it with more efficient cost calculating algorithm?
        public string GetState() => Nodes.ToString();

        public string GetPrettyView()
        {
            const int i = 3;
            return $"{"",i}{Nodes[1],i}{"-",i}{Nodes[2],i}{"",i}\n" +
                   $"{"/",i}{"",i}{"",i}{"",i}{"\\",i}\n" +
                   $"{Nodes[3],i}{"-",i}{Nodes[0],i}{"-",i}{Nodes[4],i}\n" +
                   $"{"|",i}{"",i}{"",i}{"",i}{"|",i}\n" +
                   $"{Nodes[5],i}{"",i}{"",i}{"",i}{Nodes[6],i}\n" +
                   $"{"\\",i}{"",i}{"",i}{"",i}{"/",i}\n" +
                   $"{"",i}{Nodes[7],i}{"-",i}{Nodes[8],i}{"",i}\n" +
                   $"{"",i}{"\\",i}{"",i}{"/",i}{"",i}\n" +
                   $"{"",i}{"",i}{Nodes[9],i}{"",i}{"",i}\n";
        }
        

        private void ConnectNodes(KeyValueList<int, int> connections, PuzzleNodeList nodes)
        {
            foreach ((int left, int right) in connections)
            {
                Connect(nodes.GetNodeByExpectedValue(left), nodes.GetNodeByExpectedValue(right));
            }
        }

        #region IEnumerable implementation
        
        public IEnumerator<PuzzleNode> GetEnumerator() => Nodes.GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        #endregion
        
        #region ICloneable implementation
        
        public object Clone()
        {
            return new Puzzle(this);
        }
        
        #endregion
        
        #region IEquatable implementation
        
        public bool Equals(Puzzle other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Nodes.SequenceEqual(other.Nodes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Puzzle) obj);
        }

        public override int GetHashCode()
        {
            return (Nodes != null ? Nodes.GetHashCode() : 0);
        }

        #endregion
    }
}