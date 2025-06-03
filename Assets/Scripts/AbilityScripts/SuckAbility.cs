using UnityEngine;

public class SuckAbility : Ability
{
    [SerializeField] private float duration;
    [SerializeField] private float HPCost;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float damageInterval;
    public override void Use()
    {
        PlayerStats.Instance.Suck(duration, HPCost, damage, range, damageInterval);
    }
}