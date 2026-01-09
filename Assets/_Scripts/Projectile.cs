using UnityEngine;

public enum ProjectileOwner
{
    Player,
    Enemy
}

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [Header("Ownership")]
    public ProjectileOwner owner = ProjectileOwner.Player;

    [Header("Movement")]
    public float speed = 10f;
    public bool usePhysics = false;

    [Header("Collision")]
    public AudioClip hitSound;
    public GameObject hitEffectPrefab;

    [Header("Lifetime")]
    public float maxLifetime = 10f;

    [HideInInspector]
    public Vector2 direction = Vector2.up;

    private Rigidbody2D rb;
    private float lifetime = 0f;
    private bool directionSet = false;
    public int weaponDamage = 1;

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
    }

    private void Update()
    {
        if (!usePhysics)
        {
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }

        lifetime += Time.deltaTime;
        if (lifetime >= maxLifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ShouldIgnoreCollision(collision))
        {
            return;
        }

        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(weaponDamage);

            if (hitSound != null)
            {
                AudioManager.PlaySFX(hitSound);
            }

            if (hitEffectPrefab != null)
            {
                GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                Destroy(effect, 2f);
            }
        }

        Destroy(gameObject);
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
        Destroy(gameObject);
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

    public void ApplyForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse)
    {
        rb.AddForce(force, mode);
        usePhysics = true;
    }
}
