
using UnityEngine;

public class Cube : MonoBehaviour {
    
    public int i_index;
    public int j_index;

    void Start() {
        i_index = (int)transform.position.x;
        j_index = (int)transform.position.z;
    }

    public void print_info() {
        Debug.Log("[i,j]:[" +  i_index + "," +  j_index + "]");    
    }
}
