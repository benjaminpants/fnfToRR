using System.Collections;
using FNFJSON;
using FNFJSON.Extensions;
using fnfToRR;


Console.Write("Enter in FNF Chart:");
string chartoread = Console.ReadLine();

string player1signalpath = Path.Combine(Environment.CurrentDirectory,"player1signals.txt");

string player2signalpath = Path.Combine(Environment.CurrentDirectory, "player2signals.txt");

AnimationsData Player1Signals = AnimationsData.CreateFromData(File.ReadAllText(player1signalpath));

AnimationsData Player2Signals = AnimationsData.CreateFromData(File.ReadAllText(player2signalpath));

Song song = Song.ReadFromJson(File.ReadAllText(chartoread));

Console.WriteLine();

Console.Write("Enter in WAV file to use:");

string audiotoread = Console.ReadLine();

rshwFormat f = new rshwFormat();

Console.WriteLine("Reading Audio File...");

f.audioData = File.ReadAllBytes(audiotoread);

Console.WriteLine("Audio File Read!");

f.videoData = new byte[0];

List<BitArray> bits = new List<BitArray>();

int steps_left_p1 = 0;

int steps_left_p2 = 0;
NoteType nt_p1 = (NoteType)(-1);
NoteType nt_p2 = (NoteType)(-1);
bool Player2 = false;


Console.WriteLine("Creating showtape data... (THIS MAY TAKE A WHILE SINCE ITS UNOPTIMIZED AT THE MOMENT.)");

for (int i = 0; i <= 10000; i++) //TODO: figure out how to properly calculate stuff lol
{
    BitArray bit = new BitArray(300);
    if (steps_left_p1 != 0)
    {
        steps_left_p1--;

        bit.Or(Player1Signals.animations[nt_p1.ToString().ToLower()]);
    }
    else
    {
        nt_p1 = (NoteType)(-1);
    }

    if (steps_left_p2 != 0)
    {
        steps_left_p2--;

        bit.Or(Player2Signals.animations[nt_p2.ToString().ToLower()]);
    }
    else
    {
        nt_p2 = (NoteType)(-1);
    }
    for (int s = 0; s < song.Sections.Count; s++)
    {
        for (int n = 0; n < song.Sections[s].sectionNotes.Count; n++)
        {
            Note note = new Note(song.Sections[s].sectionNotes[n]);
            if ((int)(note.StrumTime * 0.06) == i)
            {
                Player2 = !(note.GottaHit ? song.Sections[s].mustHitSection : !song.Sections[s].mustHitSection);
                if (Player2)
                {
                    steps_left_p2 = (int)(note.SustainLength * 0.06);
                    nt_p2 = note.NoteData;
                    if (steps_left_p2 == 0)
                    {
                        steps_left_p2 = 20;
                    }
                    bit.Or(Player2Signals.animations[nt_p2.ToString().ToLower()]);
                }
                else
                {
                    steps_left_p1 = (int)(note.SustainLength * 0.06);
                    nt_p1 = note.NoteData;
                    if (steps_left_p1 == 0)
                    {
                        steps_left_p1 = 20;
                    }
                    bit.Or(Player1Signals.animations[nt_p1.ToString().ToLower()]);
                }
            }
        }
    }

    bits.Add(bit);

}

Console.WriteLine("Showtape data created, converting to RR format...");

List<int> converted = new List<int>();
for (int i = 0; i < bits.Count; i++)
{
    converted.Add(0);
    for (int e = 0; e < 300; e++)
    {
        if (bits[i].Get(e))
        {
            converted.Add(e + 1);
        }
    }
}
f.signalData = converted.ToArray();

Console.WriteLine("Converted! Saving...");

f.Save(Path.Combine(Environment.CurrentDirectory, "output.rshw"));

Console.WriteLine("Showtape created, it should be called \"output.rshw\" in the folder where the EXE is.");

Console.WriteLine("Press any key to close the program.");

Console.ReadKey();
