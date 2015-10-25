using System;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    NONE, 
    PISTOL, 
    RAILGUN, 
    GRENADE_LAUNCHER
};

public class WeaponInventory : MonoBehaviour
{
    private Entity parent;

    [HideInInspector] public Weapon equipped;
    [HideInInspector] public List<Weapon> weapons = new List<Weapon>();
    public GameObject[] starting_weapon_assets;

    public void init(Entity parent)
    {
        this.parent = parent;

        if (starting_weapon_assets.Length == 0)
        {
            Debug.LogError("Starting weapon types must contain at least one weapon (set in editor)");
        }

        foreach (GameObject weapon in starting_weapon_assets)
        {
            add_weapon(weapon);
        }
        equip_weapon(weapons[0]);
    }

    public void add_weapon(GameObject weapon)
    {
        GameObject weapon_obj = Instantiate(weapon);
        weapon_obj.transform.parent = parent.transform;
        weapons.Add(weapon_obj.GetComponent<Weapon>());
        weapon_obj.SetActive(false);
    }

    public void equip_weapon(Weapon weapon)
    {
        if (equipped != null)
        {
            equipped.gameObject.SetActive(false);
        }
        weapon.gameObject.SetActive(true);
        equipped = weapon;
    }
}
