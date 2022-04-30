using Godot;
using MazeGenerators;
using MazeGenerators.Utils;

namespace DodgeTheCreeps.Utils
{
    public static class MazeGeneratorWrapper
    {
        public static int[,] Generate()
        {
            var result = new GeneratorResult();
            var settings = new GeneratorSettings
            {
                Height = 11,
                Width = 11,
                MazeText =
                "###########\n" +
                "#.........#\n" +
                "#.........#\n" +
                "#.........#\n" +
                "#.........#\n" +
                "####..#####\n" +
                "#.........#\n" +
                "#.........#\n" +
                "#.........#\n" +
                "#.........#\n" +
                "###########\n"
            };

            CommonAlgorithm.GenerateField(result, settings);
            StringParserAlgorithm.Parse(result, settings);
            GD.Print(StringParserAlgorithm.Stringify(result, settings));

            return result.Paths;
        }
    }
}
