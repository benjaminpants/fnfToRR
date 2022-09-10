using System.Collections;
using FNFJSON;


Console.WriteLine("Welcome to FNF2RR.");

Console.WriteLine("Version: 0.2");

Console.WriteLine("Press the key that coresponds to what you want to do.");

Console.WriteLine("1) Convert FNF Chart to RR Showtape");

Console.WriteLine("2) Convert RR Showtape to FNF Chart");

ConsoleKeyInfo fo = Console.ReadKey();

switch (fo.Key)
{
    case ConsoleKey.D1:
        Console.Write("Enter in FNF Chart:");
        string chartoread = Console.ReadLine();

        Console.Write("Enter in Audio File(WAV):");
        string audiotoread = Console.ReadLine();

        fnfToRR.ConvertFNF(chartoread, audiotoread);

        break;
    case ConsoleKey.D2:
        break;
}

Console.WriteLine("Press any key to close the program.");

Console.ReadKey();
