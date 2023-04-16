using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DL
{
    public class CManagerWeapon : MonoBehaviour
    {
        public GameObject AmmoUI;

        //Crear una estructura para agregar los datos de las armas 
        //private struct Weapon
        //{

        //}
        private GameObject[] _allWeapon = new GameObject[4];
        [SerializeField] private List<GameObject> _allWeaponAssets;
        [SerializeField] private List<GameObject> weapons = new List<GameObject>();
        [SerializeField] private GameObject[] auto_spawn_weapon = new GameObject[4];
        private List<CArmed> _ListHaveWeapon = new List<CArmed>();
        [SerializeField] private Transform[] _tranformSlot = new Transform[2];
        private int selectedWeapon = 0;
        //[SerializeField] private GameObject[] weapons;
        //[SerializeField] private float SwitchDelay = 1f;

        [SerializeField] private Vector3 vectorOffsetSpawnWeapon;
        [SerializeField] private Vector3 vectorOffsetRotationWeapon;
        private int index;
        private bool isSwitching;
        private GameObject CurrentWeapon;
        [SerializeField]private Transform _trasnformWeapon;
        private static CManagerWeapon Inst;
        private GameObject PreviusWeapon;
        private CManagerPickUp ManagerPickUp;
        public Transform SpawnCollection;
        private void Awake()
        {
            //Auto Preset Prefab
            _allWeapon[0] = Resources.Load<GameObject>("Assets/Prefabs/Weapons/ppk.prefab");
            _allWeapon[1] = Resources.Load<GameObject>("Assets/Prefabs/Weapons/PlayHolderWeapon/AK74M.prefab");
            _allWeapon[2] = Resources.Load<GameObject>("Assets/Prefabs/Weapons/PlayHolderWeapon/M4A1.prefab");
            _allWeapon[3] = Resources.Load<GameObject>("Assets/Prefabs/Weapons/PlayHolderWeapon/M4Shootgun.prefab");

        }
        private void Start()
        {
            AmmoUI = GameObject.Find("AmmoUi");
            ManagerPickUp = GameObject.Find("ManagerPickUp").GetComponent<CManagerPickUp>();
        }
        public void Update()
        {
           for(int i = weapons.Count -1; i>= 0; i--)
          {
            if (weapons[i] == null)
                weapons.RemoveAt(i);
         }
            EquipWeapon();
            //SelectedWeapon();

            //GetWeaponArray();
            DropWeapon();
            NotCurrentWeapon();
            UpdateAmmo();
        }
        
        private void AutoAsignedWeaponAfterDrop()
        {
            if(CurrentWeapon == null)
            {
                if (selectedWeapon >= transform.childCount - 1)
                {
                    selectedWeapon = 0;
                }
                else
                {
                    selectedWeapon++;
                }
                //GetCurrentWeapon().GetComponent<CArmed>().Desequip();
                //if (GetCurrentWeapon().GetComponent<CArmed>().IsFinishAnimation(" "))
                // { 

                //}
            }
           
                if (selectedWeapon <= 0)
                {
                    selectedWeapon = transform.childCount - 1;
                }
                else
                    //GetCurrentWeapon().GetComponent<CArmed>().Desequip()
                    //if (GetCurrentWeapon().GetComponent<CArmed>().IsFinishAnimation(" "))
                    //{

                    selectedWeapon--;


                //}
            
        }

        public void AddWeapon(GameObject Weapon)
        {
            if(weapons.Count <= 2)
            {
                foreach(GameObject w in weapons)
                {
                    var ScrtiptWeapon = w.GetComponent<CArmed>();
                    var ScriptableAddWeapon = Weapon.GetComponent<CArmed>();
                    if( ScrtiptWeapon.GetWeaponName() != ScriptableAddWeapon.GetWeaponName())
                    {
                  
                        continue;
                    }
                    else
                    {
                        return;
                    }
                }
                if(weapons.Count <= 0)
                {
                    
                    selectedWeapon = 0;
                    CurrentWeapon = Spawn(gameObject.transform.position,Weapon);
                    CurrentWeapon.SetActive(true);
                    //Debug.Log("Entra en agregar el arma");
                    
                //SelectWeapon(id);
                }
                else
                {
                    CurrentWeapon.SetActive(false);
                    CurrentWeapon = Spawn(gameObject.transform.position, Weapon);
                    selectedWeapon = transform.childCount - 1;
                    SelectedWeapon();
                    CurrentWeapon.SetActive(true);

                }
            }
        }
        private void SelectedWeapon()
        {
            //TODO:Reacer la funcion que no ejecuta correctamente el Equip
            //No debe tener parametros y debe ser completarse de forma rapida
            int i = 0;
            bool isAnim = false;
            foreach (Transform weapon in transform)
            {
               //
                if (weapon.GetSiblingIndex() == selectedWeapon)
                {
                 if(GetCurrentWeapon() != null)
                    { 
                    Debug.LogWarning(i);
                    GetCurrentWeapon().GetComponent<CArmed>().Equip();
                    weapon.gameObject.SetActive(true);
                    CurrentWeapon = weapon.gameObject;
                    }
                  else
                    {
                        CurrentWeapon = PreviusWeapon;
                        SelectedWeapon();
                    }
                }
                else
                {
                    Debug.LogWarning(i);
                    weapon.gameObject.SetActive(false);
                }
              
                //weapon.SetSiblingIndex(selectedWeapon);
                //i++;
                //i++;

            }
        }
        private void EquipWeapon()
        {
            int previousSelectedWeapon = selectedWeapon;
            PreviusWeapon = CurrentWeapon;
            if (CurrentWeapon != null)
            {
                if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                    if (selectedWeapon >= transform.childCount - 1)
                    {
                        
                        selectedWeapon = 0;
                      

                    }
                    else
                    {
                        
                        selectedWeapon++;
              

                    }
                        //GetCurrentWeapon().GetComponent<CArmed>().Desequip();
                        //if (GetCurrentWeapon().GetComponent<CArmed>().IsFinishAnimation(" "))
                        // { 
                
                    //}
                }
                if (Input.GetAxis("Mouse ScrollWheel") < 0f)
                {
                    if (selectedWeapon <= 0)
                    {
                       
                        selectedWeapon = transform.childCount - 1;
                       

                    }
                    else
                        //GetCurrentWeapon().GetComponent<CArmed>().Desequip()
                        //if (GetCurrentWeapon().GetComponent<CArmed>().IsFinishAnimation(" "))
                        //{
                    
                    selectedWeapon--;
                    

                    //}
                }

                if (previousSelectedWeapon != selectedWeapon)
                {
                    
                    SelectedWeapon();
                }

            }
            else
            {
               
                selectedWeapon = 0;
            }
        }

        public GameObject Spawn(Vector3 post, GameObject _Weapon)
        {
            GameObject obj = (GameObject)Instantiate(_Weapon, post, Quaternion.identity);
           
            obj.transform.parent = gameObject.transform;
            weapons.Add(obj); obj.transform.localEulerAngles= Vector3.zero;
            obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
            obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x,vectorOffsetRotationWeapon.y,vectorOffsetRotationWeapon.z));
           
            CArmed newWeapon = obj.GetComponent<CArmed>();
            _ListHaveWeapon.Add(newWeapon);

            return obj;
        }
        //Probar
        private void DropWeapon()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {

                //Todo:Dropea el arma, probar
                var nameWeapon = CurrentWeapon.GetComponent<CArmed>().weapon_name;
                Debug.Log(nameWeapon);
                GameObject PickUpGameObject = CManagerPickUp.Inst.getWeaponAsset(nameWeapon);
                Debug.Log(PickUpGameObject.name);
                
                //CurrentWeapon = PreviusWeapon;
                //weapons.Remove(CurrentWeapon);
                //weapons[0] = CurrentWeapon;
                PickUpGameObject.GetComponent<CWeaponPickUp>().bulletInMag = CurrentWeapon.GetComponent<CArmed>().GetAmmo_in_Mag();
                PickUpGameObject.GetComponent<CWeaponPickUp>().bullets = 0;

                
               

                //CManagerPickUp.Inst.SpawnWeapon(Vector3.forward * 50,  );
                selectedWeapon = 0;
               
              
                SelectedWeapon();
                EquipWeapon();
                GameObject obj = (GameObject)Instantiate(PickUpGameObject, SpawnCollection.position, Quaternion.identity);
                CWeaponPickUp newWeapon = obj.GetComponent<CWeaponPickUp>();
                ManagerPickUp.AddList(newWeapon);
                CurrentWeapon.GetComponent<CArmed>().Drop();
                //CurrentWeapon = PreviusWeapon;
                //selectedWeapon = transform.childCount - 1;


                //EquipWeapon();
                //SelectedWeapon();




            }
        }
        public void NotCurrentWeapon()
        {
            //Todo: Agregar requisitos y alguna interfaz para indicar que hace y cuanta energia me queda
            //if (Input.GetKeyDown(KeyCode.Mouse0) && weapons.Count <= 0)
            //{
            //    AutoSpawn();
            //}
            if(!SearchWeapon(auto_spawn_weapon[0].name))
            {

                if (Input.GetKeyDown(KeyCode.Alpha1) && weapons.Count <= 0)
                {
                    AddWeapon(auto_spawn_weapon[0]);
                }
                if (Input.GetKeyDown(KeyCode.Alpha2) && weapons.Count <= 1)
                {
                    AddWeapon(auto_spawn_weapon[0]);
                }
            }

            //if (Input.GetKeyDown(KeyCode.Alpha3))
            //{
            //    AddWeapon(auto_spawn_weapon[2]);
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha4))
            //{
            //    AddWeapon(auto_spawn_weapon[3]);
            //}

            //if (Input.GetKeyDown(KeyCode.Alpha5))
            //{
            //    AddWeapon(auto_spawn_weapon[4]);
            //}
            //if (Input.GetKeyDown(KeyCode.Alpha6))
            //{
            //    AddWeapon(auto_spawn_weapon[5]);
            //}


        }
        //private void Desequiped()
        //{
        //    CurrentWeapon.SetActive(false);
        //}
        //private void Equipped()
        //{
        //    CurrentWeapon.SetActive(true);
        //}

        public GameObject GetCurrentWeapon()
        {
            return CurrentWeapon;
        }

        //public void GetWeaponArray()
        //{
        //    foreach (GameObject w in weapons)
        //    {
        //        Debug.Log(w.name);
        //    }
        //}
        
        private void AutoSpawn()
        {
            int autoSpawn = Random.Range(0, auto_spawn_weapon.Length-1);
            AddWeapon(auto_spawn_weapon[autoSpawn]);
        }
        private bool SearchWeapon(string Name)
        {
            foreach (GameObject w in weapons)
            {
                if(w.name == Name)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateAmmo()
        {
            if (CurrentWeapon != null)
            {
                AmmoUI.GetComponent<CAmmoCotroller>().SetAmmo(CurrentWeapon.GetComponent<CArmed>().ammo_in_mag);
                AmmoUI.GetComponent<CAmmoCotroller>().SetExtraAmmo(CurrentWeapon.GetComponent<CArmed>().extra_ammo);
            }
            else
            {
                AmmoUI.GetComponent<CAmmoCotroller>().SetAmmo(0);
                AmmoUI.GetComponent<CAmmoCotroller>().SetExtraAmmo(0);
            }
      
        }

     
    }
}
