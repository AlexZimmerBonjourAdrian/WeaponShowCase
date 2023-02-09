using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DL;
public class CRecoil : MonoBehaviour
{
    //[SerializeField] private CPlayer Player_Script;

    //private bool isAiming;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

   

    [SerializeField] private CManagerWeapon loadOut;

    private void Start()
    {
        loadOut = transform.Find("WeaponManager").GetComponent<CManagerWeapon>();

    }

    void Update()
    {
        if(loadOut.GetCurrentWeapon() != null)
        { 
        //isAiming = Player_Script.aiming;
        //targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        //currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.fixedDeltaTime);
        //transform.localRotation = Quaternion.Euler(currentRotation);
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, loadOut.GetCurrentWeapon().GetComponent<CArmed>().GetReturnSpeed() * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, loadOut.GetCurrentWeapon().GetComponent<CArmed>().GetSnappiness() * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(currentRotation);
        }
    }

    public void RecoilFire()
    {
        //targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
        targetRotation += new Vector3(loadOut.GetCurrentWeapon().GetComponent<CArmed>().GetRecoilX(), Random.Range(-loadOut.GetCurrentWeapon().GetComponent<CArmed>().GetRecoilY(), loadOut.GetCurrentWeapon().GetComponent<CArmed>().GetRecoilY()), Random.Range(-loadOut.GetCurrentWeapon().GetComponent<CArmed>().GetRecoilZ(), loadOut.GetCurrentWeapon().GetComponent<CArmed>().GetRecoilZ()));

    }
}
