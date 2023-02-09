using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CHitmarket : MonoBehaviour
{

    [SerializeField]public GameObject marketUI;
    [SerializeField] private float TimeDurationHitMark;
    public  void  Hit()
    {
        StartCoroutine(HitWeapon());
    }

    IEnumerator HitWeapon()
    {
        marketUI.SetActive(true);
        yield return new WaitForSeconds(TimeDurationHitMark);
        marketUI.SetActive(false);
    }

}
