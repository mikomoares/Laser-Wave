using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName = "Basic Shot";
    public int weaponDamage = 1;

    [Header("Projectile Visual")]
    [Tooltip("The sprite or visual prefab for this weapon's projectiles")]
    public GameObject projectileVisualPrefab;

    [Header("Projectile Behavior")]
    public float projectileSpeed = 10f;
    public float projectileLifetime = 5f;

    [Header("Shooting Pattern")]
    public ShootingPattern pattern = ShootingPattern.Single;
    public int projectileCount = 1;
    public float spreadAngle = 15f;

    [Header("Effects")]
    public AudioClip shootSound;
    public AudioClip hitSound;
    public GameObject hitEffectPrefab;
    public float recoilForce = 5f;
}

public enum ShootingPattern
{
    Single,
    Spread,
    Burst
}
