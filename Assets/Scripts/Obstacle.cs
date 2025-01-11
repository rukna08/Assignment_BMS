
using UnityEngine;

public class Obstacle : MonoBehaviour {
    
    void Start() {
        gameObject.GetComponent<Renderer>().material.color = Color.red;    
    }
}
