using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
[RequireComponent(typeof(ModuleGenerator))]
[RequireComponent(typeof(PropGenerator))]
public class ModularWorldGenerator : MonoBehaviour
{
    //Singleton
    private static ModularWorldGenerator _instance;

    public MapSettings _mapSettings;
    public static MapSettings MapSettings{
        get{
            return _instance._mapSettings;
        }
    }
    public PropSettings _propSettings;
    public static PropSettings PropSettings{
        get{
            return _instance._propSettings;
        }
    }
    public int seed=0;
    public static int Seed{
        get{
            return _instance.seed;
        }
    }
    private void Awake() {
        if(ModularWorldGenerator._instance==null){
            ModularWorldGenerator._instance=this;
        }
    }
    public int Iterations;
    private Module _root;
    private void Start(){
        ModuleGenerator Mgenerator= gameObject.GetComponent<ModuleGenerator>();
        PropGenerator Pgenerator= gameObject.GetComponent<PropGenerator>();
        _root = (Module)Instantiate(ModularWorldGenerator.MapSettings.StartModule);
        // hook module to generator
        _root.transform.parent=transform;
        Mgenerator.Generate(_root,Iterations,seed);
        Pgenerator.Generate(_root,seed);
    }
    
}