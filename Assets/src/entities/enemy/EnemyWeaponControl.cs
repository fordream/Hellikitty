﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WeaponInventory))]
public class EnemyWeaponControl : MonoBehaviour
{
    private Enemy parent;
    private WeaponInventory inventory;

    public void init(Enemy parent) {
        this.parent = parent;
        inventory = GetComponent<WeaponInventory>();
    }

    public void update() {
        inventory.equipped.update_motion(parent.gameObject, Entities.player.transform.position);
        inventory.equipped.update_logic(parent.general_ai_state == GeneralAIState.SHOOTING);
	}
}