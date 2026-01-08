using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class VolantBehaviour : MonoBehaviour, IDamageable  
{
    public GameObject particle;
    public Vector2 speed = new Vector2(0f, 2f);
    
    private int life = 3;
    private float angle = 0;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        angle += 0.1f;
        if (angle > 2.0f * Mathf.PI) angle = 0.0f;
        
        Vector2 movement = new Vector2(0, Mathf.Cos(angle)) * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }
    public void TakeDamage()
    {
        if (life == 1){
            Die();
        }else{
            life--;
        }
    }
    public void Die(){
        Destroy(gameObject);
        Instantiate(particle, this.transform.position, Quaternion.identity);
    }
}
