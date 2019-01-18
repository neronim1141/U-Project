using UnityEngine;

public class Module : MonoBehaviour
{
	public string[] Tags;
    [SerializeField]
    public ModuleCollision body;
    public GameObject objects;

    public void ActivateObjects(){
        if(objects!=null){
            objects.SetActive(true);
        }
    }

	public ModuleConnector[] GetExits()
	{
		return GetComponentsInChildren<ModuleConnector>();
	}
}