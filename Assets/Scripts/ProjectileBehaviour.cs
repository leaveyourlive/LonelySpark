using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float projectileDamage;
    [SerializeField] private bool isOwnedByPlayer;
    [SerializeField] private DamageType projectileDamageType;

    public void AddDamage(float damage)
    {
        projectileDamage += damage;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOwnedByPlayer)
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<EnemyHealth>().TakeDamage(projectileDamage, projectileDamageType);
                Destroy(gameObject);
            }
        } 
        else
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerStats>().TakeDamage(projectileDamage, projectileDamageType);
                Destroy(gameObject);
            }
        }

        // Проверяем попадание в стену по тегу
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        // Проверяем попадание в стену по слою
        if (collision.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Destroy(gameObject);
        }
    }
}