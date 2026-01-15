using UnityEngine;
using System.Collections.Generic;
using CartoonFX;

public enum ProjectileOwner
{
    Player,
    Enemy
}

public enum ProjectileType
{
    Normal,
    AreaOfEffect
}

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [HideInInspector] public ProjectileOwner owner = ProjectileOwner.Player;
    [HideInInspector] public ProjectileType projectileType = ProjectileType.Normal;
    [HideInInspector] public int weaponDamage = 1;
    [HideInInspector] public float speed = 10f;
    [HideInInspector] public float maxLifetime = 10f;
    [HideInInspector] public GameObject hitEffectPrefab;
    [HideInInspector] public Vector2 direction = Vector2.up;
    [HideInInspector] public Transform followTarget;

    public bool usePhysics = false;

    private Rigidbody2D rb;
    private float lifetime = 0f;
    private bool directionSet = false;
    private Dictionary<GameObject, float> enemyLastDamageTime = new Dictionary<GameObject, float>();
    private float damageInterval = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (owner == ProjectileOwner.Enemy && !directionSet)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                direction = (player.transform.position - transform.position).normalized;
            }
        }

        if (projectileType == ProjectileType.AreaOfEffect)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.simulated = true;
            
            Collider2D[] colliders = GetComponents<Collider2D>();
            foreach (Collider2D col in colliders)
            {
                col.isTrigger = true;
            }

            if (owner == ProjectileOwner.Player)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    Collider2D playerCollider = player.GetComponent<Collider2D>();
                    if (playerCollider != null)
                    {
                        foreach (Collider2D col in colliders)
                        {
                            Physics2D.IgnoreCollision(col, playerCollider, true);
                        }
                    }
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (projectileType == ProjectileType.Normal && !usePhysics)
        {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
        else if (projectileType == ProjectileType.AreaOfEffect && followTarget != null)
        {
            transform.position = followTarget.position;
        }

        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (projectileType == ProjectileType.Normal)
        {
            HandleNormalCollision(collision);
        }
        else if (projectileType == ProjectileType.AreaOfEffect)
        {
            HandleAOECollision(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (projectileType == ProjectileType.AreaOfEffect)
        {
            HandleAOECollision(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (projectileType == ProjectileType.AreaOfEffect)
        {
            enemyLastDamageTime.Remove(collision.gameObject);
        }
    }

    private void HandleNormalCollision(Collider2D collision)
    {
        if (ShouldIgnoreCollision(collision))
        {
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(weaponDamage);

            AudioManager.PlaySFX("hit");

            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                CFXR_ParticleText particleText = effect.GetComponent<CFXR_ParticleText>();
                if (particleText != null)
                {
                    particleText.UpdateText((weaponDamage * 10).ToString());
                }
                Destroy(effect, 2f);
            }
        }

        Destroy(gameObject);
    }

    private void HandleAOECollision(Collider2D collision)
    {
        if (ShouldIgnoreCollision(collision))
        {
            return;
        }

        GameObject enemy = collision.gameObject;
        
        if (enemyLastDamageTime.TryGetValue(enemy, out float lastDamageTime))
        {
            if (Time.time < lastDamageTime + damageInterval)
            {
                return;
            }
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(weaponDamage);
            enemyLastDamageTime[enemy] = Time.time;

            AudioManager.PlaySFX("hit");

            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, collision.transform.position, Quaternion.identity);
                CFXR_ParticleText particleText = effect.GetComponent<CFXR_ParticleText>();
                if (particleText != null)
                {
                    particleText.UpdateText((weaponDamage * 10).ToString());
                }
                Destroy(effect, 2f);
            }
        }
    }

    private bool ShouldIgnoreCollision(Collider2D collision)
    {
        if (collision.CompareTag("Projectile"))
        {
            return true;
        }
        if (owner == ProjectileOwner.Player)
        {
            if (collision.CompareTag("Player"))
            {
                return true;
            }
        }
        else if (owner == ProjectileOwner.Enemy)
        {
            if (collision.CompareTag("Enemy"))
            {
                return true;
            }
        }

        return false;
    }

    private void OnBecameInvisible()
    {
        if (projectileType == ProjectileType.Normal)
        {
            Destroy(gameObject);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
        directionSet = true;
    }

    public void SetDirectionFromRotation()
    {
        direction = transform.up;
        directionSet = true;
    }

    public void SetOwner(ProjectileOwner projectileOwner)
    {
        owner = projectileOwner;
    }

    public void SetProjectileType(ProjectileType type)
    {
        projectileType = type;
    }

    public void SetFollowTarget(Transform target)
    {
        followTarget = target;
    }

    public void ApplyForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
    {
        rb.AddForce(force, mode);
        usePhysics = true;
    }
}
