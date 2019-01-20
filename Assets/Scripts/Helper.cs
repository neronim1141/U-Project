using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
/// <summary>
/// Helper Class
/// </summary>
public static class Helper{

    /// <summary>
    /// Get one random object from params 
    /// </summary>
    /// <param name="array"> Array type from which you can draw</param>
    /// <typeparam name="TItem"></typeparam>
    /// <returns></returns>
    public static TItem GetRandom<TItem>(TItem[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
    public static float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }
}