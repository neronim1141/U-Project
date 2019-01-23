using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PropGenerator  {
    private PropSettings _propSettings;

    public PropGenerator(PropSettings settings){
        _propSettings=settings;
    }

    // public void Generate(Module root,int seed){
    //     Random.State oldState=Random.state;
    //     Random.InitState(seed);
    //     foreach (Module m in root.getTreeModules())
    //     {
    //         GenerateProps(m);
    //     }
    //    Random.state=oldState;

    // }
    public  void GenerateProps(Module m){
        foreach(PropConnector entityConnector in m.gameObject.GetComponentsInChildren<PropConnector>()){

                List<Prop> entities= new List<Prop>(_propSettings.entities);
                // filter modules that can connect to this module
                entities = new List<Prop>(entities.Where(e=>e.connector.type==entityConnector.type));
                entities.Add(null);
                Prop entity=Helper.GetRandom(entities.ToArray());
                if(entity!=null){
                    entity= (Prop)GameObject.Instantiate(entity);
                    entity.transform.parent=GameObject.FindGameObjectWithTag("MapGenerator").transform;
                    Helper.MatchConnectors(entityConnector,entity.connector);
                    GameObject.Destroy(entityConnector.gameObject);
                    GameObject.Destroy(entity.connector.gameObject);
                }else{
                    GameObject.Destroy(entityConnector.gameObject);
                }
        }
    }
}