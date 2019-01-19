using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ModularWorldGenerator : MonoBehaviour
{

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
    private Module root;
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }
    void Generate(){
        Random.State oldState=Random.state;
        Random.InitState(seed);
        root = (Module)Instantiate(_mapSettings.StartModule);
        root.transform.parent=transform;
        for(int i=0;i<Iterations;i++)
        {
            foreach (Module n in root.getLeafs())
            {
                n.GenerateNewModulesOnLeaf();
                n.transform.parent=transform;

            }
        }
       CleanUp();
       Random.state=oldState;
    }
    public void CleanUp(){
            root.Clean();
            foreach (Module n in root.getLeafs())
            {
                n.CleanUp();
                n.transform.parent=transform;
            }
    }
}