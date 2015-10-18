using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    public Area[] areas;

	void Start() {
        GetComponent<Renderer>().enabled = false;

        foreach (Area area in areas) {
            if (area == null) {
                Debug.LogError("Trigger (" + name + ") area is null, please select the elements in the editor");
            }
        }
        if (areas.Length == 0) {
            Debug.Log("Trigger (" + name + ") contains no area elements. Please add them in the editor");
        }
	}

	void Update() {

	}

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject == Entities.player.gameObject) {
            foreach (Area area in areas) {
                if (area != null) {
                    area.trigger();
                }
            }
        }
    }
}
