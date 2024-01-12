using UnityEngine;

/// <summary>
/// A utility class containing helper methods.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Generates an array of unique random numbers.
    /// </summary>
    /// <param name="maxCount">The maximum number that can be generated.</param>
    /// <param name="n">The number of unique random numbers to generate.</param>
    /// <returns>An array of unique random numbers.</returns>
    public static int[] RandomNumerics(int maxCount, int n)
    {
        int[] defaults = new int[maxCount];
        int[] result = new int[n];

        for (int i = 0; i < maxCount; ++i)
        {
            defaults[i] = i;
        }
        for (int i = 0; i < n; ++i)
        {
            int index = Random.Range(0, maxCount);
            result[i] = defaults[index];
            defaults[index] = defaults[maxCount - 1];
            maxCount--;
        }
        return result;
    }
}