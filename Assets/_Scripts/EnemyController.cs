using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : SteerableBehaviour, IShooter, IDamageable
{
    public GameObject tiro;
    public GameObject player;
    private Animator animator;
    public AudioClip pop;

    private int life = 3;

    public void Shoot()
    {
        Instantiate(tiro, transform.position, Quaternion.identity);
    }

    public void Die()
    {
        Destroy(gameObject);
        AudioManager.PlaySFX(pop);

    }

    float angle = 0;
    private void Start() {
        player = GameObject.Find("Player");
        animator = GetComponent<Animator>();
    }
    private void FixedUpdate()
    {
        float step =  1f * Time.deltaTime; 
        if (player != null){
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
        //this.gameObject.GetComponent<Rigidbody2D>().MovePosition(player.transform.position);
        // angle += 0.1f;
        // Mathf.Clamp(angle, 0.0f, 2.0f * Mathf.PI);
        // float x = Mathf.Sin(angle);
        // float y = Mathf.Cos(angle);

        // Thrust(x, y);
       
    }
    public void TakeDamage()
    {
        if (life == 1){
            Die();
        }else{
            life--;
            animator.SetTrigger("Damage");
        }
    }
}
