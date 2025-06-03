using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public enum DamageType
{
    Physical,
    Magical,
    Pure
}

public interface IDamageable
{
    void TakeDamage(float amount, DamageType damageType);
}