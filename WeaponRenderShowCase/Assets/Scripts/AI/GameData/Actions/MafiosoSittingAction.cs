using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MafiosoSittingAction : GoapAction
{
    
    private bool isSitting = false;
    private MafiosoValueComponent val;

    public void Start()
    {
        val = GetComponent<MafiosoValueComponent>();
    }

    public MafiosoSittingAction()
    {
        addEffect("MoveToPlayer", true);
        cost = 1f;
    }

    public override void reset()
    {
        isSitting = false;
        target = null;
    }

    public override bool isDone()
    {
        return isSitting;
    }

    public override bool checkProceduralPrecondition(GameObject agent)
    {

        Mafioso mafioso = agent.GetComponent<Mafioso>();
        target = mafioso.player;
        return target != null;



    }
    public void Update()
    {
       Debug.Log("SittingAction: " + (val.FindOfView.GetComponent<FindOfView>().getCanSeePlayer()));
    }
    public override bool perform(GameObject agent)
    {
    
        Mafioso currMafioso = agent.GetComponent<Mafioso>();
        if (val.FindOfView.GetComponent<FindOfView>().getCanSeePlayer())
        {
            Debug.Log("FindOfView: " + val.FindOfView.GetComponent<FindOfView>().getCanSeePlayer());


            isSitting = true;
            return true;
        }
        else
        {
            val.playAnimationsController("HappyIdle");
            return false;
        }
       
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override int TakeDamage(int _damage)
    {
        return 0;
    }

    public override bool requiredIsStatic()
    {
        return true;
    }
}