using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [Header("Combat Stats")]
    public int maxLives = 3;
    public bool isShooter = false;
    public GameObject projectilePrefab;
    public float shootInterval = 2f;

    [Header("Movement")]
    public float speed = 2f;
    public EnemyMovementType movementType = EnemyMovementType.ChasePlayer;

    [Header("Audio")]
    public AudioClip hitSound;
    public AudioClip deathSound;
    public AudioClip shootSound;

    [Header("Visuals")]
    public RuntimeAnimatorController animatorController;
}

public enum EnemyMovementType
{
    ChasePlayer,
    MoveTowardsPlayer,
    Stationary,
    StraightDown
}
