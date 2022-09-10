using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class AnimationsData
{
    public Dictionary<string, BitArray> animations = new Dictionary<string, BitArray>();


    public static AnimationsData CreateFromData(string data)
    {
        string[] splitdata = data.Split("\n");
        for (int i = 0; i < splitdata.Length; i++)
        {
            splitdata[i] = splitdata[i].Replace("\r", "");
            splitdata[i] = splitdata[i].Replace("\n", "");
        }
        string current_animation = "none";
        AnimationsData animd = new AnimationsData();
        BitArray curbits = new BitArray(300);
        for (int i = 0; i < splitdata.Length; i++)
        {
            bool parsed = int.TryParse(splitdata[i], out int sig);
            if (!parsed)
            {
                if (current_animation != "none")
                {
                    animd.animations.Add(current_animation, curbits);
                    curbits = new BitArray(300);
                }
                current_animation = splitdata[i];
            }
            else
            {
                curbits.Set(sig,true);
            }
        }
        animd.animations.Add(current_animation, curbits);

        return animd;
    }
}
