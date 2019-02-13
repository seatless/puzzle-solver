namespace PuzzleResolver.Service
{
    /// <summary>
    /// Interface for resolving puzzles on given input.
    /// </summary>
    public interface IResolver
    {
        /// <summary>
        /// Solves a puzzle for given input.
        /// </summary>
        /// <param name="input">Puzzle input.</param>
        /// <returns>Sequence of moves required to reach solution for given input.</returns>
        int[] Solve(int[] input);
    }
}