using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class CRagdollController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private NavMeshAgent agent;
    private Rigidbody[] rigidbodies;
    // Start is called before the first frame update
    void Start()
    {
        rigidbodies = transform.GetComponentsInChildren<Rigidbody>();
       
        SetEnabled(false);
        
    }

    public void SetEnabled(bool enabled)
    {

        bool isKinematic = !enabled;
        foreach(Rigidbody rigidbody in rigidbodies)
        {
            rigidbody.isKinematic = isKinematic;
        }

        animator.enabled = !enabled;
        agent.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    SetEnabled(true);
        //}
        //if(Input.GetKeyDown(KeyCode.T))
        //{
        //    SetEnabled(false);
        //}
    }
}
