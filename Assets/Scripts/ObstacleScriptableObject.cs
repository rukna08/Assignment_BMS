
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Objects/Obstacle Data")]
public class ObstacleScriptableObject : ScriptableObject {
    public bool[] obstacle_data = new bool[100];
}
