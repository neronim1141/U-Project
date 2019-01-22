using UnityEngine;

public class Prop : MonoBehaviour {

    public PropConnector connector{
        get{
            return gameObject.GetComponentInChildren<PropConnector>();
        }
    }
}