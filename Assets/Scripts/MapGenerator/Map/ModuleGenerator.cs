using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class ModuleGenerator : MonoBehaviour
{
    /// <summary>
    /// Generate new World
    /// </summary>
    public void Generate(Module root, int Iterations, int seed)
    {
        //Create RNG
        Random.State oldState = Random.state;
        Random.InitState(seed);
        // Create starting Module
        for (int i = 0; i < Iterations; i++)
        {
            // foreach end Module in tree
            foreach (Module m in root.getLeafs())
            {
                m.ActivateBody();
                foreach (ModuleConnector connector in m.Connectors)
                {
                    // copy avaible Modules
                    GenerateChild(connector, m);
                }
            }
        }
        CleanUp(root);
        //root.RebakeNavMesh();
        // revert RNG to old state;
        Random.state = oldState;
    }
    /// <summary>
    /// Generate Child on exit
    /// </summary>
    /// <param name="connector"> connector to match new module </param>
    private void GenerateChild(ModuleConnector connector, Module parent)
    {
        // copy avaible Modules
        List<Weight<Module>> Modules = new List<Weight<Module>>(ModularWorldGenerator.MapSettings.Modules);
        // filter modules that can connect to this module
        Modules = new List<Weight<Module>>(Modules.Where(n => n.item.Rules.connectTo.Contains(parent.Rules.type)));
        Module child = null;
        // while module doesn't sit
        while (child == null)
        {
            //if has Modules in pool get random module otherwise get close module
            Weight<Module> module=Helper.GetRandomWithWeights(Modules.ToArray());
            Modules.Remove(module);
            Module prefab = Modules.Count > 0 && module!=null ? module.item : ModularWorldGenerator.MapSettings.CloseModule;
            //remove prefab from pull

            child = TryPlaceModule(prefab, connector, parent);
            if(Modules.Count==0)break;
        }
        //activate child body;
        child.ActivateBody();

    }
    private Module TryPlaceModule(Module prefab, ModuleConnector connector, Module prarent)
    {
        Module child = (Module)Instantiate(prefab);
        //get connectors from new module
        ModuleConnector[] connectors = child.Connectors.ToArray();
        //get default or random connector from new module
        ModuleConnector childConnector = connectors.FirstOrDefault(x => x.IsDefault) ?? Helper.GetRandom(connectors);
        MatchExits(connector, childConnector);
        //get collider from new module
        //if new module can be placed
        if (!child.Collide())
        {
            //add new module to childs of parent
            prarent.childs.Add(child);
            // add parent to child
            child.parent = prarent;
            // with this you can fold branch of world
            child.transform.parent = transform;
            // remove connector
            // that is becouse Destroy wait for update;
            child.Connectors.Remove(childConnector);

            if (!(child is CloseModule))
                CreateConnector(connector);
            //removing unnecesarry objects
            Destroy(connector.gameObject);
            Destroy(childConnector.gameObject);
            child.Clean();
            return child;
        }
        else
        {
            Destroy(child.gameObject);
            return null;
        }
    }
    private void CreateConnector(ModuleConnector connector)
    {
        var obj = Instantiate(Helper.GetRandom(ModularWorldGenerator.MapSettings.Connectors), connector.transform.position, connector.transform.rotation);
        obj.transform.parent = connector.transform.parent;
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
    private float Azimuth(Vector3 vector)
    {
        return Vector3.Angle(Vector3.forward, vector) * Mathf.Sign(vector.x);
    }

    /// <summary>
    ///  Clean up after generation
    ///  e.g. Close all remaining Connectors
    /// </summary>
    public void CleanUp(Module root)
    {
        root.Clean();
        foreach (Module n in root.getLeafs())
        {
            // Destroy Collider
            // Close All available Connectors;
            foreach (ModuleConnector connector in n.Connectors)
            {
                 n.Clean();
                //create new connector
                Module close = ModularWorldGenerator.MapSettings.CloseModule;
                Module Module = (Module)Instantiate(close);
                //get First(and only) connector
                ModuleConnector newConnector = Module.Connectors[0];
                MatchExits(connector, newConnector);
                n.childs.Add(Module);
                Module.ActivateBody();
                Module.parent = n;
                Module.transform.parent = transform;
                Destroy(connector.gameObject);
                Destroy(newConnector.gameObject);
                
            }
        }
    }

}