using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class CDataEnemy : MonoBehaviour
{
    private float Health = 100;
    [SerializeField]
    private bool IsAlert = false;

    [SerializeField]
    private List<GameObject> ListPatrollingPoints;
    private NavMeshAgent navMesh;
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask layer;
    public Animator Anim;

    Collider[] colliders = new Collider[50];
    Mesh mesh;
    public CRagdollController ragdollController;
    //[SerializeField]
    //public FieldOfView FienldOfWiew;

    public void Start()
    {
        Anim = GetComponent<Animator>();
        navMesh = GetComponent<NavMeshAgent>();
        ragdollController = GetComponent<CRagdollController>();
    }
    public List<GameObject> getListPatrollingPoints()
    {
        return this.ListPatrollingPoints;
    }
    private void Update()
    {
        //CanSee();
    }

    private void Scan()
    {
        //count = Physics.OverlapCapsuleNonAlloc(tranfo)
    }

    public Animator getAnimator()
    {
        return Anim;
    }
    public enum StateIdle
    {
        Reset = 0,
        sit = 1,
        Idle = 2


    };

    public StateIdle stateIdle;

    public enum StateOr
    {
        Idle = 0,
        Patrolling = 1
    };

    public StateOr StateOrientation = 0;

    public StateOr GetStateOr()
    {
        return this.StateOrientation;
    }
    public NavMeshAgent getNavMeshAgent()
    {
        return navMesh;
    }
    public StateIdle getStateIdle()
    {
        return this.stateIdle;
    }
    public float GetHealth()
    {
        return Health;
    }
    public void SetHealth(int Health)
    {
        this.Health = Health;
    }

    public void TakeDamage(float Damage)
    {
            Health -= Damage;
    }

    public bool getAlert()
    {
        return this.IsAlert;
    }

    public Mesh getMesh()
    {
        return this.mesh;
    }

    public void setAlert(bool IsAlert)
    {
        this.IsAlert = IsAlert;
    }
   
    //public void CanSee()
    //{
    //    if(FienldOfWiew.getCanSeePlayer() == true)
    //    { 
    //        this.IsAlert = FienldOfWiew.getCanSeePlayer();
    //    }

    //}

  public void Death()
    {
       if(Health <= 0)
        {
            ragdollController.SetEnabled(true);
        }
    }

    
    

}
