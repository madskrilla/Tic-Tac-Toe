using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNumberUtil
{

    public static int GetRandomValue(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static int GetRandomValue(List<int> range)
    {
        int randIndex = Random.Range(0, range.Count - 1);
        return range[randIndex];
    }


    public static float GetRandomValue(float min, float max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public static List<int> GetRandomValues(int min, int max, int numValues, bool uniqueValues = true, List<int> exclusions = null)
    {
        List<int> result = new List<int>();
        int value = 0;
        bool validValue = false;

        for (int i = 0; i < numValues; i++)
        {
            while (validValue == false)
            {
                value = GetRandomValue(min, max);
                if (exclusions != null)
                {
                    validValue = !exclusions.Contains(value);
                }
                if (uniqueValues)
                {
                    validValue = validValue && !result.Contains(value);
                }
            }

            result.Add(value);
            validValue = false;
        }

        return result;
    }
}
