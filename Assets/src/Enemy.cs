using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	void Start() {
        Vector2 dest = GameObject.Find("goal").transform.position;
        WaypointGrid.instance.find_path(transform.position, dest);
	}

    void Update() {

	}
}
