using UnityEngine;
[CreateAssetMenu(fileName = "MapRules", menuName = "procedural dungeon/MapRules", order = 0)]
[System.Serializable]
public class ModuleRules : ScriptableObject {
    public enum Type{Room,Corridor,Default};
    public Type type;
    public Type[] connectTo;
}