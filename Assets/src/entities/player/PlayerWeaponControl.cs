using UnityEngine;
using System.Collections;

[RequireComponent(typeof(WeaponInventory))]
public class PlayerWeaponControl : MonoBehaviour
{
    private Player parent;
    private WeaponInventory inventory;

    public void init() {
        parent = Entities.player;
        inventory = GetComponent<WeaponInventory>();
    }

    public void update()
    {
        if (inventory.equipped == null) return;

        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inventory.equipped.update(parent.gameObject, mouse_pos, Input.GetMouseButton(0));
	}
}
