using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "ScriptableObjects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [Header("Weapon Info")]
    public string weaponName = "Basic Shot";
    public int weaponDamage = 1;

    [Header("Projectile Appearance")]
    public GameObject projectilePrefab;

    [Header("Rhythm Pattern")]
    [Tooltip("Use simple pattern: Fire every X beats (1 = every beat, 2 = every other beat)")]
    public bool useSimplePattern = true;
    public int beatDivisor = 1;
    public int beatOffset = 0;

    [Header("Custom Beat Pattern")]
    [Tooltip("Advanced: Define exact beat pattern. True = shoot, False = skip. Loops when finished.")]
    public bool useCustomPattern = false;
    public bool[] customBeatPattern = new bool[] { true };

    [Header("Shooting Pattern")]
    public ShootingPattern pattern = ShootingPattern.Single;
    public int projectileCount = 1;
    public float spreadAngle = 15f;

    [Header("Effects")]
    public AudioClip shootSound;
    public float recoilForce = 5f;

    private int customPatternIndex = 0;

    public bool ShouldFireOnBeat(int currentBeat)
    {
        if (useCustomPattern && customBeatPattern.Length > 0)
        {
            bool shouldFire = customBeatPattern[customPatternIndex];
            customPatternIndex = (customPatternIndex + 1) % customBeatPattern.Length;
            return shouldFire;
        }
        else
        {
            return (currentBeat + beatOffset) % beatDivisor == 0;
        }
    }

    public void ResetPattern()
    {
        customPatternIndex = 0;
    }
}

public enum ShootingPattern
{
    Single,
    Spread,
    Burst
}
