using UnityEngine;

public class BlackFireAbillity : Ability
{
    [SerializeField] private float duration;
    [SerializeField] private float damage;
    [SerializeField] private float damageInterval;
    [SerializeField] private float positionDelta;

    public override void Use()
    {
        PlayerStats.Instance.BlackFire(duration, damage, damageInterval, positionDelta);
    }
}