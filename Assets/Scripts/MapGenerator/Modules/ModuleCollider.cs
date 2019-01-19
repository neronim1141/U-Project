using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuleCollider : MonoBehaviour
{
    /// <summary>
    /// mask to check collision
    /// </summary>
    public LayerMask m_LayerMask;
    /// <summary>
    /// center of checker
    /// </summary>
    public Vector3 center;
    /// <summary>
    /// size of checker
    /// </summary>
    public Vector3 size= Vector3.one;

    public bool Collide(){
        // negative vector because Overlap makes to big box otherwise
        Collider[] hitColliders = Physics.OverlapBox(transform.position+center, ((transform.localScale+size) / 2)- new Vector3(0.51f,0.51f,0.51f), transform.rotation, m_LayerMask);
        return hitColliders.Length>0;
    }

    void OnDrawGizmos()
    {
        //Draw Gizmo of HitCollider
        Gizmos.color = Collide()? Color.red:Color.green;
        //rotate gizmo
        Gizmos.matrix =transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero+center,size);
    }
}
