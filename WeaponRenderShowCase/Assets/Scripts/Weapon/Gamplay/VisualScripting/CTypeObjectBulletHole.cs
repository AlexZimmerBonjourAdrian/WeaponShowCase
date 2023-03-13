using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTypeObjectBulletHole : MonoBehaviour
{
   
  public enum TypeObjectBulletHole
    {
        Wood=0,Steel=1,Stone=2,Concrete=3,Meat=4
    };
    
    [SerializeField]
    public TypeObjectBulletHole BulletHole;

}
