using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
       if(other.gameObject.tag == "Player")
        { 
        CGameEvents.current.DoorwayTriggerEnter();
        }
    }
}
