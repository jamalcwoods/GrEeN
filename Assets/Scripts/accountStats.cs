using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class accountStats
{
    public static int resource = 0;
    public static List<int> ownedPassives = new List<int>();
    public static List<int> abilties = new List<int>() { 0, 0, 0};
    public static List<int> passives = new List<int>() { 0, 0, 0};
    public static int record = 0;
    public static int transferRate = 1;

    public static void Save()
    {
        Stream stream = File.OpenWrite("userlog.acc");
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(resource.ToString());
        writer.Write(record.ToString());
        string oP = "";
        for(int i = 0;i<ownedPassives.Count;i++)
        {
            oP += ownedPassives[i];
            if (i + 1 < ownedPassives.Count)
            {
                oP += "|";
            }
        }
        if(oP == "")
        {
            oP = "0|";
        }
        writer.Write(oP);
        string p = "";
        for (int i = 0; i < passives.Count; i++)
        {
            p += passives[i];
            if(i + 1 < passives.Count)
            {
                p += "|";
            }
        }
        writer.Write(p);
        writer.Close();
    }
}