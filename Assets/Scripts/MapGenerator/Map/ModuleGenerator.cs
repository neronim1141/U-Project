using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using System.Collections;

public class ModuleGenerator 
{
    public MapSettings _mapSettings;
    public ModuleGenerator(MapSettings settings){
        _mapSettings=settings;
    }
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
        List<Weight<Module>> Modules = new List<Weight<Module>>(_mapSettings.Modules);
        // filter modules that can connect to this module
        Modules = new List<Weight<Module>>(Modules.Where(n => n.item.Rules.connectTo.Contains(parent.Rules.type)));
        Module child = null;
        // while module doesn't sit
        while (child == null)
        {
            //if has Modules in pool get random module otherwise get close module
            Weight<Module> module=Helper.GetRandomWithWeights(Modules.ToArray());
            Modules.Remove(module);
            Module prefab = Modules.Count > 0 && module!=null ? module.item : _mapSettings.CloseModule;
            //remove prefab from pull

            child = TryPlaceModule(prefab, connector, parent);
            if(Modules.Count==0)break;
        }

    }
    private Module TryPlaceModule(Module prefab, ModuleConnector connector, Module prarent)
    {
        Module child = (Module)GameObject.Instantiate(prefab);
        //get connectors from new module
        ModuleConnector[] connectors = child.Connectors.ToArray();
        //get default or random connector from new module
        ModuleConnector childConnector = connectors.FirstOrDefault(x => x.IsDefault) ?? Helper.GetRandom(connectors);
        Helper.MatchConnectors(connector, childConnector);
        //get collider from new module
        //if new module can be placed
        ColliderDetection detect=child.GetComponent<ColliderDetection>();
        if ((detect &&!detect.Collide())||child is CloseModule)
        {
            //add new module to childs of parent
            prarent.childs.Add(child);
            // add parent to child
            child.parent = prarent;
            // with this you can fold branch of world
            child.transform.parent = GameObject.FindGameObjectWithTag("MapGenerator").transform;;
            // remove connector
            // that is becouse Destroy wait for update;
            child.Connectors.Remove(childConnector);

            if (!(child is CloseModule))
                CreateConnector(connector);
            //removing unnecesarry objects
            GameObject.Destroy(connector.gameObject);
            GameObject.Destroy(childConnector.gameObject);
            PropGenerator PG = new PropGenerator(ModularWorldGenerator.PropSettings);
            PG.GenerateProps(child);
            return child;
        }
        else
        {
            GameObject.Destroy(child.gameObject);
            return null;
        }
    }
    private void CreateConnector(ModuleConnector connector)
    {
        var obj = GameObject.Instantiate(Helper.GetRandom(_mapSettings.Connectors), connector.transform.position, connector.transform.rotation);
        obj.transform.parent = connector.transform.parent;
    }


    /// <summary>
    ///  Clean up after generation
    ///  e.g. Close all remaining Connectors
    /// </summary>
    public void CleanUp(Module root)
    {
        foreach (Module n in root.getLeafs())
        {
            // Destroy Collider
            // Close All available Connectors;
            foreach (ModuleConnector connector in n.Connectors)
            {
                //create new connector
                Module close = _mapSettings.CloseModule;
                Module Module = (Module)GameObject.Instantiate(close);
                //get First(and only) connector
                ModuleConnector newConnector = Module.Connectors[0];
                Helper.MatchConnectors(connector, newConnector);
                n.childs.Add(Module);
                Module.parent = n;
                Module.transform.parent = GameObject.FindGameObjectWithTag("MapGenerator").transform;;
                GameObject.Destroy(connector.gameObject);
                GameObject.Destroy(newConnector.gameObject);
                
            }
        }
        foreach (Module n in root.getTreeModules())
        {
            n.Clean();
        }
    }

    

}