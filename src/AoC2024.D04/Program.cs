using System.Text.RegularExpressions;

internal static class Program
{
    private static void Main(string[] args)
    {
        var wordPuzzle = ReadWordPuzzle();

        // search horizontally
        var wordOccurances = CountWordOccurances(wordPuzzle);

        // search vertically
        var transposedWordPuzzle = Transpose(wordPuzzle);
        wordOccurances += CountWordOccurances(transposedWordPuzzle);

        // search diagonally left
        var leftRotatedWordPuzzle = Rotate(wordPuzzle, Direction.Left);
        wordOccurances += CountWordOccurances(leftRotatedWordPuzzle);

        // search diagonally right
        var rightRotatedWordPuzzle = Rotate(wordPuzzle, Direction.Right);
        wordOccurances += CountWordOccurances(rightRotatedWordPuzzle);

        Console.WriteLine($"Word occurances in the word puzzle (Part 1): {wordOccurances}");
    }

    private static string[] ReadWordPuzzle()
    {
        const string path = "input.txt";

        try
        {
            return File.ReadAllLines(path);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error during file processing: {e.Message}");
            return [];
        }
    }

    private static int CountWordOccurances(string[] textLines)
    {
        const string word = "XMAS";
        var wordBackwards = new string(word.Reverse().ToArray());
        var occurance = 0;

        foreach (var line in textLines)
        {
            occurance += Regex.Matches(line, word).Count;
            occurance += Regex.Matches(line, wordBackwards).Count;
            //Console.WriteLine(line);
        }

        return occurance;
    }

    private static string[] Transpose(string[] wordPuzzle)
    {
        var rows = wordPuzzle.GetLength(0);
        var cols = wordPuzzle[0].Length;
        var transposedMatrix = new string[cols];

        for (int row = 0; row < rows; row++)
        {
            transposedMatrix[row] = string.Empty;
            for (int col = 0; col < cols; col++)
            {
                transposedMatrix[row] += wordPuzzle[col][row];
            }
        }

        return transposedMatrix;
    }

    private static string[] Rotate(string[] wordPuzzle, Direction direction)
    {
        var rows = wordPuzzle.Length;
        var cols = wordPuzzle[0].Length;
        var rotatedMatrix = new List<string>();

        int counter = 0;
        int limit = direction == Direction.Right ? (rows + cols - 1) : ((2 * rows) - 1);

        while (counter < limit)
        {
            var line = string.Empty;
            for (int row = 0; row < rows; row++)
            {
                int col = direction == Direction.Right ? counter - row : counter - (rows - 1) + row;
                if (col >= 0 && col < cols)
                {
                    line += wordPuzzle[row][col];
                }
            }

            counter++;
            rotatedMatrix.Add(line);
        }

        return rotatedMatrix.ToArray();
    }
}