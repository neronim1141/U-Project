using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
/// <summary>
/// Generation Module Class
/// </summary>
public class Module : MonoBehaviour
{
    [HideInInspector]
	public Module parent=null;
    [HideInInspector]
    public List<Module> childs =new List<Module>();

    /// <summary>
    /// rules for Module connections
    /// </summary>
    [SerializeField]
    private ModuleRules rules=null;
    public ModuleRules Rules{
        get{
           return rules;
        }
    }

    private GameObject body;
    public void ActivateBody(){
       // if(!body.activeSelf)
        //body.SetActive(true);
        
    }
    /// <summary>
    /// Connector list for removing purporses.
    /// because Destroy wait for Update;
    /// </summary>
    /// <typeparam name="ModuleConnector"></typeparam>
    /// <returns></returns>
    private List<ModuleConnector> _connectors= new List<ModuleConnector>();
    public List<ModuleConnector> Connectors{
        get{
            return _connectors;
        }
    }
    #region CollisionCheck
    Collider _collider;

    [Header("Collision Check")]
    /// <summary>
    /// center of checker
    /// </summary>
    public Vector3 center;
    /// <summary>
    /// size of checker
    /// </summary>
    public Vector3 size= Vector3.one;

    public virtual bool Collide(){
        body.SetActive(false);

        body.SetActive(true);
        Matrix4x4 m = Matrix4x4.Rotate(transform.rotation);
        // negative vector because Overlap makes to big box otherwise
        Collider[] hitColliders = Physics.OverlapBox(transform.position+m.MultiplyPoint3x4(center),
         ((transform.localScale+size) / 2)- new Vector3(0.52f,0.52f,0.52f),
          transform.rotation, LayerMask.GetMask("Module"));
         foreach(Collider hit in hitColliders){
             if(hit==_collider)continue;
            //   Vector3 otherPosition = hit.gameObject.transform.position;
            // Quaternion otherRotation = hit.gameObject.transform.rotation;

            // Vector3 direction;
            // float distance;

            // bool overlapped = Physics.ComputePenetration(
            //     _collider, transform.position, transform.rotation,
            //     hit, otherPosition, otherRotation,
            //     out direction, out distance
            // );
            // if(overlapped)
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        if(body==null)Init();
        //Draw Gizmo of HitCollider
        Gizmos.color = Collide()? Color.red:Color.green;
        //rotate gizmo
        Gizmos.matrix =transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero+center,size);
    }
    #endregion

    private void Awake() {
        Init();
        //disable body
       // body.SetActive(false);
       
    }
    private void Init(){
        body= transform.Find("Body").gameObject;
        //search and add connectors
        _connectors.AddRange(gameObject.GetComponentsInChildren<ModuleConnector>());
        _collider=body.GetComponent<Collider>();
    }

    /// <summary>
    /// Returns end Modules recursively
    /// </summary>
    /// <returns>
    /// Leafs of Module
    /// </returns>
    public List<Module> getLeafs(){
        //create empty list
        List<Module> Modules = new List<Module>();
        //if Module has childs
        if(childs.Count>0)
        foreach(Module n in childs){
            //if module have connectors
            if(n._connectors.Count>0)
                //add leafs to list
                Modules.AddRange(n.getLeafs());
        }
        //if module dosen't have childs add this module // end of recursion
        if(childs.Count==0)Modules.Add(this);
        return Modules;  
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns>return all modules in Tree</returns>
    public List<Module> getTreeModules(){
        //create empty list
        List<Module> Modules = new List<Module>();
        
        foreach(Module n in childs){
            Modules.AddRange(n.getTreeModules());
        }
        //if module dosen't have childs add this module // end of recursion
        Modules.Add(this);
        return Modules;  
    }
    /// <summary>
    /// remove collision Checker
    /// </summary>
    public void Clean(){
               // Destroy(_collider); 
              
    }
  



    

    
   
}