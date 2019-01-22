using UnityEngine;
/// <summary>
/// Rules for connectivity
/// </summary>
[CreateAssetMenu(fileName = "MapRules", menuName = "MapGenerator/MapRules", order = 0)]
[System.Serializable]
public class ModuleRules : ScriptableObject {
    //Types of Module
    public enum Type{Room,Corridor,Default};
    //Types of Module
    public Type type;
    // types that Module can connect to
    public Type[] connectTo;
}