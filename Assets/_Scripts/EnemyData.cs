using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Combat Stats")]
    public int maxLives = 3;
    public int projectileDamage = 1;

    [Header("Shooting")]
    public bool isShooter = false;
    public GameObject projectileVisualPrefab;
    public float projectileSpeed = 8f;
    public float projectileLifetime = 10f;
    public float shootInterval = 2f;

    [Header("Movement")]
    public float speed = 2f;
    public EnemyMovementType movementType = EnemyMovementType.ChasePlayer;

    [Header("Audio")]
    public string hitSound;
    public string deathSound;
    public string shootSound;

    [Header("Visuals")]
    public RuntimeAnimatorController animatorController;
    public GameObject[] hitEffectPrefab;
    public GameObject projectileHitEffectPrefab;
}

public enum EnemyMovementType
{
    ChasePlayer,
    MoveTowardsPlayer,
    Stationary,
    StraightDown
}
