using UnityEngine;

public class Connector : MonoBehaviour {
    
    public virtual Vector3 toMatch{
		get{
			return transform.forward;
		}
	}
}