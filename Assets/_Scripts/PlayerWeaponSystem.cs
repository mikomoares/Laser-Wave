using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("8-Beat Loop System")]
    [Tooltip("Define which weapon fires on each of the 8 beats. Null = no fire on that beat.")]
    public WeaponData[] beatLoop = new WeaponData[8];

    [Header("References")]
    private Transform weaponTransform;
    private Rigidbody2D playerRb;
    private Animator animator;

    private PlayerController playerController;
    private bool hasShot = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        playerRb = GetComponent<Rigidbody2D>();
        weaponTransform = transform;
        
        if (BeatManager.Instance != null)
        {
            BeatManager.Instance.onBeat.AddListener(OnBeat);
        }

        playerController = GetComponent<PlayerController>();

        if (beatLoop == null || beatLoop.Length != 8)
        {
            beatLoop = new WeaponData[8];
        }
    }

    private void OnBeat()
    {
        if (GameStateManager.Instance != null && !GameStateManager.Instance.IsGameStarted())
        {
            return;
        }

        int currentBeat = BeatManager.Instance.GetCurrentBeat();
        int beatIndex = currentBeat % 8;

        WeaponData weaponToFire = beatLoop[beatIndex];

        if (weaponToFire != null)
        {
            Shoot(weaponToFire);

            if (animator != null)
            {
                animator.SetTrigger("Attack");
            }

            if (!hasShot && playerController != null)
            {
                hasShot = true;
                playerController.OnFirstShot();
            }
        }
    }

    private void Shoot(WeaponData weapon)
    {
        if (weapon == null || weapon.projectileVisualPrefab == null)
        {
            return;
        }

        if (weapon.shootSounds != null)
        {
        }

        switch (weapon.pattern)
        {
            case ShootingPattern.Single:
                SpawnProjectile(weapon, -weaponTransform.right);
                break;

            case ShootingPattern.Spread:
                ShootSpread(weapon);
                break;

            case ShootingPattern.Burst:
                ShootBurst(weapon);
                break;

            case ShootingPattern.AreaOfEffect:
                SpawnAOEProjectile(weapon);
                break;
        }

        if (playerRb != null && weapon.recoilForce > 0)
        {
            Vector2 recoilDirection = weaponTransform.right;
            playerRb.AddForce(recoilDirection * weapon.recoilForce, ForceMode2D.Impulse);
        }
    }

    private void SpawnProjectile(WeaponData weapon, Vector2 direction)
    {
        if (weapon.projectileVisualPrefab == null)
        {
            Debug.LogWarning($"Weapon {weapon.weaponName} has no projectileVisualPrefab assigned!");
            return;
        }

        GameObject proj = Instantiate(
            weapon.projectileVisualPrefab,
            weaponTransform.position,
            weaponTransform.rotation
        );

        float volume = CreateVolume();
        float pitch = CreatePitch();
        if (BeatManager.Instance.GetCurrentLoop() % 4 == 0) AudioManager.PlaySFX(weapon.shootSounds[0], pitch, volume);
        else AudioManager.PlaySFX(weapon.shootSounds[1], pitch, volume);

        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile == null)
        {
            projectile = proj.AddComponent<Projectile>();
        }

        projectile.SetOwner(ProjectileOwner.Player);
        projectile.SetProjectileType(weapon.projectileType);
        projectile.SetDirection(direction);
        projectile.weaponDamage = weapon.weaponDamage;
        projectile.speed = weapon.projectileSpeed;
        projectile.maxLifetime = weapon.projectileLifetime;
        projectile.hitEffectPrefab = weapon.hitEffectPrefab;
    }

    private void ShootSpread(WeaponData weapon)
    {
        float angleStep = weapon.spreadAngle / (weapon.projectileCount - 1);
        float startAngle = -weapon.spreadAngle / 2f;

        for (int i = 0; i < weapon.projectileCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * weaponTransform.right;
            SpawnProjectile(weapon, -direction);
            float volume = CreateVolume();
            float pitch = CreatePitch();
            if (BeatManager.Instance.GetCurrentLoop() % 4 == 0) AudioManager.PlaySFX(weapon.shootSounds[0], pitch, volume);
            else AudioManager.PlaySFX(weapon.shootSounds[1], pitch, volume);
        }
    }

    private void ShootBurst(WeaponData weapon)
    {
        StartCoroutine(BurstCoroutine(weapon));
    }

    private IEnumerator BurstCoroutine(WeaponData weapon)
    {
        if (weapon.projectileCount <= 0)
        {
            yield break;
        }

        float beatInterval = BeatManager.Instance.GetBeatInterval();
        float timeBetweenShots = beatInterval / weapon.projectileCount;

        for (int i = 0; i < weapon.projectileCount; i++)
        {
            SpawnProjectile(weapon, -weaponTransform.right);
            float volume = CreateVolume();
            float pitch = CreatePitch();
            if (BeatManager.Instance.GetCurrentLoop() % 4 == 0) AudioManager.PlaySFX(weapon.shootSounds[0], pitch, volume);
            else AudioManager.PlaySFX(weapon.shootSounds[1], pitch, volume);

            if (i < weapon.projectileCount - 1)
            {
                yield return new WaitForSeconds(timeBetweenShots);
            }
        }
    }

    private void SpawnAOEProjectile(WeaponData weapon)
    {
        if (weapon.projectileVisualPrefab == null)
        {
            Debug.LogWarning($"AOE Weapon {weapon.weaponName} has no projectileVisualPrefab assigned!");
            return;
        }

        GameObject proj = Instantiate(
            weapon.projectileVisualPrefab,
            transform.position,
            Quaternion.identity
        );
        float volume = CreateVolume();
        float pitch = CreatePitch();
        if (BeatManager.Instance.GetCurrentLoop() % 4 == 0) AudioManager.PlaySFX(weapon.shootSounds[0], pitch, volume);
        else AudioManager.PlaySFX(weapon.shootSounds[1], pitch, volume);

        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile == null)
        {
            projectile = proj.AddComponent<Projectile>();
        }

        projectile.SetOwner(ProjectileOwner.Player);
        projectile.SetProjectileType(ProjectileType.AreaOfEffect);
        projectile.SetFollowTarget(transform);
        projectile.weaponDamage = weapon.weaponDamage;
        projectile.speed = 0f;
        projectile.maxLifetime = weapon.projectileLifetime;
        projectile.hitEffectPrefab = weapon.hitEffectPrefab;
    }

    public void SetBeatSlot(int beatIndex, WeaponData weapon)
    {
        if (beatIndex >= 0 && beatIndex < 8)
        {
            beatLoop[beatIndex] = weapon;
        }
    }

    public void ClearBeatSlot(int beatIndex)
    {
        if (beatIndex >= 0 && beatIndex < 8)
        {
            beatLoop[beatIndex] = null;
        }
    }

    public void ClearAllBeats()
    {
        for (int i = 0; i < 8; i++)
        {
            beatLoop[i] = null;
        }
    }

    public WeaponData GetBeatSlot(int beatIndex)
    {
        if (beatIndex >= 0 && beatIndex < 8)
        {
            return beatLoop[beatIndex];
        }
        return null;
    }

    private void OnDestroy()
    {
        if (BeatManager.Instance != null)
        {
            BeatManager.Instance.onBeat.RemoveListener(OnBeat);
        }
    }

    private float CreatePitch()
    {
        return .9f + BeatManager.Instance.GetCurrentLoop()%4 * .05f * Random.Range(0.8f, 1.2f);
    }
    private float CreateVolume()
    {
        return .9f - BeatManager.Instance.GetCurrentLoop()%4 * .05f * Random.Range(0.8f, 1.2f);
    }
}
