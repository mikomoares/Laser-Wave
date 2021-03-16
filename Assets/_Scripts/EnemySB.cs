using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySB : SteerableBehaviour
{
  private Vector3 direction;
  public AudioClip hit;


  private void OnTriggerEnter2D(Collider2D collision)
  {
      if (collision.CompareTag("Enemy")) return;

      IDamageable damageable = collision.gameObject.GetComponent(typeof(IDamageable)) as IDamageable;
      if (!(damageable is null))
      {
          damageable.TakeDamage();
          AudioManager.PlaySFX(hit);


      }
      Destroy(gameObject);
  }

  void Start()
  {
      GameObject player = GameObject.FindWithTag("Player");
      if(player != null){
          direction = (player.transform.position - transform.position).normalized;
      }
      
  }

  void Update()
  {
      Thrust(direction.x, direction.y);
  }

  private void OnBecameInvisible()
  {
      gameObject.SetActive(false);
  }
}
