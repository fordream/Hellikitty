using UnityEngine;
using System.Collections;

public class TextFade : MonoBehaviour {

    private TextMesh text_mesh;
    private float fade_timer = 0;
    private const float SPEED = .35f;
    private float accel_y = 0;

	private void Start() {
        text_mesh = GetComponent<TextMesh>();
	}

	private void Update() {
        fade_timer += Time.deltaTime;
        if (fade_timer >= .5f)
        {
            accel_y += .2f * Time.deltaTime;
            transform.Translate(0, accel_y + (SPEED * Time.deltaTime), 0);

            Color colour = text_mesh.color;
            colour.a -= 1.8f * Time.deltaTime;
            text_mesh.color = colour;

            if (colour.a <= 0) Destroy(gameObject);
        }else transform.Translate(0, SPEED * Time.deltaTime, 0);
	}
}
