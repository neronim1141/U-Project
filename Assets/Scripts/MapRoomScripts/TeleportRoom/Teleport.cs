using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class Teleport : MonoBehaviour
{
    private static Teleport t=null;
    [SerializeField]
    Teleport paired=null;
    [SerializeField]
    List<Collider> queue= new List<Collider>();
    public bool autoTeleport=true;
    public bool active=true;
    [SerializeField]
    float prepareTeleport=2f;
    [SerializeField]
    private float time=0;
    // Start is called before the first frame update
    void Start()
    {
        if(t==null)t=this;
        else{
            t.paired=this;
            paired=t;
            t=null;
        }
    }

    // Update is called once per frame
     void Update()
    {
            if(paired && active && queue.Count>0){
                time+=Time.deltaTime;
                gameObject.GetComponent<Light>().color=Color.blue;
                if(time>prepareTeleport){
                    while(queue.Count>0){
                        queue[0].transform.position=paired.transform.position;
                        queue.RemoveAt(0);
                    }
                    active=false;
                }
            }
            else{
                time=0;
                gameObject.GetComponent<Light>().color=Color.green;
            }
            if(!paired){
                gameObject.GetComponent<Light>().color=Color.red;
            }
            if(autoTeleport)active=true;
        }
    private void OnDestroy() {
        if(paired){
            paired.paired=null;
        }
    }

    private void OnTriggerEnter(Collider other) {
        queue.Add(other);
    }
    // private void OnTriggerStay(Collider other) { 
    //     Debug.Log(other.name);
    // }
    private void OnTriggerExit(Collider other) {
       queue.Remove(other);

    }
    private void OnDrawGizmosSelected() {

        Gizmos.DrawIcon(transform.position,"Teleport.png",true);
        if(paired){
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, paired.transform.position);

        }
    }
}
