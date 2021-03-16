using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateAtaque : State
{
   SteerableBehaviour steerable;
   IShooter shooter;

   public override void Awake()
   {
       base.Awake();

       Transition ToPatrulha = new Transition();
       ToPatrulha.condition = new ConditionDistGT(transform,GameObject.FindWithTag("Player").transform,10.0f);
       ToPatrulha.target = GetComponent<StatePatrulha>();
       transitions.Add(ToPatrulha);


       steerable = GetComponent<SteerableBehaviour>();
       shooter = steerable as IShooter;
       if(shooter == null)
       {
           throw new MissingComponentException("Este GameObject não implementa IShooter");
       }
   }
   public float shootDelay = 2f;
   private float _lastShootTimestamp = 0.0f;
   public override void Update()
   {
       if (Time.time - _lastShootTimestamp < 2f) return;
       _lastShootTimestamp = Time.time;
       shooter.Shoot();
   }
}
