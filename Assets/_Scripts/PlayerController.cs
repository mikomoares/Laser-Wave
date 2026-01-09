using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamageable
{
    [Header("Health & UI")]
    public int maxLives = 10;
    public Image[] hearts;
    public Sprite heart;
    public Text tutorial;

    [Header("Audio & VFX")]
    public AudioClip hit;
    public GameObject hitEffectPrefab;

    [Header("References")]
    public Rigidbody2D rb;
    public Camera cam;
    public GameObject gm;

    private int lifes;
    private Animator animator;
    private Vector2 mousePos;
    private Vector2 lookDir;
    // private bool comecou;

    private void Start()
    {
        lifes = maxLives;
        animator = GetComponent<Animator>();
        // comecou = false;
        gm = GameObject.Find("GameManager");
        AudioManager.StartMusic();
    }

    public void TakeDamage(int amount)
    {
        lifes -= amount;
        if (lifes <= 0) Die();
        animator.SetTrigger("Damage");

        if (hitEffectPrefab != null)
        {
            GameObject effect = Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }
    }

    public void Die()
    {
        gm.GetComponent<GM>().Lose();
    }

    public void OnFirstShot()
    {
        // if (!comecou)
        // {
        // comecou = true;
        if (tutorial != null)
        {
            tutorial.enabled = false;
        }
        // }
    }

    private void Update()
    {
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < lifes)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    void FixedUpdate()
    {
        lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
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
            TakeDamage(1);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        AudioManager.PlaySFX(hit);
        TakeDamage(1);
    }
}
