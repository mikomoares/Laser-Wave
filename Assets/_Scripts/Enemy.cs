using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Configuration")]
    public EnemyData enemyData;

    private int currentLives;
    private Rigidbody2D rb;
    private Animator animator;
    private Transform playerTransform;
    private float shootTimer;
    public bool isBeatAnimationSlow;

    private void Awake()
    {
        if (enemyData == null)
        {
            Debug.LogError($"EnemyData not assigned on {gameObject.name}");
            enabled = false;
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (BeatManager.Instance != null)
        {
            BeatManager.Instance.onBeat.AddListener(OnBeat);
        }
        currentLives = enemyData.maxLives;
        
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        if (animator != null && enemyData.animatorController != null)
        {
            animator.runtimeAnimatorController = enemyData.animatorController;
        }

        shootTimer = enemyData.shootInterval;
    }

    private void OnBeat()
    {
        if(isBeatAnimationSlow && BeatManager.Instance.GetCurrentBeat() % 4 == 0)
        {
            animator.SetTrigger("Beat");
        }
        else if (!(BeatManager.Instance.GetCurrentBeat() % 2 == 0))
        {
            animator.SetTrigger("Beat");
        }
    }

    private void Update()
    {
        if (enemyData.isShooter && playerTransform != null)
        {
            shootTimer -= Time.deltaTime;
            if (shootTimer <= 0f)
            {
                Shoot();
                shootTimer = enemyData.shootInterval;
            }
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (playerTransform == null) return;

        float step = enemyData.speed * Time.fixedDeltaTime;

        switch (enemyData.movementType)
        {
            case EnemyMovementType.ChasePlayer:
            {
                Vector2 direction = (playerTransform.position - transform.position).normalized;
                rb.MovePosition(rb.position + direction * step);
                break;
            }

            case EnemyMovementType.MoveTowardsPlayer:
            {
                Vector3 newPos = Vector3.MoveTowards(
                    transform.position,
                    playerTransform.position,
                    step
                );
                transform.position = newPos;
                break;
            }

            case EnemyMovementType.StraightDown:
            {
                rb.MovePosition(rb.position + Vector2.down * step);
                break;
            }

            case EnemyMovementType.Stationary:
                // Não faz nada
                break;
        }
    }
    private void Shoot()
    {
        if (enemyData.projectilePrefab != null)
        {
            GameObject proj = Instantiate(enemyData.projectilePrefab, transform.position, Quaternion.identity);
            
            Projectile projectile = proj.GetComponent<Projectile>();
            if (projectile != null)
            {
                projectile.SetOwner(ProjectileOwner.Enemy);
                
                if (playerTransform != null)
                {
                    Vector2 directionToPlayer = (playerTransform.position - transform.position).normalized;
                    projectile.SetDirection(directionToPlayer);
                }
            }
            
            // if (enemyData.shootSound != null)
            // {
            //     AudioManager.PlaySFX(enemyData.shootSound);
            // }
        }
    }

    public void TakeDamage(int amount)
    {
        currentLives -= amount;

        if (currentLives <= 0)
        {
            Die();
        }
        else
        {
            if (animator != null)
            {
                animator.SetTrigger("Damage");
            }

            if (enemyData.hitSound != null)
            {
                AudioManager.PlaySFX(enemyData.hitSound);
            }

            if (enemyData.hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(enemyData.hitEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }
        }
    }

    public void Die()
    {
        if (enemyData.deathSound != null)
        {
            AudioManager.PlaySFX(enemyData.deathSound);
        }

        Destroy(gameObject);
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
