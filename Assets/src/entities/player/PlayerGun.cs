﻿using UnityEngine;
using System.Collections;

public class PlayerGun : GunBase
{

    Player player;

    public void init() {
        player = Entities.player;
    }

    void Update()
    {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        update_gun_motion(player.gameObject, mouse_pos);

        if (Input.GetMouseButtonUp(0)) Bullet.spawn<RailgunBullet>(pos).init(angle);
	}
}
