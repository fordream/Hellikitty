using UnityEngine;
using System.Collections;

public class PlayerGun : MonoBehaviour {

    void Start()
    {

    }

    void Update()
    {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rota = transform.localEulerAngles;
        float angle = Mathf.Atan2(mouse_pos.y - Entities.player.transform.position.y, 
                                  mouse_pos.x - Entities.player.transform.position.x);
        angle *= Entities.player.transform.localScale.x / Entities.player.transform.localScale.x;
        rota.z = angle * (180.0f / Mathf.PI);
        transform.localEulerAngles = rota;

        Vector3 scale = transform.localScale;
        float scale_y = Mathf.Abs(scale.y);
        if (rota.z <= -90 || rota.z >= 90) scale_y = -scale_y;
        scale.y = scale_y;
        transform.localScale = scale;

        Vector3 pos = Entities.player.transform.position;
        pos.x += Mathf.Cos(angle) * 1;
        pos.y += Mathf.Sin(angle) * 1;
        transform.position = pos;

        if (Input.GetMouseButtonUp(0)) Bullet.spawn(transform.position).add_logic<RailgunBullet>().init(angle);
	}
}
