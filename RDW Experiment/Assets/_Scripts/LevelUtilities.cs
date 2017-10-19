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

    public static Edge ChooseRandomCorner()
    {
        int value = GenerateRandomInt(0, 4);
        switch (value)
        {
            case 0:
                return Edge.Northeast;
            case 1:
                return Edge.Northwest;
            case 2:
                return Edge.Southeast;
            case 3:
                return Edge.Southwest;
            default:
                Debug.LogWarning("Random int Generator failed, using default value");
                return Edge.Northwest;
        }
        
    }

    public static Edge EndpointCorner(Edge start, bool turnLeft)
    {
        switch (start)
        {
            case Edge.Northeast:
                if (turnLeft)
                    return Edge.Southeast;
                return Edge.Northwest;

            case Edge.Southeast:
                if (turnLeft)
                    return Edge.Southwest;
                return Edge.Northeast;

            case Edge.Southwest:
                if (turnLeft)
                    return Edge.Northwest;
                return Edge.Southeast;

            case Edge.Northwest:
                if (turnLeft)
                    return Edge.Northeast;
                return Edge.Southwest;

            default:
                throw new ArgumentOutOfRangeException("start", start, null);
        }
    }

    
}
