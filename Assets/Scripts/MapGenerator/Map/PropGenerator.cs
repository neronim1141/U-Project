using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class PropGenerator : MonoBehaviour {
    
    public void Generate(Module root,int seed){
        Random.State oldState=Random.state;
        Random.InitState(seed);
        foreach (Module m in root.getTreeModules())
        {
            GenerateProps(m);
        }
       Random.state=oldState;

    }

    public void GenerateProps(Module m){
 
        foreach(PropConnector entityConnector in m.gameObject.GetComponentsInChildren<PropConnector>()){
            Debug.Log("Create Prop");

                List<Prop> entities= new List<Prop>(ModularWorldGenerator.PropSettings.entities);
                // filter modules that can connect to this module
                entities = new List<Prop>(entities.Where(e=>e.connector.type==entityConnector.type));
                entities.Add(null);
                Prop entity=Helper.GetRandom(entities.ToArray());
                if(entity!=null){
                    entity= (Prop)Instantiate(entity);
                    entity.transform.parent=transform;
                    MatchExits(entityConnector,entity.connector);
                    Destroy(entityConnector.gameObject);
                    Destroy(entity.connector.gameObject);
                }else{
                    Destroy(entityConnector.gameObject);
                }
        }
    }

     private void MatchExits(PropConnector oldExit, PropConnector newExit)
    {
        //get parent of new Exit
        var newModule = newExit.transform.parent;
        // dalej sie w magiczy sposob przyrownują wyjścia XD
        var rightVectorToMatch = oldExit.transform.right;
        var correctiveRotationRight =  Helper.Azimuth(rightVectorToMatch) - Helper.Azimuth(newExit.transform.right);
        newModule.RotateAround(newExit.transform.position, Vector3.up, correctiveRotationRight);
       
        var correctiveTranslation = oldExit.transform.position - newExit.transform.position;
        newModule.transform.position += correctiveTranslation;

    }

    

}