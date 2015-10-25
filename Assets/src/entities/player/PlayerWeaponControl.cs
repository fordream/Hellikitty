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
        Vector2 mouse_pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inventory.equipped.update_motion(parent.gameObject, mouse_pos);
        inventory.equipped.update_logic(Input.GetMouseButtonUp(0));
	}
}
