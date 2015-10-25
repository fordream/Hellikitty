using UnityEngine;
using System.Collections;

public class PlayerWeapon : WeaponBase
{
    Player player;

    public void init() {
        player = Entities.player;
    }

    void Update()
    {
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        update_weapon_motion(player.gameObject, mouse_pos);
        update_weapon_logic(Input.GetMouseButtonUp(0));
	}
}
