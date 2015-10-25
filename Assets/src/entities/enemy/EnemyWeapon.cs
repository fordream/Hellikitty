using UnityEngine;
using System.Collections;

public class EnemyWeapon : WeaponBase
{
    [HideInInspector] public Enemy parent;

    public void init(Enemy parent) {
        this.parent = parent;
    }

    public void update() {
        update_weapon_motion(parent.gameObject, Entities.player.transform.position);
        update_weapon_logic(parent.general_ai_state == GeneralAIState.SHOOTING);
	}
}
