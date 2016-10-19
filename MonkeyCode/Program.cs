using System;
using System.IO;
using System.Linq;

namespace MonkeyCode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("--- MonkeyCode ---");
            while (true)
            {
                try
                {
                    Console.Write("> ");
                    var inputString = Console.ReadLine();
                    if (string.IsNullOrEmpty(inputString)) break;
                    var scanner = new Scanner(inputString);
                    var tokenList = scanner.ScanAll().ToList();
                    var parser = new Parser(tokenList);
                    var semanticBlockList = parser.Parse();
                    var builder = new InstructionBuilder(semanticBlockList);
                    var instructions = builder.Build();
                    var asmGenerator = new IntelGenerator(instructions);
                    var code = asmGenerator.GenerateCode();
                    using (var w = new StreamWriter("./codefile.txt", true))
                    {
                        w.Write(";" + inputString + "\r\n" + code + "\r\n\r\n");
                    }
                    Console.WriteLine("Done.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}