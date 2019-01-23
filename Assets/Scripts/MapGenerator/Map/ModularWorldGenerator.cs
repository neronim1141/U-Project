using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
[RequireComponent(typeof(ModuleGenerator))]
[RequireComponent(typeof(PropGenerator))]
public class ModularWorldGenerator : MonoBehaviour
{
    //Singleton
    private static ModularWorldGenerator _instance;

    NavMeshDataInstance navMeshDataInstance;

    [SerializeField]
    Module StartModule;
    
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
        _root = (Module)Instantiate(StartModule,transform.position,transform.rotation);
        // hook module to generator
        _root.transform.parent=transform;
        Mgenerator.Generate(_root,Iterations,seed);
        Pgenerator.Generate(_root,seed);
        BuildNavMesh();
    }
    private void BuildNavMesh(){
        List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();

        NavMeshBuilder.CollectSources(transform, LayerMask.GetMask("NavMesh"), NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), buildSources);

        NavMeshData navData = NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByID(0), buildSources,
                                new Bounds(Vector3.zero, new Vector3(10000, 10000, 10000)), Vector3.down,
                                Quaternion.Euler(Vector3.up)
                                );

        navMeshDataInstance = NavMesh.AddNavMeshData(navData);
    }
    private void OnDestroy() {
        navMeshDataInstance.Remove();
    }    
}