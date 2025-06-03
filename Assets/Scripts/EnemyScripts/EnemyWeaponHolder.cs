using System.Collections;
using UnityEngine;

public class EnemyWeaponHolder : MonoBehaviour
{
    private bool isWeaponShown = true;

    public void AnimateMeleeAttack(Vector2 direction, float swingAngle, float swingTime)
    {
        if (transform.GetChild(0) != null && transform.GetChild(0).CompareTag("Melee"))
        {
            StartCoroutine(SwingSword(direction, swingAngle, swingTime));
        }
    }

    private IEnumerator SwingSword(Vector2 direction, float swingAngle, float swingTime)
    {
        Transform sword = transform.GetChild(0).GetChild(0);

        float elapsedTime = 0f;

        bool turnedLeft = sword.GetComponentInParent<EnemyWeaponHolder>().GetComponentInParent<EnemyHealth>().transform.localScale.x < 0;

    
        float startAngle = -45f + swingAngle/2;
        float endAngle = -45f - swingAngle/2;
      
        if (turnedLeft) {
            startAngle = 90f - swingAngle/2;
            endAngle = 90f + swingAngle/2;
        }

        Quaternion startRotation = Quaternion.Euler(0, 0, startAngle);
        Quaternion endRotation = Quaternion.Euler(0, 0, endAngle);

        while (elapsedTime < swingTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / swingTime;

            sword.rotation = Quaternion.Lerp(startRotation, endRotation, t);
            yield return null;
        }

        sword.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void ToggleWeaponVisibility()
    {
        Transform weapon = transform.GetChild(0);
        isWeaponShown = !isWeaponShown;
        weapon.gameObject.SetActive(isWeaponShown);
    }

}