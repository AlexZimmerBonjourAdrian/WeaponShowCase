using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Mafioso : Enemy
{

    private void Start()
    {
        health = 100;
        speed = 20;
        regenRate = 50f;
        initialSpeed = (speed / 10) / 2;
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        EnemyType = GetComponent<CEnemyType>();
        
        //minDist = UnityEngine.Random.Range(6f, 40f);
        //aggroDist = UnityEngine.Random.Range(6f, 60f);
        minDist = 6f;
        aggroDist = 40f;
        if (GameObject.Find("Player") != null)
        { 
            player = GameObject.Find("Player");
        }

        ShootRangeFindOFWiew = GameObject.Find("Character1_Head").GetComponent<FindOfView>();
        
        if(GameObject.Find("Player").transform != null)
        { 
         PositionEnemy = GameObject.Find("Player").transform;
        }


    }
    public override HashSet<KeyValuePair<string, object>> createGoalState()
    {
        HashSet<KeyValuePair<string, object>> goal = new HashSet<KeyValuePair<string, object>>();
        goal.Add(new KeyValuePair<string, object>("Waiting", false));
        goal.Add(new KeyValuePair<string, object>("MoveToPlayer", true));
        goal.Add(new KeyValuePair<string, object>("KillPlayer", true));
        
        return goal;
    }

    public override void TakeDamage(int damage)
    {
        Debug.Log("Vida" + health);
        animator.Play("hit");
        health = health - damage;
       
        if(health <= 0)
        {
          // EnemyType.SpawnPickUpWeapon();
           MafiosoValues.SetEnable(true);
            
        }
    }
   

    public override void deathMe()
    {
        isDeath = true;
    }

    public override void hitDamage()
    {
       
    }
}
