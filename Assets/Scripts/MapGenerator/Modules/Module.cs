using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System;
/// <summary>
/// Generation Module Class
/// </summary>
[RequireComponent(typeof(ColliderDetection))]
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

    private void Awake() {
        transform.Find("Objects").gameObject.SetActive(false);
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
              transform.Find("Objects").gameObject.SetActive(true);
              MeshCollider collider=  gameObject.GetComponent<MeshCollider>();
              if(collider){
                  collider.convex=false;
              }else{
                  Destroy(gameObject.GetComponent<Collider>());
              }
              
    }
  



    

    
   
}