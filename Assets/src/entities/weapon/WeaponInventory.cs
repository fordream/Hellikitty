using System;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    NONE, 
    PISTOL, 
    RAILGUN, 
    GRENADE_LAUNCHER, 
    SHOTGUN,
    SNIPER,
    LASER,
    ROCKET,
    XRAY
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

        foreach (GameObject weapon in starting_weapon_assets)
        {
            add_weapon(weapon);
        }
        if (weapons.Count > 0) equip_weapon(weapons[0]);
    }

    public void add_weapon(GameObject weapon)
    {
        GameObject weapon_obj = Instantiate(weapon);
        weapon_obj.transform.parent = parent.transform.parent.transform;
        Weapon weapon_comp = weapon_obj.GetComponent<Weapon>();
        weapon_comp.entity = parent;
        weapons.Add(weapon_comp);
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
