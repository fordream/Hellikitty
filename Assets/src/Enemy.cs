using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

    List<WaypointNode> path;

	void Start() {
        Vector2 dest = GameObject.Find("goal").transform.position;
        path = WaypointGrid.instance.find_path(transform.position, dest);
        foreach (WaypointNode n in path)
        {
            n.debug_draw_path = true;
        }
        Debug.Log(path.Count);
    }

    void Update() {

	}
}
