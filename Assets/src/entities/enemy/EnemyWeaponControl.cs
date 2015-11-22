using UnityEngine;
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
        if (inventory.equipped == null) return;

        inventory.equipped.update(parent.gameObject, Entities.player.transform.position, 
                                  parent.general_ai_state == GeneralAIState.SHOOTING);
	}
}
