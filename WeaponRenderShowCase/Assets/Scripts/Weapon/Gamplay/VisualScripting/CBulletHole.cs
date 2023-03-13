using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBulletHole : MonoBehaviour
{
    public GameObject bulletHole;
    public GameObject bulletHoleWood;
    public GameObject bulletHoleSteel;
    public GameObject bulletHoleStone;
    public GameObject bulletHoleConcrete;
    public GameObject bulletHoleMeat;
    public float distance = 10f;
    Transform cam;
    public CTypeObjectBulletHole Hole;
    // Start is called before the first frame update
    private void Start()
    {
       
    }

    public void setCamera(Transform Cam)
    {
        this.cam = Cam;
    }

    public void bulletLogic()
    {
       
            RaycastHit hit;
            
            if (Physics.Raycast(cam.transform.position,cam.transform.forward,out hit, Mathf.Infinity))
            {
                
                var bulletHole =  hit.collider.GetComponent<CTypeObjectBulletHole>().BulletHole;
               
                    switch ((int)bulletHole)
                    {
                        case 0:
                            GameObject BH = Instantiate(bulletHoleWood, hit.point + new Vector3(0f, 0f, -0.02f), Quaternion.LookRotation(hit.normal));
                            break;

                        case 1:
                            BH = Instantiate(bulletHoleSteel, hit.point + new Vector3(0f, 0f, -0.02f), Quaternion.LookRotation(hit.normal));
                            break;
                        case 2:
                            BH = Instantiate(bulletHoleStone, hit.point + new Vector3(0f, 0f, -0.02f), Quaternion.LookRotation(hit.normal));
                            break;
                        case 3:
                            BH = Instantiate(bulletHoleConcrete, hit.point + new Vector3(0f, 0f, -0.02f), Quaternion.LookRotation(hit.normal));
                            break;
                        case 4:
                        BH = Instantiate(bulletHoleMeat, hit.point + new Vector3(0f, 0f, -0.02f), Quaternion.LookRotation(hit.normal));
                        break;

                    default:
                        BH = Instantiate(bulletHoleWood, hit.point + new Vector3(0f, 0f, -0.02f), Quaternion.LookRotation(hit.normal));
                        break;
                   

                }

               

            
        }
    }
}
