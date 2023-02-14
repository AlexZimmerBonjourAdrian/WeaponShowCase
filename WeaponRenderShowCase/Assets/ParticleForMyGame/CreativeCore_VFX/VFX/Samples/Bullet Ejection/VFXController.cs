using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class VFXController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private VisualEffect visualEffect;


    [SerializeField,Range(0, 30)]
    private float BulletShellIntensity = 0;

    //[SerializeField]
    //private Gradient Gravity = null;

    void Start()
    {
        visualEffect = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        visualEffect.SetFloat("RateBulletShell", BulletShellIntensity);
    
       if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            visualEffect.Play();
        }
       
    }
}
