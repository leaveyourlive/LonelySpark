using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WingsOfDespairAbility : Ability
{
    [SerializeField] private float landingTime;
    [SerializeField] private float landingDamageRange;
    [SerializeField] private float landingDamage;
    [SerializeField] private GameObject circleHighlightingPrefab;
    [SerializeField] private float arcHeight;
    public override void Use()
    {
        FindObjectOfType<PlayerMovement>()
        .BigJump(
            landingTime, 
            landingDamageRange,
            landingDamage,
            circleHighlightingPrefab,
            arcHeight
        );
    }
}