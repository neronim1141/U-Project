using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
/// <summary>
/// Generation Module Class
/// </summary>
public class Module : MonoBehaviour
{

	private Module parent=null;
    private List<Module> childs =new List<Module>();

    /// <summary>
    /// rules for Module connections
    /// </summary>
    [SerializeField]
    private ModuleRules rules;

    [SerializeField]
    private GameObject body=null;
    /// <summary>
    /// Connector list for removing purporses.
    /// because Destroy wait for Update;
    /// </summary>
    /// <typeparam name="ModuleConnector"></typeparam>
    /// <returns></returns>
    private List<ModuleConnector> _connectors= new List<ModuleConnector>();
    [SerializeField]
    Collider _collider;

    private void Awake() {
        //disable body
        body.SetActive(false);
        //search and add connectors
        _connectors.AddRange(gameObject.GetComponentsInChildren<ModuleConnector>());
    }
    private void Start(){
        body.SetActive(true);
        GenerateProps();
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
    /// <returns></returns>
    public List<Module> getChilds(){
        //create empty list
        List<Module> Modules = new List<Module>();
        
        foreach(Module n in childs){
            Modules.AddRange(n.getChilds());
        }
        //if module dosen't have childs add this module // end of recursion
        Modules.Add(this);
        return Modules;  
    }
    /// <summary>
    /// Clean up Module;
    /// </summary>
    public void CleanUp(){
        //Destroy Collider
        Clean();

        // Close All available Connectors;
        foreach (ModuleConnector connector in _connectors)
        {
            //create new connector
            Module close= ModularWorldGenerator.MapSettings.CloseModule;
            Module Module = (Module)Instantiate(close);
            //get First(and only) connector
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
    public void GenerateProps(){
 
        foreach(PropConnector entityConnector in gameObject.GetComponentsInChildren<PropConnector>()){
            Debug.Log("Create Prop");

                List<Prop> entities= new List<Prop>(ModularWorldGenerator.PropSettings.entities);
                // filter modules that can connect to this module
                entities = new List<Prop>(entities.Where(e=>e.connector.type==entityConnector.type));
                entities.Add(null);
                Prop entity=Helper.GetRandom(entities.ToArray());
                if(entity!=null){
                    entity= (Prop)Instantiate(entity);
                    entity.transform.parent=body.transform;
                    MatchExits(entityConnector,entity.connector);
                    Destroy(entityConnector.gameObject);
                    Destroy(entity.connector.gameObject);
                }else{
                    Destroy(entityConnector.gameObject);
                }
        }
    }
    /// <summary>
    /// Generate new Modules on Exits
    /// </summary>
    public void GenerateNewModulesOnLeaf(){
        body.SetActive(true);
        foreach (ModuleConnector connector in _connectors)
        {
            GenerateChild(connector);
        }
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
    /// true if module collide with something
    /// false otherwise
    /// </returns>
    private bool Collide(){
         ModuleCollider collider= gameObject.GetComponent<ModuleCollider>();
         if(!collider) return false;
         return collider.Collide();
    }
    /// <summary>
    /// Generate Child on exit
    /// </summary>
    /// <param name="connector"> connector to match new module </param>
    private void GenerateChild(ModuleConnector connector){
        // copy avaible Modules
        List<Module> Modules= new List<Module>(ModularWorldGenerator.MapSettings.Modules);
        // filter modules that can connect to this module
        Modules = new List<Module>(Modules.Where(m=>m.rules.connectTo.Contains(rules.type)));
        Module child=null;
        // while module doesn't sit
        while(child==null)
        {
                //if has Modules in pool get random module otherwise get close module
                Module prefab=Modules.Count>0?prefab=Helper.GetRandom(Modules.ToArray()):ModularWorldGenerator.MapSettings.CloseModule;
                //remove prefab from pull
                Modules.Remove(prefab);
                //place prefab in world
                child= (Module)Instantiate(prefab);
                //get connectors from new module
                ModuleConnector[] connectors=child._connectors.ToArray();
                //get default or random connector from new module
                ModuleConnector childConnector=connectors.FirstOrDefault(x => x.IsDefault) ?? Helper.GetRandom(connectors);
                MatchExits(connector,childConnector);
                //get collider from new module
                ModuleCollider collider= child.GetComponent<ModuleCollider>();
                //if new module can be placed
                if(!child.Collide()){
                    //add new module to childs of parent
                    childs.Add(child);
                    // add parent to child
                    child.parent=this;
                    // with this you can fold branch of world
                    child.transform.parent=transform;
                    // remove connector
                    // that is becouse Destroy wait for update;
                    child._connectors.Remove(childConnector);

                    if(!(child is CloseModule))
                         // connect two modules
                         CreateConnector(connector);
                    //removing unnecesarry objects
                    Destroy(connector.gameObject);
                    Destroy(childConnector.gameObject);
                    Clean();
                    child.Clean();
                }else{
                    //destroy object;
                   Destroy(child.gameObject);
                   //set child to null for next random
                    child=null;
                }
        }
        //activate child body;
        child.body.SetActive(true);

    }
    private void CreateConnector(ModuleConnector connector){
            var obj =Instantiate(Helper.GetRandom(ModularWorldGenerator.MapSettings.Connectors),connector.transform.position,connector.transform.rotation);
            obj.transform.parent=connector.transform.parent;
    }
    /// <summary>
    /// Match Exits of two gameobject with exits
    /// </summary>
    /// <param name="oldExit"> exit of first Module</param>
    /// <param name="newExit">  exit of second Module</param>
    private void MatchExits(ModuleConnector oldExit, ModuleConnector newExit)
    {
        //get parent of new Exit
        var newModule = newExit.transform.parent;
        // dalej sie w magiczy sposob przyrownują wyjścia XD
        var forwardVectorToMatch = -oldExit.transform.forward;
        var correctiveRotation = Azimuth(forwardVectorToMatch) - Azimuth(newExit.transform.forward);
        newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotation);
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        newModule.transform.position += correctiveTranslation;

    }
    private void MatchExits(PropConnector oldExit, PropConnector newExit)
    {
        //get parent of new Exit
        var newModule = newExit.transform.parent;
        // dalej sie w magiczy sposob przyrownują wyjścia XD
        var rightVectorToMatch = oldExit.transform.right;
        var correctiveRotationRight = Azimuth(rightVectorToMatch) - Azimuth(newExit.transform.right);
        newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotationRight);
       
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        newModule.transform.position += correctiveTranslation;

    }

    private float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }
}