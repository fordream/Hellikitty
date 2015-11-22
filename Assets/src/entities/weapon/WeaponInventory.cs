using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    private Entity parent;

    [HideInInspector] public Weapon equipped;
    [HideInInspector] public List<Weapon> weapons = new List<Weapon>();

    public void init(Entity parent)
    {
        this.parent = parent;

        Weapon[] weapon_comps = GetComponents<Weapon>();

        foreach (Weapon weapon in weapon_comps)
        {
            weapon.entity = parent;
            weapons.Add(weapon);
            if (weapon.initially_equipped) equip_weapon(weapon);
        }
    }

    public void dequip_weapon()
    {
        if (equipped != null)
        {
            Destroy(equipped.weapon_asset_instance);
        }
    }

    public void equip_weapon(Weapon weapon)
    {
        dequip_weapon();

        if (weapon.weapon_asset == null) Debug.LogError("weapon asset was not assigned");

        GameObject weapon_obj = Instantiate(weapon.weapon_asset);
        weapon_obj.transform.parent = parent.transform.parent.transform;
        weapon.weapon_asset_instance = weapon_obj;

        equipped = weapon;
    }
}
