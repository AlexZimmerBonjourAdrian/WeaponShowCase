using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CAmmoCotroller : MonoBehaviour
{
    public Text MagSize;
    public Text ExtraAmmo;
    public int ammo;
    public int extraAmmo;

    public static CAmmoCotroller Inst;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAmmunition();
    }

    public void UpdateAmmunition()
    {
        MagSize.text = ammo.ToString();
        ExtraAmmo.text = extraAmmo.ToString();
    }

    public void SetAmmo(int Ammo)
    {
        ammo = Ammo;
    }
    public void SetExtraAmmo(int ExtraAmmo)
    {
        extraAmmo = ExtraAmmo;
    }

}
