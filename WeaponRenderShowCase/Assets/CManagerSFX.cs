using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CManagerSFX : MonoBehaviour
{

    public static CManagerSFX Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("ManagerSFX");
                return obj.AddComponent<CManagerSFX>();
            }
            return _inst;
        }
    }
    private static CManagerSFX _inst;
    private List<AudioSource> audioSourceList = new List<AudioSource>();
    private List<AudioClip> audioclipList = new List<AudioClip>();

    public void Awake()
    {
        if (_inst != null && _inst != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        _inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }



    public void AddAudioClip(AudioClip audioclip)
    {
        audioclipList.Add(audioclip);
    }

    public void AddAudioSource(AudioSource audioSource)
    {
        audioSourceList.Add(audioSource);

    }

    // Update is called once per frame
    void Update()
    {
        if(audioSourceList != null)
        {
            for (int i = audioSourceList.Count - 1; i >= 0; i--)
            {
                Debug.Log(audioSourceList[i]);
            }
        }
        for (int i = audioSourceList.Count - 1; i >= 0; i--)
        {
            if (audioSourceList[i] == null)
                audioSourceList.RemoveAt(i);
        }

        for (int i = audioclipList.Count - 1; i >= 0; i--)
        {
            if (audioclipList[i] == null)
                audioclipList.RemoveAt(i);
        }

    }

    public AudioSource findAudioSource(AudioSource Find)
    {
        return audioSourceList.Find(x => x.gameObject == Find);
    }

    public AudioClip findClipAudio(AudioClip Find)
    {
        return audioclipList.Find(x => x == Find);
    }
}
