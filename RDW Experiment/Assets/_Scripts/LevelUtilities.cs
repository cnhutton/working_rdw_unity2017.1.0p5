using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelUtilities
{
    public static void WriteToFile(string path, string text)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
    }

    //min(inclusive) max(exclusive)
    public static int GenerateRandomInt(int minRange, int maxRange)
    {
        return Random.Range(minRange, maxRange);
    }

    public static bool GenerateRandomBool()
    {
        return Random.Range(0, 2) > 0;
    }

    public static Vector3 FlattenVector3(Vector3 vector)
    {
        return new Vector3(vector.x, 0, vector.z);
    }

    public static Corner ChooseRandomCorner()
    {
        int value = GenerateRandomInt(0, 4);
        switch (value)
        {
            case 0:
                return Corner.Northeast;
            case 1:
                return Corner.Northwest;
            case 2:
                return Corner.Southeast;
            case 3:
                return Corner.Southwest;
            default:
                Debug.LogWarning("Random int Generator failed, using default value");
                return Corner.Northwest;
        }
        
    }

    public static Corner EndpointCorner(Corner start, bool turnLeft)
    {
        switch (start)
        {
            case Corner.Northeast:
                if (turnLeft)
                    return Corner.Southeast;
                return Corner.Northwest;

            case Corner.Southeast:
                if (turnLeft)
                    return Corner.Southwest;
                return Corner.Northeast;

            case Corner.Southwest:
                if (turnLeft)
                    return Corner.Northwest;
                return Corner.Southeast;

            case Corner.Northwest:
                if (turnLeft)
                    return Corner.Northeast;
                return Corner.Southwest;

            default:
                throw new ArgumentOutOfRangeException("start", start, null);
        }
    }

    
}
