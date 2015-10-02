using UnityEngine;
using System.Collections;

public class Area : MonoBehaviour {

	void Start() {
        GetComponent<Renderer>().enabled = false;
	}

	void Update() {
	
	}

    public void trigger() {
        GetComponent<Renderer>().enabled = true;
    }
}
