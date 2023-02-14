using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class CMuzzleController : MonoBehaviour
{
    private float BulletShellIntensity = 0;
    private float MuzzleShellIntensity = 0;

    [SerializeField]
    private VisualEffect ShellEjection;
    [SerializeField]
    private VisualEffect MuzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        //ShellEjection = GameObject.Find("bulletEjection Variant").GetComponent<VisualEffect>();
        //MuzzleFlash = GameObject.Find("MuzzleFlash").GetComponent<VisualEffect>();
    }
    private void Update()
    {
        ShellEjection.SetFloat("RateBulletShell", BulletShellIntensity);
        MuzzleFlash.SetFloat("RateMuzzle", MuzzleShellIntensity);
    }
    // Update is called once per frame

    public void PlayeVisualEffect()
    {

        ShellEjection.Play();
        MuzzleFlash.Play();
    }
    public void StopVisualEffect()
    {
        ShellEjection.Stop();
        MuzzleFlash.Stop();
    }

    public void SetRates(int rates)
    {
        BulletShellIntensity = rates;
        MuzzleShellIntensity = rates;
    }
}


