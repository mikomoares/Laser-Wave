using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerController : SteerableBehaviour, IShooter, IDamageable
{
    public AudioClip shootSFX;
    public Text tutorial;
    public AudioClip hit;
    public Rigidbody2D rb;
    public Camera cam;
    public GameObject bullet;
    private int lifes;
    public float shootDelay = 0.2f;

    private float lastShootTimestamp = 0.0f;
    Animator animator;
    Vector2 mousePos;
    Vector2 lookDir;
    Vector2 toPos;
    public Image[] hearts;
    public Sprite heart;
    private bool comecou;

    public Transform weapon;
    public GameObject gm;
    private void Start()
    {
        lifes = 10;
        animator = GetComponent<Animator>();
        comecou = false;
        gm = GameObject.Find("GameManager");
    }


    public void Shoot()
    {
        toPos.x = lookDir.x;
        toPos.y = lookDir.y;
        if (Time.time - lastShootTimestamp < 0.2f) return;
        animator.SetTrigger("Attack");
        AudioManager.PlaySFX(shootSFX);
        lastShootTimestamp = Time.time;
        GameObject bul = Instantiate(bullet, weapon.position, transform.rotation);
        Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
        rb.AddForce(weapon.up*20, ForceMode2D.Impulse);
        //Thrust(lookDir.normalized.x*2, lookDir.normalized.y*2);
        this.GetComponent<Rigidbody2D>().AddForce(toPos.normalized*5f, ForceMode2D.Impulse);
        if(!comecou){
            comecou=true;
            tutorial.enabled = false;

        }
    }

    public void TakeDamage()
    {
        lifes--;
        if (lifes <= 0) Die();
        animator.SetTrigger("Damage");
        
    }

    public void Die()
    {
        gm.GetComponent<GM>().Lose();
    }
    private void Update() {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i<lifes){
                hearts[i].enabled = true;
            }else{
                hearts[i].enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        if(Input.GetMouseButton(0))
        {
            Shoot();
        }
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
            if (!(damageable is null))
            {
                damageable.Die();
            }
            TakeDamage();
            TakeDamage();
            TakeDamage();

        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        AudioManager.PlaySFX(hit);
        TakeDamage();
    }
 
}
