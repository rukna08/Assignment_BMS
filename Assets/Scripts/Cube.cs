
using UnityEngine;

public class Cube : MonoBehaviour {
    

    // Cube Position in Cube Grid.
    public int i_index;
    public int j_index;

    void Start() {
        i_index = (int)transform.position.x;
        j_index = (int)transform.position.z;
    }
}
