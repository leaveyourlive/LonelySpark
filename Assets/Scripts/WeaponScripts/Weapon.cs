using System.Collections;
using UnityEngine;

public enum WeaponType {
    Melee,
}

public abstract class Weapon : Item
{
    public WeaponType weaponType;
    public float attackSpeed;
    public float staminaCost;

    public override void Activate()
    {
        Attack(PlayerStats.Instance);
    }

    public abstract void Attack(PlayerStats playerStats);
}