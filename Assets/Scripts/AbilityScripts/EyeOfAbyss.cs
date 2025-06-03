using UnityEngine;

public class eyeOfAbyssAbility : Ability
{
    [SerializeField] private float duration;
    [SerializeField] private float range;
    [SerializeField] private float decelerationFactor;
    [SerializeField] private float accelerationCoefficient;
    public override void Use()
    {
        PlayerStats.Instance.eyeOfAbyss(duration, range, decelerationFactor, accelerationCoefficient);
    }
}