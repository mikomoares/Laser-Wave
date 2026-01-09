using UnityEngine;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("Active Weapons")]
    [Tooltip("All weapons in this list will fire simultaneously, each following its own rhythm pattern")]
    public WeaponData[] activeWeapons;

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

        foreach (var weapon in activeWeapons)
        {
            if (weapon != null)
            {
                weapon.ResetPattern();
            }
        }
    }

    private void OnBeat()
    {
        print("Beat received in PlayerWeaponSystem");
        bool anyWeaponFired = false;

        foreach (var weapon in activeWeapons)
        {
            if (weapon != null && weapon.ShouldFireOnBeat(BeatManager.Instance.GetCurrentBeat()))
            {
                Shoot(weapon);
                anyWeaponFired = true;
            }
        }

        if (anyWeaponFired)
        {
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
        if (weapon == null || weapon.projectilePrefab == null)
        {
            return;
        }

        if (weapon.shootSound != null)
        {
            AudioManager.PlaySFX(weapon.shootSound);
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
        }

        if (playerRb != null && weapon.recoilForce > 0)
        {
            Vector2 recoilDirection = weaponTransform.right;
            playerRb.AddForce(recoilDirection * weapon.recoilForce, ForceMode2D.Impulse);
        }
    }

    private void SpawnProjectile(WeaponData weapon, Vector2 direction)
    {
        GameObject proj = Instantiate(
            weapon.projectilePrefab,
            weaponTransform.position,
            weaponTransform.rotation
        );

        Projectile projectile = proj.GetComponent<Projectile>();
        if (projectile != null)
        {
            projectile.SetOwner(ProjectileOwner.Player);
            projectile.SetDirection(direction);
            projectile.weaponDamage = weapon.weaponDamage;
        }
    }

    private void ShootSpread(WeaponData weapon)
    {
        float angleStep = weapon.spreadAngle / (weapon.projectileCount - 1);
        float startAngle = -weapon.spreadAngle / 2f;

        for (int i = 0; i < weapon.projectileCount; i++)
        {
            float currentAngle = startAngle + (angleStep * i);
            Vector2 direction = Quaternion.Euler(0, 0, currentAngle) * weaponTransform.right;
            SpawnProjectile(weapon, direction);
        }
    }

    private void ShootBurst(WeaponData weapon)
    {
        for (int i = 0; i < weapon.projectileCount; i++)
        {
            SpawnProjectile(weapon, weaponTransform.right);
        }
    }

    public void AddWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        WeaponData[] newArray = new WeaponData[activeWeapons.Length + 1];
        activeWeapons.CopyTo(newArray, 0);
        newArray[activeWeapons.Length] = weapon;
        activeWeapons = newArray;
        weapon.ResetPattern();
    }

    public void RemoveWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        int index = System.Array.IndexOf(activeWeapons, weapon);
        if (index >= 0)
        {
            WeaponData[] newArray = new WeaponData[activeWeapons.Length - 1];
            int newIndex = 0;
            for (int i = 0; i < activeWeapons.Length; i++)
            {
                if (i != index)
                {
                    newArray[newIndex] = activeWeapons[i];
                    newIndex++;
                }
            }
            activeWeapons = newArray;
        }
    }

    public void ClearAllWeapons()
    {
        activeWeapons = new WeaponData[0];
    }

    private void OnDestroy()
    {
        if (BeatManager.Instance != null)
        {
            BeatManager.Instance.onBeat.RemoveListener(OnBeat);
        }
    }
}
