using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;
public class ModularWorldGenerator : MonoBehaviour
{
    //Singleton
    private static ModularWorldGenerator _instance;

    NavMeshDataInstance navMeshDataInstance;

    [SerializeField]
    Module StartModule;
    [SerializeField]
    PropSettings propSettings;
    public static PropSettings PropSettings{
        get{
            return _instance.propSettings;
        }
    }
    [SerializeField]
    MapSettings mapSettings;
    public static MapSettings MapSettings{
        get{
            return _instance.mapSettings;
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
        ModuleGenerator Mgenerator= new ModuleGenerator(mapSettings);
        _root = (Module)Instantiate(StartModule,transform.position,transform.rotation);
        // hook module to generator
        _root.transform.parent=transform;
        Mgenerator.Generate(_root,Iterations,seed);
        //CombineMesh();
        BuildNavMesh();
    }
    private void CombineMesh(){
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        meshFilters=meshFilters.Where(m=>m.gameObject.layer==9).ToArray();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;

        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            Destroy(meshFilters[i]);
            
             i++;
        }
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

    }
    private void BuildNavMesh(){
        List<NavMeshBuildSource> buildSources = new List<NavMeshBuildSource>();

        NavMeshBuilder.CollectSources(transform, LayerMask.GetMask("Module"), NavMeshCollectGeometry.PhysicsColliders, 0, new List<NavMeshBuildMarkup>(), buildSources);

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