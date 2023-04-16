using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    public CDataBullet Bullets;

    #region BulletAddsFunction
    public void AddBulletMP5K(int Monto)
    {
        Bullets.BulletMP5K += Monto;
    }

    public void AddBulletM4A1(int Monto)
    {
        Bullets.BulletM4A1 += Monto;
    }

    public void AddBulletAK74M(int Monto)
    {
        Bullets.BulletAK74M += Monto;
    }

    public void AddBulletCALICO(int Monto)
    {
        Bullets.BulletCalico += Monto;
    }

    public void AddBulletM4SHOOTGUN(int Monto)
    {
        Bullets.BulletM4Shootgun += Monto;
    }
    #endregion

    #region BulletRemoveFunction

    public void RemoveBulletMP5K(int Monto)
    {
        Bullets.BulletMP5K -= Monto;
    }

    public void RemoveBulletM4A1(int Monto)
    {
        Bullets.BulletM4A1 -= Monto;
    }
    public void RemoveBulletM4SHOOTGUN(int Monto)
    {
        Bullets.BulletM4Shootgun -= Monto;
    }
    public void RemoveBulletAK74M(int Monto)
    {
        Bullets.BulletAK74M -= Monto;
    }
    public void RemoveBulletCalico(int Monto)
    {
        Bullets.BulletCalico -= Monto;
    }

    #endregion

}
