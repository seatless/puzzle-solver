using System;
using System.Linq;
using System.Runtime.InteropServices;
using PuzzleResolver.Service;

namespace PuzzleResolver.App
{
    class Program
    {
        private static readonly IResolver _puzzleResolver = new PuzzleResolverService();
        private const string PlayModeArg = "-play";
        private const string SolveModeArg = "-solve";
        
        static void Main(string[] args)
        {
            int[] puzzleInput;
            string puzzleMode;

            try
            {
                (puzzleInput, puzzleMode) = ParseArgs(args);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine($"Input error: " + e.Message);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error: " + e.Message);
                return;
            }
            
            var puzzle = new Puzzle(puzzleInput, PuzzleConstants.DefaultFinalConfiguration);

            Console.WriteLine("Initial puzzle state:");
            Console.WriteLine(puzzle.GetPrettyView());

            if (puzzleMode != PlayModeArg)
            {
                Console.WriteLine($"Started solving puzzle in automatic mode.");
                
                var solution = _puzzleResolver.Solve(puzzleInput);
                
                Console.WriteLine($"Puzzle solution is: {string.Join(",", solution)}.");
                Console.WriteLine("Click enter to exit.");
                Console.ReadLine();
                return;
            }
            
            Console.WriteLine($"Started solving puzzle in play mode. Type a number between 1 and 9 to make a move. Type exit to close application.");
            string input;
            while ((input = Console.ReadLine()) != "exit")
            {
                try
                {
                    if (int.TryParse(input, out var value))
                    {
                        puzzle.Move(value);
                        Console.WriteLine(puzzle.GetPrettyView());

                        if (puzzle.IsSolved())
                        {
                            Console.WriteLine("Victory!");
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Enter a number between 1 and 9 to make a move.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }

        private static (int[], string) ParseArgs(string[] args)
        {
            string puzzleInputString = "1234658970";
            string[] supportedModes = {SolveModeArg, PlayModeArg};
            string puzzleMode = supportedModes[0];
            
            if (args.Length > 2)
            {
                throw new ArgumentException($"Invalid input. Supported format: {puzzleInputString} [-play|-solve]");
            }
            if (args.Length != 0 && args[0].Length != 10)
            {
                throw new ArgumentException(
                    $"Invalid puzzle input length. Valid input example: {puzzleInputString}");
            }
            if (args.Length == 2)
            {
                if (!supportedModes.Contains(args[1]))
                    throw new ArgumentException($"Invalid second argument {args[1]}. Supported input format: {puzzleInputString} [{PlayModeArg}|{SolveModeArg}]");
                puzzleMode = args[1];
            }
            if (args.Any())
            {
                puzzleInputString = args[0];
            }

            var puzzleInput = puzzleInputString.Select(c => int.Parse(c.ToString())).ToArray();
            
            return (puzzleInput, puzzleMode);
        }
    }
}