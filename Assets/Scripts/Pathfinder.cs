
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {
    
    public int grid_width  = 10;
    public int grid_height = 10;
    public Transform player;
    public Transform enemy;
    public ObstacleManager obstacle_manager;
    public GameManager game_manager;

    bool[,] obstacle_data;
    bool is_data_initialised = false;

    int current_path_index = 0;
    List<Vector2Int> current_path;

    int current_enemy_path_index = 0;
    List<Vector2Int> current_enemy_path;
    
    public float movement_speed = 5f;

    // Node class for path chains.
    class Node {
        public int x, y;
        public int g, h;
        public Node parent;

        public Node(int x, int y) => (this.x, this.y) = (x, y);
        public int f => g + h;
    }

    // Value for calculating the total weight.
    int heuristic(int x1, int y1, int x2, int y2) => Mathf.Abs(x1 - x2) + Mathf.Abs(y1 - y2);

    bool is_valid(int x, int y, int max_x, int max_y) => x >= 0 && x < max_x && y >= 0 && y < max_y;

    // Finds the List of Coordinate paths and returns it.
    List<Vector2Int> find_path(Func<int, int, bool> is_obstacle, int max_x, int max_y, int start_x, int start_y, int target_x, int target_y) {
        if (!is_valid(start_x, start_y, max_x, max_y) || !is_valid(target_x, target_y, max_x, max_y) || is_obstacle(target_x, target_y)) {
            return new List<Vector2Int>();
        }
        Node start_node = new Node(start_x, start_y);
        List<Node> open_set = new List<Node> { start_node };
        HashSet<Node> closed_set = new HashSet<Node>();

        while (open_set.Count > 0) {
            Node current = open_set[0];
            foreach (Node node in open_set) {
                if (node.f < current.f)
                    current = node;
            }

            open_set.Remove(current);
            closed_set.Add(current);

            if (current.x == target_x && current.y == target_y) {
                List<Vector2Int> path = new List<Vector2Int>();
                while (current != null) {
                    path.Add(new Vector2Int(current.x, current.y));
                    current = current.parent;
                }
                path.Reverse();
                return path;
            }

            foreach (var (dx, dy) in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) }) {
                int nx = current.x + dx;
                int ny = current.y + dy;

                if (is_valid(nx, ny, max_x, max_y) && !is_obstacle(nx, ny)) {
                    Node neighbor = new Node(nx, ny);
                    if (closed_set.Contains(neighbor)) {
                        continue;
                    }

                    neighbor.g = current.g + 1;
                    neighbor.h = heuristic(nx, ny, target_x, target_y);
                    neighbor.parent = current;

                    if (!open_set.Contains(neighbor) || neighbor.g < current.g) {
                        if (open_set.Contains(neighbor)) {
                            open_set.Remove(neighbor);
                        }
                        open_set.Add(neighbor);
                    }
                }
            }
        }
        return new List<Vector2Int>();
    }

    // Checks if there is any obstacle in this x and y.
    bool is_obstacle(int x, int y) {
        if (obstacle_data == null || y < 0 || y >= obstacle_data.GetLength(0) || x < 0 || x >= obstacle_data.GetLength(1)) {
            return true;
        }
        return obstacle_data[x, y];
    }

    void Start() {
        
        StartCoroutine(initialise_obstacle_data());

        // Sets the initial positions of the player and enemy if they are somewhere else.

        if (player != null) {
            player.position = new Vector3(0f, player.position.y, 0f);
        }

        if (enemy != null) {
            enemy.position = new Vector3(0f, enemy.position.y, 9f);
        }
    }

    // Uses the Obstacle Data which is a boolean matrix to determine obstacle positions.
    IEnumerator initialise_obstacle_data() {
        while (obstacle_manager == null || obstacle_manager.obstacle_data == null) {
            yield return null;
        }

        obstacle_data = obstacle_manager.obstacle_data;
        if (obstacle_data != null) {
            grid_height = obstacle_data.GetLength(0);
            grid_width = obstacle_data.GetLength(1);

            string obstacleString = "";
            for (int y = 0; y < grid_height; y++) {
                for (int x = 0; x < grid_width; x++) {
                    obstacleString += obstacle_data[y, x] ? "1" : "0";
                    obstacleString += " ";
                }
                obstacleString += "\n";
            }
        }
        
        is_data_initialised = true;
    }

    bool is_player_moving = false;

    void Update() {
        if (!is_data_initialised || player == null || obstacle_data == null || game_manager == null) return;

        // Mouse click to move the player to the highlighted position.
        if (Input.GetMouseButtonDown(0) && !is_player_moving) {
            is_player_moving = true;

            if (obstacle_data.GetLength(0) != grid_height || obstacle_data.GetLength(1) != grid_width) {
                grid_height = obstacle_data.GetLength(0);
                grid_width = obstacle_data.GetLength(1);
            }

            Vector2Int player_grid_pos = new Vector2Int(Mathf.FloorToInt(player.position.x), Mathf.FloorToInt(player.position.z));
            Vector2Int enemy_grid_pos = new Vector2Int(Mathf.FloorToInt(enemy.position.x), Mathf.FloorToInt(enemy.position.z));

            // The path for the player and enemy to travel respectively.
            List<Vector2Int> path = find_path(is_obstacle, grid_width, grid_height, player_grid_pos.x, player_grid_pos.y, (int)game_manager.hovered_point.x, (int)game_manager.hovered_point.y);
            List<Vector2Int> enemy_path = find_path(is_obstacle, grid_width, grid_height, enemy_grid_pos.x, enemy_grid_pos.y, player_grid_pos.x, player_grid_pos.y);

            if (path != null && path.Count > 0) {
                current_path = path;
                current_path_index = 0;
            } else {
                current_path = null;
            }

            if (enemy_path != null && enemy_path.Count > 0) {
                current_enemy_path = path;
                current_enemy_path_index = 0;
            } else {
                current_enemy_path = null;    
            }
        }

        

        if (current_path != null && current_path_index < current_path.Count) {
            Vector3 target_position = new Vector3(current_path[current_path_index].x, player.position.y, current_path[current_path_index].y);
            player.position = Vector3.MoveTowards(player.position, target_position, movement_speed * Time.deltaTime);
            
            // Enable Gizmos in the editor for displaying movement line.
            if(is_player_moving) {
                Debug.DrawLine(player.position, target_position, Color.green);
            }

            if (Vector3.Distance(player.position, target_position) < 0.01f) {
                current_path_index++;
                if (current_path_index >= current_path.Count) {
                    current_path = null;
                    is_player_moving = false;
                }
            }
        }

        if(!is_player_moving) {
            if (current_enemy_path != null && current_enemy_path_index < current_enemy_path.Count) {
                Vector3 target_position = new Vector3(current_enemy_path[current_enemy_path_index].x, enemy.position.y, current_enemy_path[current_enemy_path_index].y);
                enemy.position = Vector3.MoveTowards(enemy.position, target_position, movement_speed * Time.deltaTime);


                // <= 1f so that the enemy stops at any 1 block adjacent to the player.
                if(Vector3.Distance(enemy.position, target_position) <= 1f) {
                    current_enemy_path_index++;
                    if (current_enemy_path_index >= current_enemy_path.Count) {
                        current_enemy_path = null;    
                    }
                }
            }
        }
    }
}