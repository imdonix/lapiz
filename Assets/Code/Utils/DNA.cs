using System.Collections;
using UnityEngine;


public static class DNA 
{

    private readonly static Color[] eyes = new Color[4] { Color.green, Color.cyan, Color.magenta, Color.grey };

    public static int GetRandom()
    {
        return Random.Range(0, int.MaxValue);
    }

    public static Color GetEye(int dna, EyeType type)
    {
        return eyes[(dna + (int) type) % eyes.Length];
    }
}
