using UnityEngine;
using System.Collections;

public class BackgroundScroll : MonoBehaviour
{
    private Vector3 start_pos;

	void Start() {
        start_pos = Camera.main.transform.position;
	}

	void Update() {
        Vector3 pos = start_pos + (Camera.main.transform.position - start_pos) / 2.0f;
        pos.z = 20;
        transform.position = pos;
	}
}
