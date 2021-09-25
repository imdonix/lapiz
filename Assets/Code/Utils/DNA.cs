using System.Collections;
using UnityEngine;


public static class DNA 
{

    private readonly static Color[] eyes = new Color[4] { Color.green, Color.cyan, Color.magenta, Color.grey };
    private readonly static string[] names = new string[5] { "Mito", "Linkir", "Nijara", "Vikara", "Luizee" }; 

    public static int GetRandom()
    {
        return Random.Range(0, int.MaxValue);
    }

    internal static string GetName(int dna)
    {
        return names[dna % names.Length];
    }

    public static Color GetEye(int dna, EyeType type)
    {
        return eyes[(dna + (int) type) % eyes.Length];
    }
}
