using UnityEngine;
/// <summary>
/// Map Setting For Generator
/// </summary>
[CreateAssetMenu(fileName = "MapSettings", menuName = "MapGenerator/MapSettings", order = 0)]
public class MapSettings : ScriptableObject {
    //Close Module
    public Module CloseModule;
    // available modules
    [SerializeField]
    public WeightModule[] Modules;
    // available connectors
    public GameObject[] Connectors;
}
[System.Serializable]
public class Weight<T>{
    public T item;
    [Range(0,1)]
    public float weight=1;
}
[System.Serializable]
public class WeightModule:Weight<Module>{}