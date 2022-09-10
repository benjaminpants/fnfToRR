using FNFJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class fnfToRR
{

    static void ApplyFunny(Song song) //This code is from a really old unreleased project. I've just decided to recycle it
    {
        Random rng = new Random();
        List<string> patterns = new List<string>
        {
        "lrlrududludrrdldurur",
        "ururldldurldurdldludrl",
        "dduullrruullrrdduuddllrrlldrl",
        "rldlrldlrudurududrlrud",
        "dudulrlr",
        "dduddrdudrlurrrddlu",
        "dudulrddudurrdd",
        "udlrludururluld",
        "dduddruuduul",
        "drludrludrlu",
        "rurudldlurdlurldululdrdl",
        "udrudrudludldurdurduldul",
        "ululdduldduldul",
        "dudulrrlduuddldrduulur",
        "dddduuuullllrrrr",
        "uuuullllrrrrdddd",
        "uuddllrrdduurrll",
        "ldldururllddlldduurruurr",
        "lruulruudlrrdlrr",
        "dudduurrllrlrldurldurllurldurd",
        "ludludludludrudrudurdurdrulrulrulrul",
        "dllurrlddrddlrrrll",
        "dldudurdurdldur",
        "lurldrlurldrludludddlrr",
        "lduldurlduldur",
        "dlruudlruudllruuddlru",
        "lrldlu",
        "ldldldurururur",
        "ddluurlluurrd",
        "rlrldduululrrd",
        "dulrludluddulrdrdldu"
        };
        int yes = 0;
        int currentpattern = rng.Next(0, patterns.Count - 1);
        int currentoffset = rng.Next(0, 30);
        bool last = false;
        foreach (Section sect in song.Sections)
        {
            yes = currentoffset;
            if (last != sect.mustHitSection)
            {
                if (!sect.mustHitSection)
                {
                    currentpattern = rng.Next(0, patterns.Count - 1);
                    currentoffset = rng.Next(0, patterns[currentpattern].Length);
                    //Console.WriteLine(song.Player2Char + " turn detected, changing pattern to pattern #" + currentpattern);
                }
            }
            last = sect.mustHitSection;
            List<Note> notes = new List<Note>();
            //Note[] notez = sect.ConvertSectionToNotes();
            /*for (int i=0; i < notez.Length; i++)
            {
                
            }*/
            foreach (Note note in sect.ConvertSectionToNotes())
            {
                switch (patterns[currentpattern][yes % (patterns[currentpattern].Length - 1)])
                {
                    case 'l':
                        note.NoteData = NoteType.Left;
                        break;
                    case 'r':
                        note.NoteData = NoteType.Right;
                        break;
                    case 'u':
                        note.NoteData = NoteType.Up;
                        break;
                    case 'd':
                        note.NoteData = NoteType.Down;
                        break;
                    default:
                        note.NoteData = NoteType.Left;
                        break;
                }
                yes++;
                notes.Add(note);
            }
            sect.SaveNotes(notes);
        }
    }

    public static bool ConvertRR(string showtapetoread, int mouthsignal, int opponentmouthsignal)
    {
        rshwFormat thefile = rshwFormat.ReadFromFile(showtapetoread);
        List<BitArray> newSignals = new List<BitArray>();
        int countlength = 0;
        if (thefile.signalData[0] != 0)
        {
            countlength = 1;
            BitArray bit = new BitArray(300);
            newSignals.Add(bit);
        }
        for (int i = 0; i < thefile.signalData.Length; i++)
        {
            if (thefile.signalData[i] == 0)
            {
                countlength += 1;
                BitArray bit = new BitArray(300);
                newSignals.Add(bit);
            }
            else
            {
                newSignals[countlength - 1].Set(thefile.signalData[i] - 1, true);
            }
        }

        List<Note> notes = new List<Note>();

        for (int i = 0; i < newSignals.Count; i++)
        {
            if (newSignals[i].Get(mouthsignal))
            {
                //this is incomplete
            }
        }

        return true;
    }


    public static bool ConvertFNF(string chartoread, string audiotoread)
    {
        try
        {
            string player1signalpath = Path.Combine(Environment.CurrentDirectory, "player1signals.txt");

            string player2signalpath = Path.Combine(Environment.CurrentDirectory, "player2signals.txt");

            //string player3signalpath = Path.Combine(Environment.CurrentDirectory, "player3signals.txt");

            AnimationsData Player1Signals = AnimationsData.CreateFromData(File.ReadAllText(player1signalpath));

            AnimationsData Player2Signals = AnimationsData.CreateFromData(File.ReadAllText(player2signalpath));

            //AnimationsData Player3Signals = AnimationsData.CreateFromData(File.ReadAllText(player3signalpath));

            Song song = Song.ReadFromJson(File.ReadAllText(chartoread));

            Console.WriteLine();

            Console.Write("Enter in WAV file to use:");

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


            Console.WriteLine("Creating showtape data...");

            int lastsectionval = song.Sections.Count - 1;

            Section lastsection = song.Sections[lastsectionval];

            Note lastnote = null;

            while (lastnote == null)
            {
                if (lastsectionval == 0)
                {
                    throw new IndexOutOfRangeException("Chart has no notes??? WTF are you doing.");
                }
                if (lastsection.sectionNotes.Count == 0)
                {
                    lastsectionval--;
                    lastsection = song.Sections[lastsectionval];
                }
                else
                {
                    lastnote = new Note(lastsection.sectionNotes.Last());
                }
            }


            int furthest_bit = (int)((lastnote.StrumTime + lastnote.SustainLength + 100) * 0.06) + 25;

            for (int i = 0; i <= furthest_bit; i++) //TODO: figure out how to properly calculate stuff lol
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

        }
        catch (Exception E)
        {
            Console.WriteLine(E);
            return false;
        }


        return true;

    }


}
