using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PuzzleResolver.Service
{
    /// <summary>
    /// Service that resolves puzzle for given input.
    /// </summary>
    public class PuzzleResolverService : IResolver
    {
        /// <inheritdoc />
        public int[] Solve(int[] input)
        {
            var puzzle = new Puzzle(input, PuzzleConstants.DefaultFinalConfiguration);

            // Contains next puzzle steps that hasn't been checked for solution yet.
            var openStates = new SortedSet<PuzzleState>(PuzzleState.Comparer) { new PuzzleState(puzzle, 0) };
            
            // Contains puzzle steps that has been checked already.
            // Store them in order to avoid multiple checks of already explored puzzle configurations.
            var closedStates = new SortedSet<PuzzleState>(PuzzleState.Comparer);
            
            // Steps taken to solve the puzzle (number of puzzle configurations that has been iterated through).
            int stepsToSolve = 0;

            // TODO: Add multithreading support
            while (openStates.Any())
            {
                var nextOpenState = openStates.First();
                
                if (nextOpenState.Puzzle.IsSolved())
                {
                    Debug.WriteLine(
                        $"Solution: '{string.Join(",", nextOpenState.Puzzle.MoveList)}', total steps taken to solve puzzle: {stepsToSolve}.");
                    return nextOpenState.Puzzle.MoveList.ToArray();
                }
                
                var nextLevel = nextOpenState.Level + 1;
                
                foreach (var puzzleMove in nextOpenState.Puzzle.GetMoves())
                {
                    var nextMove = new PuzzleState(puzzleMove, nextLevel);
                    if (!closedStates.Contains(nextMove))
                    {
                        openStates.Add(nextMove);
                    }
                }

                closedStates.Add(nextOpenState);
                openStates.Remove(nextOpenState);
                stepsToSolve++;
            }
            
            return Array.Empty<int>();
        }
    }
}