using UnityEngine;

[CreateAssetMenu(fileName = "MapSettings", menuName = "procedural dungeon/MapSettings", order = 0)]
public class MapSettings : ScriptableObject {
    public Module StartModule;
    public Module CloseModule;
    public Module[] Modules;
    public GameObject[] Connectors;
}