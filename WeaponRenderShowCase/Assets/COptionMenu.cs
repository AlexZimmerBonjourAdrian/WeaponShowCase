using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COptionMenu : MonoBehaviour
{

    public static bool GameIsPaused = false;

    public GameObject optionMenuUi;

    public Slider Slider;

    public DataOption optionData;
    // Start is called before the first frame update
    

    
    public void Resume()
    {
        optionMenuUi.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Paused()
    {

        optionMenuUi.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Save()
    {

        Slider = optionMenuUi.GetComponentInChildren<Slider>();
        optionData.sensX = Slider.value;
        optionData.sensY = Slider.value;
        optionMenuUi.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        GameIsPaused = false;

    }

}
