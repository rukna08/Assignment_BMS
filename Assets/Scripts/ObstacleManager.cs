
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
    
    public ObstacleScriptableObject obstacle_data_script;
    public GameObject obstacle;
    public GameObject[,] obstacle_matrix;
    public bool[,] obstacle_data;

    void Start() {
        obstacle_matrix = new GameObject[10, 10];
        obstacle_data = new bool[10, 10];
        for(int i = 0; i < 10; i++) {
            for(int j = 0; j < 10; j++) {
                obstacle_data[i, j] = false;
            }
        }
        spawn_obstacles();
    }

    // Spawns the obstacles in the game mode by pulling the data from
    // the ObstacleScriptableObject.
    void spawn_obstacles() {
        for(int i = 0; i < 10; i++) {
            for(int j = 0; j < 10; j++) {
                int index = i * 10 + j;
                if (obstacle_data_script.obstacle_data[index] == true) {
                    //Debug.LogFormat("Enemy Spawned at: ({0},{1})", i, j);
                    obstacle_matrix[i, j] = Instantiate(obstacle, this.transform);
                    obstacle_matrix[i, j].transform.position = new Vector3(i, 1f, j);
                    obstacle_data[i, j] = true;
                }
            }
        }
    }

}
