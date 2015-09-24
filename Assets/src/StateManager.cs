using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    bool init = false;

	void Start() {

	}

	void Update() {
        //init once on update instead of start (to make sure all scripts have been initialised themselves)
        if (!init)
        {
            init = true;

            WaypointGrid.instance.init();
            EnemySpawner.instance.init();
        }
        WaypointGrid.instance.update();
        EnemySpawner.instance.update();
	}
}
