using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CManagerPickUp : MonoBehaviour
{

    public List<Transform> transforms;
   [SerializeField] private List<GameObject> _WeaponAsset;
    private List<GameObject> _PickList;
     private List<CWeaponPickUp> _PickUpList;
    public static CManagerPickUp Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("PickUpManager");
                return obj.AddComponent<CManagerPickUp>();
            }
            return _inst;
        }
    }
    private static CManagerPickUp _inst;
    private void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        _inst = this;
        //_bulletAsset = Resources.Load<GameObject>("GenericBullet");
        // _bulletList = new List<CGenericBullet>();
    }

    // Start is called before the first frame update
  
    // Update is called once per frame
    void Update()
    {
        //for (int i = _PickList.Count - 1; i >= 0; i--)
        //{
        //    if (_PickList[i] == null)
        //        _PickList.RemoveAt(i);
        //}

    }

    public GameObject getWeaponAsset(string Name)
    {
        switch(Name)
        {
            case "MP5K":
                return _WeaponAsset[0]; 
            case "M4A1":
                return _WeaponAsset[1];
               
            case "AK74M":
                return _WeaponAsset[2];
                
            case "M4SHOOTGUN":
                 return _WeaponAsset[3];
               
            case "CALICO":
                return _WeaponAsset[4];
                

            default:
                Debug.LogError("No encuentra ninguna arma");
                break;
        };

        return null;
    }
    public void AddList(CWeaponPickUp obj)
    {
       _PickUpList.Add(obj);
    
    }
  
    public void SpawnWeapon(Vector3 post, GameObject _AssetPickUp)
    {

        GameObject obj = (GameObject)Instantiate(_AssetPickUp, post, Quaternion.identity);

        CWeaponPickUp newWeapon = obj.GetComponent<CWeaponPickUp>();
        
        //_PickUpList.Add(newWeapon);
        _PickList.Add(obj);

    }
   
}
