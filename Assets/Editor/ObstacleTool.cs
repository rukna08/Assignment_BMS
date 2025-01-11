
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ObstacleScriptableObject))]
public class ObstacleTool : Editor {

    public ObstacleScriptableObject obstacle_data_script;
    
    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        for (int i = 0; i < 10; i++) {
            GUILayout.BeginHorizontal();
            for(int j = 0; j < 10; j++) {
                if (GUILayout.Button(i + "," + j)) {
                    int index = i * 10 + j;
                    obstacle_data_script.obstacle_data[index] = !obstacle_data_script.obstacle_data[index];
                }    
            }
            GUILayout.EndHorizontal();
        }
    }

}
