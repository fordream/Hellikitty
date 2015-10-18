using UnityEngine;
using System.Collections;

public class InitScript : MonoBehaviour {

	void Start() {
        Debug.Log("initialising singletons...");

        WaypointGrid.instance.init();
        Player.instance.init();
	}

	void Update() {
	
	}
}
