using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CampfireScript : MonoBehaviour, IInteractable
{
    [Header("Campfire Settings")]
    public InteractPriority interactionPriority = InteractPriority.Campfire;
    public float restCooldown = 2f;
    
    [Header("References")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private TeleportationMenu teleportationMenu;

    private bool isResting = false;
    private bool canEndResting = false;
    
    public InteractPriority Priority => interactionPriority;
    public Vector3 Position => transform.position;

    private void Awake()
    {
        if (teleportationMenu == null)
            teleportationMenu = GetComponentInChildren<TeleportationMenu>();
        
        FindPlayerReferences();
    }

    private void FindPlayerReferences()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerStats = player.GetComponent<PlayerStats>();
            movement = player.GetComponent<PlayerMovement>();
        }
    }

    public void Interact()
    {
        if (!isResting)
        {
            StartResting();
        }
        else if (canEndResting)
        {
            StopResting();
        }
    }

    private void StartResting()
    {
        isResting = true;
        canEndResting = false;
        
        movement.rb.velocity = Vector2.zero;
        RefreshPlayerStats();
        DeactivateEnemies();
        RegisterCampfire();
        OpenMenu();

        StartCoroutine(RestingCooldown());
    }

    private IEnumerator RestingCooldown()
    {
        yield return new WaitForSeconds(restCooldown);
        canEndResting = true;
    }

    private void RefreshPlayerStats()
    {
        if (playerStats != null)
        {
            playerStats.currentHealth = playerStats.maxHealth;
            playerStats.currentStamina = playerStats.maxStamina;
            playerStats.currentMana = playerStats.maxMana;
        }
    }

    private void DeactivateEnemies()
    {
        foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            var enemySpawner = spawner.GetComponent<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.DeactivateAllEnemies();
            }
        }
    }

    private void RegisterCampfire()
    {
        CampfireManager.Instance.AddCampfire(
            gameObject.name,
            SceneManager.GetActiveScene().name,
            transform.position
        );
    }

    private void StopResting()
    {
        isResting = false;
        canEndResting = false;
        ReactivateEnemies();
        CloseMenu();
    }

    private void ReactivateEnemies()
    {
        foreach (var spawner in GameObject.FindGameObjectsWithTag("Spawner"))
        {
            var enemySpawner = spawner.GetComponent<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.SpawnEnemy();
            }
        }
    }

    private void OpenMenu()
    {
        if (teleportationMenu != null)
        {
            teleportationMenu.OpenMenu();
            movement.enabled = false;
        }
    }

    public void CloseMenu()
    {
        if (teleportationMenu != null)
        {
            teleportationMenu.CloseMenu();
            movement.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var interactionSystem = other.GetComponent<PlayerInteraction>();
            if (interactionSystem != null)
            {
                interactionSystem.RegisterInteractable(this);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var interactionSystem = other.GetComponent<PlayerInteraction>();
            if (interactionSystem != null)
            {
                interactionSystem.UnregisterInteractable(this);
            }
            
            if (isResting)
            {
                StopResting();
            }
        }
    }

    // Методы для кнопок UI
    public void OnLevelUpButtonPressed()
    {
        // Логика улучшения уровня
    }

    public void OnTeleportButtonPressed()
    {
        // Логика телепортации
    }
}