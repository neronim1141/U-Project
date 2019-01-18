using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ModularWorldGenerator : MonoBehaviour
{

    private static ModularWorldGenerator _instance;
    public static System.Action CleanUp;
    [SerializeField]
    private MapSettings _mapSettings=null;
    public static MapSettings MapSettings{
        get {return _instance._mapSettings;}
    }
    public int Iterations = 5;
    public int seed;


    void Start()
    {
        // create singleton
        if(ModularWorldGenerator._instance==null)
            ModularWorldGenerator._instance=this;
        StartCoroutine(GenerateMap());
    }
    IEnumerator GenerateMap()
    {
        Random.State oldState = Random.state;
        Random.InitState(seed);
        var startModule = (Module)Instantiate(_mapSettings.StartModule, transform.position, transform.rotation);
        startModule.transform.parent = transform;
        var pendingExits = new List<ModuleConnector>(startModule.GetExits());
        yield return new WaitForSeconds(2);
        for (int iteration = 0; iteration < Iterations; iteration++)
        {
            var newExits = new List<ModuleConnector>();

            foreach (var pendingExit in pendingExits)
            {
                var newTag = GetRandom(pendingExit.Tags);
                //TODO: Check if can be placed
                List<Module> tmpModules = new List<Module>(_mapSettings.Modules);
                Module newModule = null;
                ModuleConnector exitToMatch = null;
                ModuleConnector[] newModuleExits = new ModuleConnector[0];
                while (newModule == null)
                {
                    Module newModulePrefab = GetRandomWithTag(tmpModules, newTag);
                    tmpModules.Remove(newModulePrefab);
                    newModule = (Module)Instantiate(newModulePrefab);
                    newModuleExits = newModule.GetExits();

                    exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
                    MatchExits(pendingExit, exitToMatch);


                    yield return new WaitForFixedUpdate();
                    if (!newModule.body.clear)
                    {
                        Destroy(newModule.gameObject);
                        newModule = null;
                    }
                }
                newModule.transform.parent = transform;
                if (newModule.name != "CloseModule(Clone)")
                {
                    var connector = Instantiate(GetRandom(_mapSettings.Connectors), pendingExit.transform.position, pendingExit.transform.rotation);
                    connector.gameObject.transform.parent = transform;
                }
                newExits.AddRange(newModuleExits.Where(e => e != exitToMatch));
                Destroy(pendingExit);
                Destroy(exitToMatch);
            }

            pendingExits = newExits;
        }
        CleanAfter(pendingExits);
        Random.state = oldState;
        yield return null;
    }
    private void CleanAfter(List<ModuleConnector> pendingExits){
         foreach (var pendingExit in pendingExits)
        {
            var newModule = (Module)Instantiate(_mapSettings.CloseModule);
            var newModuleExits = newModule.GetExits();
            var exitToMatch = newModuleExits.FirstOrDefault(x => x.IsDefault) ?? GetRandom(newModuleExits);
            MatchExits(pendingExit, exitToMatch);
             Destroy(pendingExit);
            newModule.transform.parent = transform;

        }
        foreach (Module m in GetComponentsInChildren<Module>())
        {
            m.body.removeComponents();
            m.ActivateObjects();
        }
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


    private TItem GetRandom<TItem>(TItem[] array)
    {
        return array[Random.Range(0, array.Length)];
    }


    private Module GetRandomWithTag(IEnumerable<Module> modules, string tagToMatch)
    {
        var matchingModules = modules.Where(m => m.Tags.Contains(tagToMatch)).ToArray();
        if (matchingModules.Length > 0)
            return GetRandom(matchingModules);
        else
        {
            return _mapSettings.CloseModule;
        }
    }


    private float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }
}