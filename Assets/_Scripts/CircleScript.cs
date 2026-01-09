using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleScript : MonoBehaviour, IDamageable
{
    public AudioClip pop;
    public GameObject player;

    private void Start() {
        player = GameObject.Find("Player");
    }
    public void TakeDamage(int amount)
    {
        Die();
    }
    public void Die(){
        Destroy(gameObject);
        AudioManager.PlaySFX(pop);
    }
    private void FixedUpdate() {
        float step =  2f * Time.deltaTime; 
        if (player != null){
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
        }
    }
}
