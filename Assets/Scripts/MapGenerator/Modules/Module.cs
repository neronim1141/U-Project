using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// Generation Module Class
/// </summary>
public class Module : MonoBehaviour
{

	public Module parent=null;
    public List<Module> childs =new List<Module>();

    /// <summary>
    /// rules for Module connections
    /// </summary>
    [SerializeField]
    private ModuleRules rules;
    public ModuleRules Rules{
        get{
           return rules;
        }
    }

    [SerializeField]
    private GameObject body=null;
    public void ActivateBody(){
        body.SetActive(true);
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
    [SerializeField]
    Collider _collider;

    private void Awake() {
        //disable body
        body.SetActive(false);
        //search and add connectors
        _connectors.AddRange(gameObject.GetComponentsInChildren<ModuleConnector>());
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
                ModuleCollider collider= gameObject.GetComponent<ModuleCollider>();
                Destroy(collider); 
                Destroy(_collider); 

    }

    /// <summary>
    /// Collision Check
    /// </summary>
    /// <returns>
    /// true if module collide with something,
    /// false otherwise
    /// </returns>
    public bool Collide(){
         ModuleCollider collider= gameObject.GetComponent<ModuleCollider>();
         if(!collider) return false;
         return collider.Collide();
    }

    

    
   
}