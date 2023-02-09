using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CControllerPlayerInMaps : MonoBehaviour
{
    

    [Header("Exterior an open spaces")]

    [Header("open spaces Values")]
    [SerializeField] private float maxSlideTime_Exterior;
    [SerializeField] private float slideForce_Exterior;
    [SerializeField] public float slideYScale_Exterior;
    [SerializeField] private float maxSlopeAngle_Exterior;

    [Header("Movements value to open spaces")]
    [SerializeField] public float walkSpeed_Exterior;
    [SerializeField] public float sprintSpeed_Exterior;
    [SerializeField] public float speedIncreaseMultiplier;
    [SerializeField] public float slopeIncreaseMultiplier;
    [SerializeField] public float groundDrag;
   
    [Header("Jumping")]
    [SerializeField] public float jumpForce_Exterior;
    [SerializeField] public float jumpCooldown_Exterior;
    [SerializeField] public float airMultiplier_Exterior;

    [Header("Crouching")]
    [SerializeField] public float crouchSpeed_Exterior;
    [SerializeField] public float crounchYScale_Exterior;
    [SerializeField] private float startYScale_Crouch_Exterior;

    
    
    [Header("Values Interior on Streets")]
        [Header("Slide interior Values")]
    [Header("open spaces Values")]
    [SerializeField] private float maxSlideTime_Interior;
    [SerializeField] private float slideForce_Interior;
    [SerializeField] public float slideYScale_Interior;
    [SerializeField] private float maxSlopeAngle_Interior;

    [Header("Movements value to open spaces")]
    [SerializeField] public float walkSpeed_Interior;
    [SerializeField] public float sprintSpeed_Interior;
    [SerializeField] public float speedIncreaseMultiplier_Interior;
    [SerializeField] public float slopeIncreaseMultiplier_Interior;
    [SerializeField] public float groundDrag_Interior;

    [Header("Jumping")]
    [SerializeField] public float jumpForce_Interior;
    [SerializeField] public float jumpCooldown_Interior;
    [SerializeField] public float airMultiplier_Interior;

    [Header("Crouching")]
    [SerializeField] public float crouchSpeed_Interior;
    [SerializeField] public float crounchYScale_Interior;
    [SerializeField] private float startYScale_Crouch_Interior;






    private CPlayerMovementAdvance PlayerMovement;
    private CSlide Slider;
    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement = GetComponent<CPlayerMovementAdvance>();
        Slider = GetComponent<CSlide>();
        if (CLevelManager.Inst.GetLevelIsInterior() == false)
        {
            //Exterior
            // Slider Exterior
            Slider.SetMaxSlideTime(maxSlideTime_Exterior);
            Slider.SetSlideForce(slideForce_Exterior);
            Slider.SetSlideYScale(slideYScale_Exterior);
            PlayerMovement.SetMaxSlopeAngle(maxSlopeAngle_Exterior);


            // PlayerMovement Exterior
            PlayerMovement.SetWalkSpeed(walkSpeed_Exterior);
            PlayerMovement.SetSprintSpeed (sprintSpeed_Exterior);
             PlayerMovement.SetSprintSpeed( speedIncreaseMultiplier);
            PlayerMovement.SetSlopeIncreaseMultiplier(slopeIncreaseMultiplier);
             PlayerMovement.SetGroundDrag( groundDrag);
            PlayerMovement.SetjumpForce(jumpForce_Exterior);
            PlayerMovement.SetjumpColddown(jumpCooldown_Exterior);
            PlayerMovement.SetairMultiplier(airMultiplier_Exterior);
            PlayerMovement.SetCrounchSpeed(crouchSpeed_Exterior);
            PlayerMovement.SetCrouchYScale(crouchSpeed_Exterior);
           
        }
        else
        {
            //Interior
            // Slider Interior
            Slider.SetMaxSlideTime(maxSlideTime_Interior);
            Slider.SetSlideForce(slideForce_Interior);
            Slider.SetSlideYScale(slideYScale_Interior);
            PlayerMovement.SetMaxSlopeAngle(maxSlopeAngle_Interior);


            //PlayerMovement Interior
            PlayerMovement.SetWalkSpeed(walkSpeed_Interior);
            PlayerMovement.SetSprintSpeed(sprintSpeed_Interior);
            PlayerMovement.SetSprintSpeed(speedIncreaseMultiplier_Interior);
            PlayerMovement.SetSlopeIncreaseMultiplier(slopeIncreaseMultiplier_Interior);
            PlayerMovement.SetGroundDrag(groundDrag_Interior);
            PlayerMovement.SetjumpForce(jumpForce_Interior);
            PlayerMovement.SetjumpColddown(jumpCooldown_Interior);
            PlayerMovement.SetairMultiplier(airMultiplier_Interior);
            PlayerMovement.SetCrounchSpeed(crouchSpeed_Interior);
            PlayerMovement.SetCrouchYScale(crouchSpeed_Interior);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
