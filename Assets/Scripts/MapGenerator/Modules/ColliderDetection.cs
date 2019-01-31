using UnityEngine;

public class ColliderDetection : MonoBehaviour {

    /// <summary>
    /// center of checker
    /// </summary>
    public Vector3 center;
    /// <summary>
    /// size of checker
    /// </summary>
    public float radius;

    public bool Collide(){
        Collider _collider = gameObject.GetComponent<Collider>();
       _collider.enabled=false;
       _collider.enabled=true;

        // negative vector because Overlap makes to big box otherwise
        Collider[] hitColliders = Physics.OverlapSphere(transform.position,
        radius, LayerMask.GetMask("Module"));
         foreach(Collider hit in hitColliders){
             if(hit==_collider)continue;
              Vector3 otherPosition = hit.gameObject.transform.position;
            Quaternion otherRotation = hit.gameObject.transform.rotation;

            Vector3 direction;
            float distance;

            bool overlapped = Physics.ComputePenetration(
                _collider, transform.position, transform.rotation,
                hit, otherPosition, otherRotation,
                out direction, out distance
            );
            decimal dist= System.Decimal.Parse(distance.ToString(),
                                  System.Globalization.NumberStyles.Float);
            if(dist>0.101m)
            return true;
        }
        return false;
    }
     private void OnDrawGizmosSelected()
    {
        Gizmos.color=new Color(1, 1, 1, 0.3f);
        Gizmos.DrawWireSphere(transform.position+center,radius);
        var col=gameObject.GetComponent<MeshCollider>();
        if(col&&col.convex){
            //Draw Gizmo of HitCollider
            Gizmos.color = Collide()? Color.red:Color.green;
            //rotate gizmo
            Gizmos.DrawWireMesh(gameObject.GetComponent<MeshCollider>().sharedMesh,transform.position,transform.rotation);
        }
    }
}