using System;
using System.IO;
using System.Linq;
using System.Threading;

namespace MonkeyCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("--- MonkeyCode ---");
            if (args.Count() == 1 && args[0] == "-i")
            {
                EnterInteractiveMode();
            }
            else if (args.Count() == 2)
            {
                EnterFileMode(args[0], args[1]);
            }
            else
            {
                Console.Error.Write("Missing source/target filenames or interactive argument");
                Thread.Sleep(2000);
            }
        }

        private static void EnterFileMode(string inputPath, string outputPath)
        {
            var inputString = File.ReadAllText(inputPath);
            CompileString(inputString, outputPath);
        }

        private static void EnterInteractiveMode()
        {
            while (true)
            {
                try
                {
                    Console.Write("> ");
                    var inputString = Console.ReadLine();
                    if (string.IsNullOrEmpty(inputString)) break;
                    CompileString(inputString, "./codefile.asm");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void CompileString(string inputString, string outputPath)
        {
            Console.WriteLine("Started.");
            var scanner = new Scanner(inputString);
            var tokenList = scanner.ScanAll().ToList();
            var parser = new Parser(tokenList);
            var semanticBlockList = parser.Parse();
            var builder = new InstructionBuilder(semanticBlockList);
            var instructions = builder.Build();
            var asmGenerator = new IntelGenerator(instructions);
            var code = asmGenerator.GenerateCode();
            using (var w = new StreamWriter(outputPath, true))
                w.Write(code);
            Console.WriteLine("Done.");
        }
    }
}