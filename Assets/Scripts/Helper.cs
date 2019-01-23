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

    public static Weight<TItem> GetRandomWithWeights<TItem>(Weight<TItem>[] array){
            float pool= array.Sum(i=>i.weight);
            float randomNum= Random.Range(0,pool);
            float sum=0;
            for(int i=0;i<array.Length;i++){
                sum+=array[i].weight;
                if(sum>=randomNum){
                    return array[i];
                }
            }
            return null;
    }
    public static void MatchConnectors(Connector oldExit, Connector newExit)
    {
        //get parent of new Exit
        var newModule = newExit.transform.parent;
        // dalej sie w magiczy sposob przyrownują wyjścia XD
        var correctiveRotation =  Helper.Azimuth(-oldExit.toMatch) - Helper.Azimuth(newExit.toMatch);
        newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
       
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        newModule.transform.position += correctiveTranslation;

    }
    
}