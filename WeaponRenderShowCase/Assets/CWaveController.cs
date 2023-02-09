using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CWaveController : MonoBehaviour
{
    public static bool IsEndWave = false;
    //public 
    [SerializeField] public float TimeText; 
    public GameObject _waveUi;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    public void WaveEnd()
    {
        _waveUi.SetActive(true);
    }

    public void NewWave()
    {
        _waveUi.SetActive(true);
    }

    public void Startwave()
    {
        _waveUi.SetActive(true);
    }

    public void DesactiveMenu()
    {
        _waveUi.SetActive(false);
    }

    //IEnumerator DesactiveMenu()
    //{

    //    yield return new WaitForSeconds(TimeText);

    //}

}
