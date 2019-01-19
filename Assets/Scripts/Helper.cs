using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public static class Helper{


    public static TItem GetRandom<TItem>(TItem[] array)
    {
        return array[Random.Range(0, array.Length)];
    }
}