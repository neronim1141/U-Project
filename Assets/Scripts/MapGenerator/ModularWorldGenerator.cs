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
    public int Iterations;
    public int seed=0;
    private void Awake() {
        if(ModularWorldGenerator._instance==null){
            ModularWorldGenerator._instance=this;
        }
    }
    //root of tree;
    private Module root;
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }
    /// <summary>
    /// Generate new World
    /// </summary>
    void Generate(){
        //Create RNG
        Random.State oldState=Random.state;
        Random.InitState(seed);
        // Create starting Module
        root = (Module)Instantiate(_mapSettings.StartModule);
        // hook module to generator
        root.transform.parent=transform;
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
       CleanUp();

       // revert RNG to old state;
       Random.state=oldState;
    }

    /// <summary>
    ///  Clean up after generation
    ///  e.g. Close All Left Connectors
    /// </summary>
    public void CleanUp(){
            root.Clean();
            foreach (Module n in root.getLeafs())
            {
                n.CleanUp();
                n.transform.parent=transform;
            }
    }
}