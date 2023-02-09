using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CManageResources: MonoBehaviour
{
    public static CManageResources Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = new GameObject("ResourcesManager");
                return obj.AddComponent<CManageResources>();
            }
            return _inst;
        }
    }
    private static CManageResources _inst;

    public bool isSpectre = false;
    public TrailRenderer SpectreTrail;
    public TrailRenderer HotBulletTrail;
    public GameObject BulletHole;
    private List<TrailRenderer> TrailRenderersList = new List<TrailRenderer>();
    [SerializeField]
    private GameObject MP5k_PickUp;
    [SerializeField]
    private GameObject M4A1_PickUp;
    [SerializeField]
    private GameObject AK47M_PickUp;
    [SerializeField]
    private GameObject Calico_PickUp;
    [SerializeField]
    private GameObject m4ShotGun_PickUp;



    //Todo:Create Bullet Holder
    //private List<GameObject> BulletHolder = new List<GameObject>();

    private float BulletSpeed = 100;

    // Start is called before the first frame update

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

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TrailRenderersList != null)
        {
            for (int i = TrailRenderersList.Count - 1; i >= 0; i--)
            {
                if (TrailRenderersList[i] == null)
                    TrailRenderersList.RemoveAt(i);
            }
        }
    }

 

    public void TimeToDeath(TrailRenderer trail)
    {
        Destroy(trail.gameObject, trail.time);
    }

    //TODO:Bullet Hole Controller 
   
   

    public TrailRenderer getBulletTrailSpectre()
    {
        return SpectreTrail;
    }
    public TrailRenderer getBulletTrail()
    {
        return HotBulletTrail;
    }

    public GameObject getBulletHoleWall()
    {
        return BulletHole;
    }

    public GameObject getBulletHoleBlood()
    {
        return null;
    }

    public GameObject getBulletHoleFloor()
    {
        return null;
    }

    public GameObject getBulletHoleMetalCrash()
    {
        return null;
    }

    public GameObject getBulletHolePlastic()
    {
        return null;
    }

    public GameObject getBulletHoleCristal()
    {
        return null;
    }
    public GameObject getBulletHoleQuetblar()
    {
        return null;
    }
    public GameObject getM4A1PicKUP()
    {
        return M4A1_PickUp;
    }
    public GameObject getmp5kPicKUP()
    {
        return MP5k_PickUp;
    }
    public GameObject getCalicoPicKUP()
    {
        return Calico_PickUp;
    }
    public GameObject getAK74MicKUP()
    {
        return AK47M_PickUp;
    }
    public GameObject getM4shotgunPicKUP()
    {
        return m4ShotGun_PickUp;
    }
}

