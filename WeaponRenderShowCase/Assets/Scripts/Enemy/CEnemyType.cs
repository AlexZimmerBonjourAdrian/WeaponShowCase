using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CEnemyType : MonoBehaviour
{



    //public enum EnemyTypeWeapon
    //{


    //};

    private const int MP5K = 1;
    private const int M4A1 = 2;
    private const int AK74 = 3;
    private const int CALICO = 4;
    private const int M4SHOTGUN = 5;

   
    [SerializeField]
    private int State;
    
    
   private GameObject CurrentWeapon;

  
    
    // Start is called before the first frame update
    void Start()
    {
        switch(State)
        {
            case (MP5K):
            {
                    CurrentWeapon = CManageResources.Inst.getmp5kPicKUP();
              break;
            }
            case (AK74):
                {
                    CurrentWeapon = CManageResources.Inst.getAK74MicKUP();
                    break;
                }
            case (CALICO):
                {
                    CurrentWeapon = CManageResources.Inst.getCalicoPicKUP();
                    break;
                }
            case (M4A1):
                {
                    CurrentWeapon = CManageResources.Inst.getM4A1PicKUP();
                    break;
                }
            case (M4SHOTGUN):
                {
                    CurrentWeapon = CManageResources.Inst.getM4shotgunPicKUP();
                    break;
                }
            default:
                Debug.LogError("No existe el pickUp");
                break;
            
        }
    }

    public void SpawnPickUpWeapon()
    {
        CControllerPickup.Inst.AddPickUp(CurrentWeapon, transform);
    }

    
}
