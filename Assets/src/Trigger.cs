using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

    public Area[] areas;

	void Start() {
        GetComponent<Renderer>().enabled = false;
	}

	void Update() {

	}

    void OnTriggerEnter2D(Collider2D c) {
        if (c.gameObject == Player.instance.gameObject) {
            foreach (Area area in areas) {
                area.trigger();
            }
        }
    }
}
