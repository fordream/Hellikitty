using UnityEngine;
using System.Collections;

public class PlayerGun : MonoBehaviour {

    Player player;
    Vector3 prev_pos;

    void Start()
    {
        player = Entities.player;
        prev_pos = Vector3.zero;
    }

    void Update()
    {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 rota = transform.localEulerAngles;
        float angle = Mathf.Atan2(mouse_pos.y - player.transform.position.y, 
                                  mouse_pos.x - player.transform.position.x);
        angle *= player.transform.localScale.x / player.transform.localScale.x;
        rota.z = angle * (180.0f / Mathf.PI);
        transform.localEulerAngles = rota;

        Vector3 scale = transform.localScale;
        float scale_y = Mathf.Abs(scale.y);
        if (rota.z <= -90 || rota.z >= 90) scale_y = -scale_y;
        scale.y = scale_y;
        transform.localScale = scale;

        Vector3 pos = player.transform.position;
        pos.x += Mathf.Cos(angle) * 1;
        pos.y += Mathf.Sin(angle) * 1;
        transform.position = pos;

        if (Input.GetMouseButtonUp(0)) Bullet.spawn(pos).add_logic<RailgunBullet>().init(angle);
	}
}
