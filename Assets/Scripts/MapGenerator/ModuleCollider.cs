using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCollider : MonoBehaviour
{
   public LayerMask m_LayerMask;
    public Vector3 center;
    public Vector3 size= Vector3.one;

    public bool Collide(){
        Collider[] hitColliders = Physics.OverlapBox(transform.position+center, ((transform.localScale+size) / 2)- new Vector3(0.51f,0.51f,0.51f), transform.rotation, m_LayerMask);
        return hitColliders.Length>0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Collide()? Color.red:Color.green;
        Gizmos.matrix =transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero+center,size);
    }
}
