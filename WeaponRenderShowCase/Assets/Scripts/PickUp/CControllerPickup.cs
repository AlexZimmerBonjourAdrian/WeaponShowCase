using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
public class CControllerPickup : MonoBehaviour
{

    //private Keyboard kb = Keyboard.current;
    // Start is called before the first frame update
    //[SerializeField] private List<Transform> _SpawnTransform;
    //[SerializeField] private List<GameObject> _WeaponAsset;
    public static CControllerPickup Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("ControllerPickUP");
                return obj.AddComponent<CControllerPickup>();
            }
            return _inst;
        }
    }
    private static CControllerPickup _inst;
    public void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }


    public void Update()
    {
       
    }
    private void TestController()
    {
        //if(Input.GetKeyDown(KeyCode.Q))
        //{
        //    int WeaponId = Random.Range(0, _WeaponAsset.Count);
        //    int Positiion = Random.Range(0, _SpawnTransform.Count);
        //    CManagerPickUp.Inst.SpawnWeapon(_SpawnTransform[Positiion].position, _WeaponAsset[WeaponId]);
        //}
        //if(kb.eKey.wasPressedThisFrame)
        //{
        //    CManagerPickUp.Inst.SpawnWeapon(transform.position, _AssetMP5K);
        //}
    }


    public void  AddPickUp(GameObject weapon, Transform Position)
    {
        CManagerPickUp.Inst.SpawnWeapon(Position.position, weapon);

    }
}
