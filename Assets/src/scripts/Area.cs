using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Area : MonoBehaviour {

	void Start() {
        foreach (Transform child in transform) {
            GameObject obj = child.gameObject;
            obj.SetActive(false);
        }
	}

	void Update() {

	}

    public void trigger() {
        foreach (Transform child in transform) {
            GameObject obj = child.gameObject;
            obj.SetActive(true);
        }
    }
}
