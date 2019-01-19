using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Module : MonoBehaviour
{
	private Module parent=null;
    [SerializeField]
    private ModuleRules rules;
    private List<Module> childs =new List<Module>();

    [SerializeField]
    private GameObject body=null;
    private List<ModuleConnector> _connectors= new List<ModuleConnector>();
    [SerializeField]
    Collider _collider;
    private void Awake() {
        body.SetActive(false);
        _connectors.AddRange(gameObject.GetComponentsInChildren<ModuleConnector>());
    }

    public List<Module> getLeafs(){
        List<Module> Modules = new List<Module>();
        if(childs.Count>0)
        foreach(Module n in childs){
            if(n._connectors.Count>0)
                Modules.AddRange(n.getLeafs());
        }
        if(childs.Count==0)Modules.Add(this);
        return Modules;  
    }
    public void CleanUp(){
        Destroy(_collider);
        foreach (ModuleConnector connector in _connectors)
        {
            Module close= ModularWorldGenerator.MapSettings.CloseModule;
            Module Module = (Module)Instantiate(close);
            ModuleConnector newConnector = Module._connectors[0];
            MatchExits(connector,newConnector);
            childs.Add(Module);
            Module.body.SetActive(true);
            Module.parent=parent;
            Module.transform.parent=transform;
            Destroy(connector.gameObject);
            Destroy(newConnector.gameObject);
        }
    }
    public void GenerateNewModulesOnLeaf(){
        body.SetActive(true);
        foreach (ModuleConnector connector in _connectors)
        {
            GenerateChild(connector);
        }
    }
    public void Clean(){
                ModuleCollider collider= gameObject.GetComponent<ModuleCollider>();
                Destroy(collider); 

    }
    private bool Collide(){
         ModuleCollider collider= gameObject.GetComponent<ModuleCollider>();
         if(!collider) return false;
         return collider.Collide();
    }
    private void GenerateChild(ModuleConnector connector){
        // copy avaible Modules
        List<Module> Modules= new List<Module>(ModularWorldGenerator.MapSettings.Modules);
        Modules = new List<Module>(Modules.Where(m=>m.rules.connectTo.Contains(rules.type)));
        Module child=null;
        while(child==null)
        {
                Module prefab=Modules.Count>0?prefab=Helper.GetRandom(Modules.ToArray()):ModularWorldGenerator.MapSettings.CloseModule;
                Modules.Remove(prefab);

                child= (Module)Instantiate(prefab);
                ModuleConnector[] connectors=child._connectors.ToArray();
                ModuleConnector childConnector=connectors.FirstOrDefault(x => x.IsDefault) ?? Helper.GetRandom(connectors);
                MatchExits(connector,childConnector);
                ModuleCollider collider= child.GetComponent<ModuleCollider>();
                if(!child.Collide()){
                    childs.Add(child);
                    child.parent=this;
                    child.transform.parent=transform;
                    child._connectors.Remove(childConnector);
                    if(!(child is CloseModule))
                         CreateConnector(connector);
                    Destroy(connector.gameObject);
                    Destroy(childConnector.gameObject);
                    Destroy(collider); 
                    break;
                }else{
                   Destroy(child.gameObject);
                    child=null;
                }
        }

        if(child!=null){
        child.body.SetActive(true);
        Destroy(child._collider);
        }
    }
    private void CreateConnector(ModuleConnector connector){
            var obj =Instantiate(Helper.GetRandom(ModularWorldGenerator.MapSettings.Connectors),connector.transform.position,connector.transform.rotation);
            obj.transform.parent=connector.transform.parent;
    }

    private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
    {
        var newModule = newExit.transform.parent;
        var forwardVectorToMatch = -oldExit.transform.forward;
        var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
        newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        newModule.transform.position += correctiveTranslation;

    }

    private float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }
}