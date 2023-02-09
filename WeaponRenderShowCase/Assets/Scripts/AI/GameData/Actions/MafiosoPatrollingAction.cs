using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MafiosoPatrollingAction : GoapAction
{
    //mafioso.playAnimationsController("RifleRun");
    private bool follow = false;
    private MafiosoValueComponent Val;
    public Transform[] points;
    public Transform[] waypoints;
    Vector3 target_Points;
    int waypointIndex;
    bool walkPointSet;
    public void Start()
    {
        Val = GetComponent<MafiosoValueComponent>();
    }
    public MafiosoPatrollingAction()
    {
        Debug.Log("Entra en el efecto del mafioso");
        addEffect("FollowPlayer", true);
        cost = -1f;
    }


    public override void reset()
    {
        follow = false;
        target = null;
    }

    public override bool isDone()
    {
        return follow;
    }

    //Esta parte seria apropiada revisar tambien el Codigo de Fear SDK 1.8 para cambiarlo
    // Tendria que asignar el target mediante el agent y no de esta manera
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        target = GameObject.Find("Player");
        return target != null;
    }
    private void SearchPattern()
    {
        //    if (agent.transform.position != points[current].position)
        //    {
        //        agent.SetDestination(points[current].position);
        //    }
        //    else
        //        current = current + 1;
        if (Vector3.Distance(transform.position, target_Points) < 1)
        {
            UpdateDestination();
            IterateWayPointIndex();
        }
    }

    void UpdateDestination()
    {

        //if (ExtrLibrary.isArrayemptyTransform(waypoints))
        //{
            target = (waypoints[waypointIndex]).gameObject;
            agent.SetDestination(target.transform.position);
        //}
    }

    void IterateWayPointIndex()
    {
        //if (ExtrLibrary.isArrayemptyTransform(waypoints))
        //{
            waypointIndex++;
            if (waypointIndex == waypoints.Length)
            {
                waypointIndex = 0;
            }
        //}
    }
    private void Patroling()
    {
       
        if (!walkPointSet) SearchPattern();

        if (walkPointSet)
            agent.SetDestination(target.transform.position);

        //Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //if (distanceToWalkPoint.magnitude < 1f)
        walkPointSet = false;

        //GetComponent<MeshRenderer>().material = green;
    }

    public override bool perform(GameObject agent)
    {

        Mafioso currMafioso = agent.GetComponent<Mafioso>();


        Debug.Log("Entra en Perform en la accion");
        if (!currMafioso.ShootRangeFindOFWiew.canSeePlayer)
        {
            Patroling();
           

            follow = true;
            return true;
        }
        else
        {
            Val.playAnimationsController("WalkWithRifle");
            return false;
        }
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override int TakeDamage(int _damage)
    {
        throw new System.NotImplementedException();
    }

    public override bool requiredIsStatic()
    {
        return false;
    }
}
