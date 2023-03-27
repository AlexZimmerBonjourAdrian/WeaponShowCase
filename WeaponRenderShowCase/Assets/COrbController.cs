using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COrbController : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator Anim;
    float input = 0;
   private void Start()
    {
        Anim = GetComponent<Animator>();
    }

    public void Update()
    {
        //#region BlendTree
        //if (Input.GetKey(KeyCode.Q))
        //{
        //    input -= 0.02f;
        //        Anim.SetFloat("Blend", input);

        //}
        //if (Input.GetKey(KeyCode.W))
        //{
        //    input += 0.02f;
        //    Anim.SetFloat("Blend", input);


        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    input = 0f;
        //    Anim.SetFloat("Blend", input);

        //}
        //#endregion
        #region ControllerAnimation
        if (Input.GetKey(KeyCode.Q))
        {

            Anim.SetTrigger("Normal");

        }
        if (Input.GetKey(KeyCode.W))
        {


            Anim.SetTrigger("Big");


        }
        if (Input.GetKeyDown(KeyCode.E))
        {

            Anim.SetTrigger("Small");

        }

        #endregion

    }
}