using UnityEngine;
using System.Collections;

public class AutoParticleDestroy : MonoBehaviour {

    ParticleSystem sys;

	void Start() {
        sys = GetComponent<ParticleSystem>();
	}

    void Update() {
        if (sys.isStopped) Destroy(gameObject);
	}
}
