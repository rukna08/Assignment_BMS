
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Scriptable Objects/Obstacle Data")]
public class ObstacleScriptableObject : ScriptableObject {
    // Unity doesn't Serialize 2D Arrays. So used 10x10=100 size of array.
    // index of 1D Array = i * 10 + j where i, j are indices of a 2D Array.
    public bool[] obstacle_data = new bool[100];
}
