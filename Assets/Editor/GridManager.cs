
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GridManager : Editor {

    public override void OnInspectorGUI() {

        base.OnInspectorGUI();
        
        GameManager game_manager_script = (GameManager)target;
        
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Generate Grid")) {
            game_manager_script.generate_grid();
        }
        if(GUILayout.Button("Clear Grid")) {   
            game_manager_script.clear_grid();
        }
        GUILayout.EndHorizontal();
    }
}
