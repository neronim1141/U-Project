using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    public int Iterations;
    public int seed=0;
    private void Awake() {
        if(ModularWorldGenerator._instance==null){
            ModularWorldGenerator._instance=this;
        }
    }
    //root of tree;
    private Module _root;
    // Start is called before the first frame update
    void Start()
    {
         _root = (Module)Instantiate(_mapSettings.StartModule);
        // hook module to generator
        _root.transform.parent=transform;
        Generate(_root);
    }
    /// <summary>
    /// Generate new World
    /// </summary>
    void Generate(Module root){
        //Create RNG
        Random.State oldState=Random.state;
        Random.InitState(seed);
        // Create starting Module
       
        for(int i=0;i<Iterations;i++)
        {
            // foreach end Module in tree
            foreach (Module n in root.getLeafs())
            {
                // create new Module
                n.GenerateNewModulesOnLeaf();
                // hook module to generator
                n.transform.parent=transform;

            }
        }
       CleanUp(root);
       foreach(Module n in root.getChilds()){
           n.GenerateProps();
       }
       // revert RNG to old state;
       Random.state=oldState;
    }

    /// <summary>
    ///  Clean up after generation
    ///  e.g. Close All Left Connectors
    /// </summary>
    public void CleanUp(Module root){
            root.Clean();
            foreach (Module n in root.getLeafs())
            {
                n.CleanUp();
                n.transform.parent=transform;
            }
    }
}