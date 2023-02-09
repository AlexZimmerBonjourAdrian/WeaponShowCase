using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MafiosoFollowPlayerAction : GoapAction
{

    private bool attacked = false;
    private MafiosoValueComponent val;

    public void Start()
    {
        val = GetComponent<MafiosoValueComponent>();
    }

    public MafiosoFollowPlayerAction()
    {
        addPrecondition("MoveToPlayer", true);
        addEffect("KillPlayer", true);
        cost = 1f;
    }

    public override void reset()
    {
       
        attacked = false;
        target = null;
    }

    public override bool isDone()
    {
        return attacked;
    }
    public void Update()
    {
        Debug.Log("SittingAction: " + (val.FindOfView.GetComponent<FindOfView>().getCanSeePlayer()));
    }
    public override bool checkProceduralPrecondition(GameObject agent)
    {
        Mafioso mafioso = agent.GetComponent<Mafioso>();
        target = mafioso.player;
        return target != null;
    }

    public override bool perform(GameObject agent)
    {
        Mafioso currMafioso = agent.GetComponent<Mafioso>();

        if (val.FindOfView.GetComponent<FindOfView>().getCanSeePlayer())
        {
            val.playAnimationsController("FiringRiffle");
            Rigidbody rb = Instantiate(currMafioso.Projectile, currMafioso.ShootTransform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce((target.transform.position - currMafioso.ShootTransform.position).normalized * 32f, ForceMode.Impulse);

            attacked = true;
            return true;
        }
        else
        {
            val.playAnimationsController("RifleRun");
            return false;
        }
    }

    public override bool requiresInRange()
    {
        return true;
    }

    public override int TakeDamage(int _damage)
    {
        attacked = true;
        return _damage;
    }

    public override bool requiredIsStatic()
    {
        return false;
    }
}

