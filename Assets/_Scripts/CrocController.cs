using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocController : SteerableBehaviour, IDamageable, IShooter
{
    private Animator animator;
    public int life;
    public GameObject gm;
    public GameObject weapon;
    public GameObject enemyShot;
    public float _lastShootTimestamp = 0f;
    void Start()
    {
        life = 50;
        animator = GetComponent<Animator>();
        gm = GameObject.Find("GameManager");
    }

    void Update()
    {
        Shoot();
    }

    public void TakeDamage(){
        if (life == 1){
            Die();
        }else{
            life--;
            animator.SetTrigger("damage");
        }
    }
    public void Die(){
        gm.GetComponent<GM>().Win();
    }
    public void Shoot(){
        if (Time.time - _lastShootTimestamp < 2f) return;
       _lastShootTimestamp = Time.time;
       Instantiate(enemyShot, weapon.transform.position, Quaternion.identity);
    }
}
