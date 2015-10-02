using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    bool init = false;

	void Start() {
        WaypointGrid.instance.init();
	}

	void Update() {
        //recalc waypoint nodes after all other scripts have been initialised
        //this is because raycast2d cannot run in the start call
        if (!init)
        {
            init = true;

            WaypointGrid.instance.recalc_waypoint_nodes();
        }
        WaypointGrid.instance.update();
	}
}
