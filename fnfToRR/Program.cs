using System.Collections;
using FNFJSON;


Console.WriteLine("Welcome to FNF2RR.");

Console.WriteLine("Version: 0.2");

Console.WriteLine("Press the key that coresponds to what you want to do.");

Console.WriteLine("1) Convert FNF Chart to RR Showtape");

Console.WriteLine("2) Convert RR Showtape to FNF Chart");

Console.WriteLine("3) Extract audio from RR Showtape");

ConsoleKeyInfo fo = Console.ReadKey();

switch (fo.Key)
{
    case ConsoleKey.D1:
        Console.WriteLine("Enter in FNF Chart:");
        string chartoread = Console.ReadLine();

        Console.Write("Enter in Audio File(WAV):");
        string audiotoread = Console.ReadLine();

        fnfToRR.ConvertFNF(chartoread, audiotoread);

        break;
    case ConsoleKey.D2:
        Console.WriteLine("Enter in RR showtape:");
        string shtoread = Console.ReadLine();

        Console.Write("Enter in Player 2 Signal(BF):");
        int bf = int.Parse(Console.ReadLine());

        Console.Write("Enter in Player 1 Signal(Dad):");
        int dad = int.Parse(Console.ReadLine());

        Console.Write("Chart Generation Seed:");
        int seed = int.Parse(Console.ReadLine());

        fnfToRR.ConvertRR(shtoread, bf, dad, seed);

        break;
    case ConsoleKey.D3:
        Console.Write("Enter in RR showtape:");
        string shtoextract = Console.ReadLine();

        rshwFormat thefile = rshwFormat.ReadFromFile(shtoextract);

        File.WriteAllBytes(Path.Combine(Environment.CurrentDirectory, "output.wav"),thefile.audioData);

        Console.WriteLine("Wav extracted, it should be called \"output.wav\" in the folder where the EXE is.");
        break;
}

Console.WriteLine("Press any key to close the program.");

Console.ReadKey();
