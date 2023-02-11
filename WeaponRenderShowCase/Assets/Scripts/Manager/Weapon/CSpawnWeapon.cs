using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CSpawnWeapon : MonoBehaviour
{
   //Bien estructuremos esto
   //Que nesecito 
   //Quiero generar un arma apretando su boton correspondiente, y me desactive el arma anterior
   //Se me ocurren dos una un sistema de generacion automatica e autoasignacion del arma y la otra que el arma se active y se desactive 
   //Para eso nesecito lo siguiente ListWeaponCompleteList con todas las armas
   //un Curren Weapon el cual se pueda remplazar
   //Chequear la current Weapon
   //Dar una funcion de codigo si esta o no desactivada el arma
   //
   [SerializeField]
    private List<GameObject> ListWeaponCompleteList;

    [SerializeField]
    private KeyCode key_1 = KeyCode.Alpha1;
    [SerializeField]
    private KeyCode key_2= KeyCode.Alpha2;
    [SerializeField]
    private KeyCode key_3= KeyCode.Alpha3;
    [SerializeField]
    private KeyCode key_4 = KeyCode.Alpha4;
    [SerializeField]
    private KeyCode key_5= KeyCode.Alpha5;
    [SerializeField]
    private KeyCode key_6 = KeyCode.Alpha6;

    [SerializeField]
    private GameObject CurrentWeapon=null;
    
    [SerializeField]
    private List<GameObject> ListSpawnedWeapon;
    [SerializeField] private Vector3 vectorOffsetSpawnWeapon;
    [SerializeField] private Vector3 vectorOffsetRotationWeapon;
    private int LastWeapon;
    private int SelectedWeapon;
    public void Update()
    {
        WeaponGenerator();
        SwitchWeapon();
    }

    #region IAAutoGenerateWeapon
    public bool IsDuplicated(String NameWeapon)
    {
        foreach(GameObject var in ListSpawnedWeapon)
        {
            if (var.GetComponent<CArmed>().weapon_name == NameWeapon)
                return true;
        }
        return false;
    }
    private void WeaponGenerator()
    {
        
        if (Input.GetKeyDown(key_1))
        {
            //Create PPK
            Debug.Log("PPK");
            if(!IsDuplicated(ListWeaponCompleteList[0].GetComponent<CArmed>().weapon_name))
            { 
            GameObject obj = (GameObject)Instantiate(ListWeaponCompleteList[0],gameObject.transform.position, Quaternion.identity);
               
               
                obj.transform.parent = gameObject.transform;
                obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
                obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x, vectorOffsetRotationWeapon.y, vectorOffsetRotationWeapon.z));
                ListSpawnedWeapon.Add(obj);
            CurrentWeapon = obj;
              
            }
    

        } 
        if(Input.GetKeyDown(key_2))
        {
            //Create mp5k
            if (!IsDuplicated(ListWeaponCompleteList[1].GetComponent<CArmed>().weapon_name))
            {
                Debug.Log("MP5K");
            GameObject obj = (GameObject)Instantiate(ListWeaponCompleteList[1], transform.position, Quaternion.identity);
                obj.transform.parent = gameObject.transform;
                obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
                obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x, vectorOffsetRotationWeapon.y, vectorOffsetRotationWeapon.z));
                ListSpawnedWeapon.Add(obj);
                CurrentWeapon = obj;
            }
            
        }
        if(Input.GetKeyDown(key_3))
        {
            if (!IsDuplicated(ListWeaponCompleteList[2].GetComponent<CArmed>().weapon_name))
            {
                //Create m4shootgun
                Debug.Log("M4shootgun");
                GameObject obj = (GameObject)Instantiate(ListWeaponCompleteList[2], transform.position, Quaternion.identity);
                obj.transform.parent = gameObject.transform;
                obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
                obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x, vectorOffsetRotationWeapon.y, vectorOffsetRotationWeapon.z));
                ListSpawnedWeapon.Add(obj);
                CurrentWeapon = obj;
            }
            
        } 
        if(Input.GetKeyDown(key_4))
        {
            if (!IsDuplicated(ListWeaponCompleteList[3].GetComponent<CArmed>().weapon_name))
            {
                //Create m4a1
                Debug.Log("M4A1");
                GameObject obj = (GameObject)Instantiate(ListWeaponCompleteList[3], transform.position, Quaternion.identity);
                obj.transform.parent = gameObject.transform;
                obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
                obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x, vectorOffsetRotationWeapon.y, vectorOffsetRotationWeapon.z));
                ListSpawnedWeapon.Add(obj);
                CurrentWeapon = obj;

            }
         

        } 
        if(Input.GetKeyDown(key_5))
        {
            if (!IsDuplicated(ListWeaponCompleteList[4].GetComponent<CArmed>().weapon_name))
            {
                //Create Calico
                Debug.Log("Calico");
                GameObject obj = (GameObject)Instantiate(ListWeaponCompleteList[4], transform.position, Quaternion.identity);
                obj.transform.parent = gameObject.transform;
                obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
                obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x, vectorOffsetRotationWeapon.y, vectorOffsetRotationWeapon.z));
                ListSpawnedWeapon.Add(obj);
                CurrentWeapon = obj;
               
            }
           

        } 
        if(Input.GetKeyDown(key_6))
        {
            if (!IsDuplicated(ListWeaponCompleteList[5].GetComponent<CArmed>().weapon_name))
            {
                //Create ak74M
                Debug.Log("AK74M");
                GameObject obj = (GameObject)Instantiate(ListWeaponCompleteList[5], transform.position, Quaternion.identity);
                obj.transform.parent = gameObject.transform;
                obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
                obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x, vectorOffsetRotationWeapon.y, vectorOffsetRotationWeapon.z));
                ListSpawnedWeapon.Add(obj);
                CurrentWeapon = obj;
               
            }
          

        } 
    }

    public GameObject Spawn(Vector3 post, GameObject _Weapon)
    {
        GameObject obj = (GameObject)Instantiate(_Weapon, post, Quaternion.identity);

        obj.transform.parent = gameObject.transform;
        ListSpawnedWeapon.Add(obj); 
        obj.transform.localPosition = new Vector3(vectorOffsetSpawnWeapon.x, vectorOffsetSpawnWeapon.y, vectorOffsetSpawnWeapon.z);
        obj.transform.localRotation = Quaternion.Euler(new Vector3(vectorOffsetRotationWeapon.x, vectorOffsetRotationWeapon.y, vectorOffsetRotationWeapon.z));
        

        return obj;
    }
    private bool CheckWeaponCurrent()
    {
        if (CurrentWeapon != null)
        {
            return true;
        }

        return false;

    }

    private void EquipWeapon()
    {
        int previusSelectedWeapon = SelectedWeapon;

        if(CurrentWeapon != null)
        {

        }
    }

    private void AddWeapon(GameObject Weapon)
    {
        if (ListSpawnedWeapon.Count <= 0)
        {
            CurrentWeapon = Spawn(gameObject.transform.position, Weapon);
            CurrentWeapon.SetActive(true);
        }
    }


    private void SwitchWeapon()
    {
        
        LastWeapon = transform.childCount - 1;

        if (CheckWeaponCurrent())
        {
           
                //Codigo de cambio de armas
                //if (Input.GetKeyDown(key_1))
                //{
                CurrentWeapon = ListSpawnedWeapon[LastWeapon];
                if(ListSpawnedWeapon.Count <= 0)
                {
                CurrentWeapon = ListSpawnedWeapon[0];
                //CurrentWeapon.gameObject.SetActive(false);
                }
                else
                 {
                    CurrentWeapon.gameObject.SetActive(false);
                 }

                //Create PPK
                Debug.Log("PPK");
                LastWeapon = 0;
                //ListSpawnedWeapon[0].SetActive(true);

              
                CurrentWeapon = ListSpawnedWeapon[0];
            }
            if (Input.GetKeyDown(key_2))
            {
            //CurrentWeapon = ListSpawnedWeapon[LastWeapon];
            //if (CurrentWeapon != null && !(ListSpawnedWeapon.Count <= 0))
            //{
            CurrentWeapon = ListSpawnedWeapon[LastWeapon];
            if (ListSpawnedWeapon.Count <= 0)
            {
                CurrentWeapon = ListSpawnedWeapon[1];
                //CurrentWeapon.gameObject.SetActive(false);
            }
            else
            {
                CurrentWeapon.gameObject.SetActive(false);
            }

            //Create mp5k
            Debug.Log("MP5K");
                LastWeapon = 1;
                ListSpawnedWeapon[1].SetActive(true);
                CurrentWeapon = ListSpawnedWeapon[1];
            }
            if (Input.GetKeyDown(key_3))
            {
               
                //Create m4shootgun
                Debug.Log("M4shootgun");
                //LastWeapon = 1;
                //ListSpawnedWeapon[2].SetActive(true);
                //LastWeapon = transform.childCount - 1;
                CurrentWeapon = ListSpawnedWeapon[2];
            }
            if (Input.GetKeyDown(key_4))
            {
              
                //Create m4a1
                Debug.Log("M4A1");
                LastWeapon = 3;
                //ListSpawnedWeapon[3].SetActive(true);
                //LastWeapon = transform.childCount - 1;
                CurrentWeapon = ListSpawnedWeapon[3];
            }
            if (Input.GetKeyDown(key_5))
            {
              
                //Create Calico
                Debug.Log("Calico");
                //ListSpawnedWeapon[4].SetActive(true);
                LastWeapon = 4;
                //LastWeapon = transform.childCount - 1;
                CurrentWeapon = ListSpawnedWeapon[4];
            }
            if (Input.GetKeyDown(key_6))
            {
               
                //Create ak74M
                Debug.Log("AK74M");
                LastWeapon = 5;
                //ListSpawnedWeapon[5].SetActive(true);
                //LastWeapon = transform.childCount - 1;
                CurrentWeapon = ListSpawnedWeapon[5];

            }

      

        }
    

    #endregion
}
