using System;
using System.Collections.Generic;

namespace PuzzleResolver.Service
{
    /// <summary>
    /// State of the puzzle that stores puzzle level (depth) and provides comparer for storing different puzzles with their states in ordered collection.
    /// </summary>
    public class PuzzleState : IEquatable<PuzzleState>
    {
        public static IComparer<PuzzleState> Comparer { get; } = new PuzzleStateComparer();
        public Puzzle Puzzle { get; }
        public int Level { get; }

        public PuzzleState(Puzzle puzzle, int level)
        {
            Puzzle = puzzle;
            Level = level;
        }

        public int GetCost() => Puzzle.GetCost() + Level;
        
        private sealed class PuzzleStateComparer : IComparer<PuzzleState>
        {
            public int Compare(PuzzleState x, PuzzleState y)
            {
                if (ReferenceEquals(x, y)) return 0;
                if (ReferenceEquals(null, y)) return 1;
                if (ReferenceEquals(null, x)) return -1;
                
                var costComparison = x.GetCost().CompareTo(y.GetCost());
                if (costComparison != 0) return costComparison;
                return x.Puzzle.GetState().CompareTo(y.Puzzle.GetState());
            }
        }
        
        #region IEquatable implementation
        
        public bool Equals(PuzzleState other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Puzzle.GetState(), other.Puzzle.GetState());
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PuzzleState) obj);
        }

        public override int GetHashCode()
        {
            return (Puzzle != null ? Puzzle.GetHashCode() : 0);
        }
        
        #endregion
    }
}