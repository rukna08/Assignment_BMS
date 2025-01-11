
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject cube;
    public GameObject[,] cube_matrix;
    public GameObject grid;
    public TMP_Text tile_info_text;
    public Vector2 hovered_point;

    void Start() {
        generate_grid();
    }

    void Update() {
        show_tile_information();
    }

    // Generates Cube Grid
    public void generate_grid() {
        if(grid.transform.childCount == 0) {
            cube_matrix = new GameObject[10, 10];
            for(int i = 0; i < 10; i++) {
                for(int j = 0; j < 10; j++) {
                    cube_matrix[i, j] = Instantiate(cube, grid.transform);
                    cube_matrix[i, j].transform.position = new Vector3(i, 0f, j);
                }
            }  
        } else {
            Debug.Log("Grid is already generated.");
        }
    }

    public void clear_grid() {
        if(grid.transform.childCount != 0) {
            for (int i = 0; i < 10; i++) {
                for (int j = 0; j < 10; j++) {
                    DestroyImmediate(cube_matrix[i, j]);
                }
            } 
        } else {
            Debug.Log("Grid is already clear");    
        }
    }

    // Shows the currently hovered tile information in a Text UI Element.
    void show_tile_information() { 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            for(int i = 0; i < 10; i++) {
                for(int j = 0; j < 10; j++) {
                    if (cube_matrix[i, j] == hit.transform.gameObject) {
                        cube_matrix[i, j].GetComponent<Renderer>().material.color = Color.cyan;
                        tile_info_text.text = "Selected Tile: " + i + ", " + j;
                        hovered_point = new Vector2(i, j);
                    } else {
                        cube_matrix[i, j].GetComponent<Renderer>().material.color = Color.white;
                    }
                }        
            }
        }
    }

    void OnApplicationQuit() {
        clear_grid();
    }

}
