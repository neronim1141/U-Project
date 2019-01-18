using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCollision : MonoBehaviour
{
    private List<Collider> TriggerList = new List<Collider>();
    public bool clear{
        get{
            return TriggerList.Count==0;
        }
    }

   public void removeComponents(){
       
        Destroy(gameObject.GetComponent<Rigidbody>());
        Destroy(gameObject.GetComponent<MeshCollider>());
    }

 //called when something enters the trigger
 void OnTriggerEnter(Collider other)
 {
     //if the object is not already in the list

     if(!TriggerList.Contains(other))
     {
         //add the object to the list
         TriggerList.Add(other);
     }
 }
 
 //called when something exits the trigger
void OnTriggerExit(Collider other)
 {
     //if the object is in the list
     if(TriggerList.Contains(other))
     {
         //remove it from the list
         TriggerList.Remove(other);
     }
 }
}
