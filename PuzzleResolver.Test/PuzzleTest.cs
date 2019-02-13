using System;
using PuzzleResolver.Service;
using Xunit;

namespace PuzzleResolver.Test
{
    public class PuzzleTest
    {
        private readonly IResolver _puzzleResolver;
        
        // TODO: Add DI
        public PuzzleTest()
        {
            _puzzleResolver = new PuzzleResolverService();
        }

        [Fact]
        public void Solve_EmptyPuzzle_Exception()
        {
            Assert.Throws<ArgumentException>(() => _puzzleResolver.Solve(Array.Empty<int>()));
        }
        
        [Fact]
        public void Solve_EasyPuzzle_Success()
        {
            var solution = _puzzleResolver.Solve(new[] {1, 2, 3, 4, 6, 5, 0, 7, 8, 9});
            
            Assert.NotNull(solution);
            Assert.NotEmpty(solution);
            Assert.Equal(new [] {6, 4}, solution);
            
        }
        
        [Fact]
        public void Solve_MediumPuzzle_Success()
        {
            var solution = _puzzleResolver.Solve(new[] {1, 2, 3, 4, 6, 5, 8, 9, 7, 0});
            
            Assert.NotNull(solution);
            Assert.NotEmpty(solution);
        }
        
        [Fact]
        public void Solve_HardPuzzle_Success()
        {
            var solution = _puzzleResolver.Solve(new[] {3, 5, 7, 1, 4, 9, 6, 2, 0, 8});
            
            Assert.NotNull(solution);
            Assert.NotEmpty(solution);
        }
    }
}