using UnityEngine;
/// <summary>
/// Map Setting For Generator
/// </summary>
[CreateAssetMenu(fileName = "MapSettings", menuName = "MapGenerator/MapSettings", order = 0)]
public class MapSettings : ScriptableObject {
    // StartingModule
    public Module StartModule;
    //Close Module
    public Module CloseModule;
    // available modules
    public Module[] Modules;
    // available connectors
    public GameObject[] Connectors;
}