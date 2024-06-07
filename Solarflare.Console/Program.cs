// See https://aka.ms/new-console-template for more information
using Solarflare.Compiler;
using System.Drawing;

Console.WriteLine("Welcome to the Solar Flare REPL!");

var evaluator = new Evaluator();

while (true)
{
    Console.Write("> ");
    var expression = Console.ReadLine();

    var parser = new Parser(expression);
    var tree = parser.GenerateTree();

    if (!parser.Errors.Any())
    {
        var result = evaluator.Evaluate(tree);
        Console.WriteLine(result);
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;

        foreach (var error in parser.Errors)
        {
            Console.WriteLine(error);
        }

        Console.ResetColor();
            ;
    }
}
