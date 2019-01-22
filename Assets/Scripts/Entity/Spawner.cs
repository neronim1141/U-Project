using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Enemy enemyPrefab;
    public float spawnTime = 3f; 
    // Start is called before the first frame update
    void Start()
    {
         InvokeRepeating ("Spawn", spawnTime, spawnTime);
    }

     void Spawn ()
    {
        if(GameManager.MaxEnemiesNumber>0){
            Instantiate (enemyPrefab, transform.position, transform.rotation);
            GameManager.MaxEnemiesNumber--;
        }
        // Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.
    }
    private void OnDrawGizmos() {
        Gizmos.color=Color.gray;
        Gizmos.DrawSphere(transform.position,0.5f);    
    }
}
