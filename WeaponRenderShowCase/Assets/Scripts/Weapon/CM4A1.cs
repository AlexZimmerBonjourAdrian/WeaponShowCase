using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CM4A1 : CArmed
{
    [Header("Gun Setting")]
    [SerializeField] bool _canShoot;

    public Sprite[] flashes;

    public Vector3 normalLocalPosition;
    public Vector3 aimmingLocalPosition;

    public float aimSmoothing = 10f;
    public enum GunState
    {
        Setup_Left = 0,
        Setup_Right = 1,
        Idle_Right = 2,
        Idle_Left = 3,
        Shoot_Left = 6,
        Shoot_Right = 7,
        AutoShoot_Left = 8,
        AutoShoot_Right = 9,
        Reload_Left = 10,
        Reload_Right = 11,
        ReloadNotAmmo_Left = 12,
        ReloadNotAmmo_Right = 13,
        //Estos dejarlos para lo ultimo
        Desequip_Left = 14,
        Desequip_Right = 15,
        Drop = 16,
    }
    public int state;
    [Header("Mouse Setting")]
    public float mouseSensitivity = 10;
    Vector2 _currentRotation;
    public float weaponSwayAmount = 10f;
    public bool IsLeft;
    //public bool randomizeRecoil;
    //public Vector2 randomRecoilContraints;
    //public Vector2[] recoilPattern;

    private Animator _anim;

    [SerializeField] private Transform _spawnShooter;
    private Vector3 _Shootposition;
    private GameObject bulletHolePreb;
    [Header("DEBUG!!!!----------")]
    public List<string> ListAnimationDebug;
    [SerializeField] public string[] ListAniamtionArrayDebug;
    public uint ValueIndex = 0;
    //[SerializeField] private CRecoil recoil_Script;

    float timeSinceLastShot = 0f;

    float fireRate = 0.08f;
    public bool isAuto;

    private CMuzzleController VFXMP5K;

    private Transform SpawmBulletTransform;
    [SerializeField]
    private GameObject bulletTrailPreb;
    //[SerializeField]
    //private bool AddBulletSpread = true;
    [SerializeField]
    private Vector3 BulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField]
    private ParticleSystem ShootingSystem;
    [SerializeField]
    private ParticleSystem ImpactParticleSystem;

    [SerializeField]
    private TrailRenderer BulletTrail;
    [SerializeField]
    private float BulletSpeed = 100;
    [SerializeField]
    private float ShootDelay = 0.5f;
    [SerializeField]
    protected LayerMask Mask;


    [SerializeField] private AudioClip SFXSetupLeft;
    [SerializeField] private AudioClip SFXSetupRight;
    [SerializeField] private AudioClip SFXAutoShootLeft;
    [SerializeField] private AudioClip SFXAutoShootRight;
    [SerializeField] private AudioClip SFXReloadNOTAMMORight;
    [SerializeField] private AudioClip SFXReloadNOTAMMOLeft;
    [SerializeField] private AudioClip SFXReloadRight;
    [SerializeField] private AudioClip SFXReloadLeft;
    [SerializeField] private AudioClip SFXSingleFireRight;
    [SerializeField] private AudioClip SFXSingleFireLeft;
    [SerializeField] private AudioClip SFXDesequip;
    [SerializeField] private AudioClip SFXDropLeft;
    [SerializeField] private AudioClip SFXIdleLeft;
    [SerializeField] private AudioClip SFXIdleRight;
    public AudioSource audioSource;

    [SerializeField]
    private GameObject MuzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        _canShoot = true;
        transform.localPosition = new Vector3(0.0005f, -0.0201f, 0.0057f);
        _anim = GetComponent<Animator>();
        recoil_Script = GameObject.Find("CameraRecoil").GetComponent<CRecoil>();
        bulletTrailPreb = GameObject.Find("WeaponSpawnPoint");
        bulletHolePreb = CManageResources.Inst.getBulletHoleStone();
        BulletTrail = GameObject.Find("HotTrail").GetComponent<TrailRenderer>();
        playerCamera = GameObject.Find("PlayerCam").transform;
        NameAnimation = GameObject.Find("NameAnimation").GetComponent<Text>();
        marketUI = GameObject.Find("UICrosshair");
        setState((int)GunState.Setup_Left);
        LoadInfo();
        isAuto = true;
        audioSource = GetComponent<AudioSource>();

        VFXMP5K = GetComponent<CMuzzleController>();
        VFXMP5K.SetRates(0);
        VFXMP5K.PlayeVisualEffect();
        IsLeft = false;


    }

    // Update is called once per frame
    //void FixedUpdate()
    //{
    ////    if (Input.GetKey(KeyCode.Mouse0) && _canShoot && ammo_in_mag > 0)
    ////    {
    ////        for (int i = 0; i <= 2; i++)
    ////        {
    ////            _canShoot = false;
    ////            ammo_in_mag--;
    ////             recoil_Script.RecoilFire();
    ////            StartCoroutine(ShootGun());
    ////        }
    ////    }

    ////    else if (Input.GetKeyUp(KeyCode.R) && ammo_in_mag < mag_size && extra_ammo > 0)
    ////    {
    ////        //isReload = true;
    ////        ////_anim.SetBool("CanReload", isReload);
    ////        int amountNeeded = mag_size - ammo_in_mag;
    ////        if (amountNeeded >= extra_ammo)
    ////        {
    ////            ammo_in_mag += extra_ammo;
    ////            extra_ammo -= amountNeeded;
    ////        }
    ////        else
    ////        {
    ////            ammo_in_mag = mag_size;
    ////            extra_ammo -= amountNeeded;
    ////        }
    ////    }
    //}

    void Update()
    {
        AnimatorStateInfo currentState = _anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Estado actual" + state);
        float TimeFinish = currentState.normalizedTime;
        switch (state)
        {
            case (int)GunState.Idle_Left:
                AnimationNameFunction("m4a1-Idle-Right");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXIdleLeft;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Idle-Right"))
                {
                    _anim.Play("m4a1-Idle-Right");
                }
                if (Input.GetKeyDown(KeyCode.R))
                {


                    if (extra_ammo > 0 && ammo_in_mag >= 1 && ammo_in_mag != mag_size && IsLeft == false /*&& ammo_in_mag < 1*/ /*&& isReload==true*/)
                    {
                        //iniciar la animacion de recarga que tiene las valas normales
                        audioSource.Stop();
                        setState((int)GunState.Reload_Right);
                        IsLeft = true;

                    }
                    else if (extra_ammo > 0 && (ammo_in_mag < 1) && ammo_in_mag != mag_size && IsLeft == false /*&& isReload == true*/)
                    {
                        //iniciar la animacion de recarga que no tiene balas
                        audioSource.Stop();
                        setState((int)GunState.ReloadNotAmmo_Right);
                        IsLeft = true;
                    }

                    else if (extra_ammo > 0 && ammo_in_mag >= 1 && ammo_in_mag != mag_size && IsLeft == true /*&& ammo_in_mag < 1*/ /*&& isReload==true*/)
                    {
                        //iniciar la animacion de recarga que tiene las valas normales
                        audioSource.Stop();
                        setState((int)GunState.Reload_Left);
                        IsLeft = false;

                    }
                    else if (extra_ammo > 0 && (ammo_in_mag < 1) && ammo_in_mag != mag_size && IsLeft == true  /*&& isReload == true*/)
                    {
                        //iniciar la animacion de recarga que no tiene balas
                        setState((int)GunState.ReloadNotAmmo_Left);
                        IsLeft = false;
                    }
                }
                if (Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate))
                {
                    //if (!isAuto)
                    //{
                    //    setState((int)GunState.Shoot);
                    //}
                    if (isAuto)
                    {
                        audioSource.Stop();
                        setState((int)GunState.AutoShoot_Left);
                    }
                }
                else
                {
                    timeSinceLastShot += Time.deltaTime;
                }
                break;
            case (int)GunState.Idle_Right:
                AnimationNameFunction("m4a1-Idle-Right");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXIdleRight;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Idle-Right"))
                {
                    _anim.Play("m4a1-Idle-Right");
                }
                if (extra_ammo > 0 && ammo_in_mag >= 1 && ammo_in_mag < mag_size && IsLeft == false /*&& ammo_in_mag < 1*/ /*&& isReload==true*/)
                {
                    //iniciar la animacion de recarga que tiene las valas normales
                    audioSource.Stop();
                    IsLeft = true;
                    setState((int)GunState.Reload_Right);
                 

                }
                else if (extra_ammo > 0 && (ammo_in_mag < 1) && ammo_in_mag < mag_size && IsLeft == false /*&& isReload == true*/)
                {
                    //iniciar la animacion de recarga que no tiene balas
                    audioSource.Stop();
                    IsLeft = true;
                    setState((int)GunState.ReloadNotAmmo_Right);
                    
                }

                 if (extra_ammo > 0 && ammo_in_mag >= 1 && ammo_in_mag < mag_size && IsLeft == true /*&& ammo_in_mag < 1*/ /*&& isReload==true*/)
                {
                    //iniciar la animacion de recarga que tiene las valas normales
                    audioSource.Stop();
                    IsLeft = false;
                    setState((int)GunState.Reload_Left);
                    

                }
                else if (extra_ammo > 0 && (ammo_in_mag < 1) && ammo_in_mag < mag_size && IsLeft == true  /*&& isReload == true*/)
                {
                    //iniciar la animacion de recarga que no tiene balas
                    audioSource.Stop();
                    IsLeft = false;
                    setState((int)GunState.ReloadNotAmmo_Left);
                    
                }

                if (Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate))
                {
                    if (!isAuto)
                    {
                        setState((int)GunState.Shoot_Left);
                    }
                    if (isAuto)
                    {
                        audioSource.Stop();
                        setState((int)GunState.AutoShoot_Left);
                    }
                }

                else if(Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate) && IsLeft == false)
                {
                    if (!isAuto)
                    {
                        setState((int)GunState.Shoot_Right);
                    }
                    if (isAuto)
                    {
                        audioSource.Stop();
                        setState((int)GunState.AutoShoot_Right);
                    }
                }
                else
                {
                    timeSinceLastShot += Time.deltaTime;
                }

                break;

            case (int)GunState.Shoot_Right:
                AnimationNameFunction("m4a1-Fire");
                VFXMP5K.SetRates(1);
                StartCoroutine(FireMuzzle());
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSingleFireRight;
                    audioSource.Play();
                }
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                Debug.Log("Estado actual" + state);
                TimeFinish = currentState.normalizedTime;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Fire"))
                {
                    _anim.Play("m4a1-Fire");
                    //ammo_in_mag--;
                    Fire();
                    timeSinceLastShot = 0f;
                }

                if (TimeFinish > .45f)
                {
                    audioSource.Stop();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle_Right);
                }
                break;


            case (int)GunState.Shoot_Left:

                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                Debug.Log("Estado actual" + state);
                TimeFinish = currentState.normalizedTime;
                AnimationNameFunction("Shoot_Left");
                StartCoroutine(FireMuzzle());
                StartCoroutine(FireMuzzle());
                VFXMP5K.SetRates(1);

                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSingleFireLeft;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Fire"))
                {
                    _anim.Play("m4a1-Fire");
                    //ammo_in_mag--;
                    Fire();
                    timeSinceLastShot = 0f;
                }

                if (TimeFinish > .45f)
                {
                    audioSource.Stop();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle_Left);
                }
                break;

            case (int)GunState.AutoShoot_Right:
                Debug.LogWarning("timeSinceLastShot es" + timeSinceLastShot);
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                StartCoroutine(FireMuzzle());

                AnimationNameFunction("m4a1-Shoot-Auto");
                if (!audioSource.isPlaying && (ammo_in_mag > 0))
                {
                    audioSource.clip =SFXAutoShootRight;
                    audioSource.Play();
                    
                }
                if ((ammo_in_mag > 0) && (timeSinceLastShot >= fireRate))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Shoot-Auto"))
                    {
                        _anim.Play("m4a1-Shoot-Auto");
                    }
                    if (TimeFinish > .8f)
                    {
                        _anim.Play("m4a1-Shoot-Auto");
                    }
                    Debug.Log("Entra");
                    //_anim.Play("mp5k-shoot-automatic");
                    StartCoroutine(FireMuzzle());
                    Fire();
                    VFXMP5K.SetRates(ammo_in_mag);
                    timeSinceLastShot = 0f;
                    //.RecoilFire();
                    //ammo_in_mag--;
                }
                if ((ammo_in_mag <= 0))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Idle-Right"))
                    {
                        _anim.Play("m4a1-Idle-Right");
                    }
                }
                timeSinceLastShot += Time.deltaTime;

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    audioSource.Stop();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle_Right);
                }
                break;

            case (int)GunState.AutoShoot_Left:
                Debug.LogWarning("timeSinceLastShot es" + timeSinceLastShot);
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;

                AnimationNameFunction("m4a1-Shoot-Auto");
                StartCoroutine(FireMuzzle());
                if (!audioSource.isPlaying && (ammo_in_mag > 0))
                {
                    audioSource.clip = SFXAutoShootLeft;
                    audioSource.Play();
                    
                }
                Debug.LogWarning("timeSinceLastShot es" + timeSinceLastShot);
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;



                if ((ammo_in_mag > 0) && (timeSinceLastShot >= fireRate) )
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Shoot-Auto"))
                    {
                        _anim.Play("m4a1-Shoot-Auto");
                    }
                    if (TimeFinish > .8f)
                    {
                        _anim.Play("m4a1-Shoot-Auto");
                    }
                    Debug.Log("Entra");
                    //_anim.Play("mp5k-shoot-automatic");
                    StartCoroutine(FireMuzzle());
                    Fire();
                    VFXMP5K.SetRates(ammo_in_mag);
                    timeSinceLastShot = 0f;
                    //.RecoilFire();
                    //ammo_in_mag--;
                }
                if ((ammo_in_mag <= 0))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Idle-Right"))
                    {
                        _anim.Play("m4a1-Idle-Right");
                    }
                }
                timeSinceLastShot += Time.deltaTime;

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    audioSource.Stop();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle_Left);
                }
                break;

            case (int)GunState.Desequip_Left:
                AnimationNameFunction("m4a1-Shoot-Auto");
                break;
            case (int)GunState.Desequip_Right:
                AnimationNameFunction("m4a1-Shoot-Auto");
                break;

            case (int)GunState.Reload_Left:
                AnimationNameFunction("m4a1-reload-normal-left");

                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReloadLeft;
                    audioSource.Play();
                }
                //isReload = false;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-reload-normal-left"))
                {
                    _anim.Play("m4a1-reload-normal-left");
                }
                int amoutNeeded = mag_size - ammo_in_mag;
                if (!(extra_ammo <= 0))
                {
                    //_anim.Play("m4Shoothun-Reload-Normal");
                    if (extra_ammo >= ammo_in_mag)
                    {
                        if (amoutNeeded >= extra_ammo)
                        {
                            ammo_in_mag += extra_ammo;
                            extra_ammo -= amoutNeeded;
                        }
                        else
                        {
                            ammo_in_mag = mag_size;
                            extra_ammo -= amoutNeeded;
                        }
                    }
                }

                if (TimeFinish > .98f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle_Left);
                }

                break;
            case (int)GunState.Reload_Right:
                AnimationNameFunction("m4a1-Reload-normal-right");
                //isReload = false;
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReloadRight;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Reload-normal-right"))
                {
                    _anim.Play("m4a1-Reload-normal-right");
                }
                amoutNeeded = mag_size - ammo_in_mag;
                if (!(extra_ammo <= 0))
                {
                    //_anim.Play("m4Shoothun-Reload-Normal");
                    if (extra_ammo >= ammo_in_mag)
                    {
                        if (amoutNeeded >= extra_ammo)
                        {
                            ammo_in_mag += extra_ammo;
                            extra_ammo -= amoutNeeded;
                        }
                        else
                        {
                            ammo_in_mag = mag_size;
                            extra_ammo -= amoutNeeded;
                        }
                    }
                }

                if (TimeFinish > .98f)
                {
                    //setState((int)GunState.Idle_Right);
                    audioSource.Stop();
                    setState((int)GunState.Idle_Right);
                }

                break;

            case (int)GunState.ReloadNotAmmo_Left:
                AnimationNameFunction("m4a1-reload-Left-NotAmmo");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReloadNOTAMMOLeft;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-reload-Left-NotAmmo"))
                {
                    _anim.Play("m4a1-reload-Left-NotAmmo");
                    //_anim.Play("m4Shoothun-Reload-Normal");
                    amoutNeeded = mag_size - ammo_in_mag;
                    if (extra_ammo >= ammo_in_mag)
                    {
                        if (amoutNeeded >= extra_ammo)
                        {
                            ammo_in_mag += extra_ammo;
                            extra_ammo -= amoutNeeded;
                        }
                        else
                        {
                            ammo_in_mag = mag_size;
                            extra_ammo -= amoutNeeded;
                        }
                    }
                }
                if (TimeFinish > .98f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle_Left);
                }
                break;
            case (int)GunState.ReloadNotAmmo_Right:

                AnimationNameFunction("m4a1-reload-right-NotAmmo");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReloadNOTAMMORight;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-reload-right-NotAmmo"))
                {
                    _anim.Play("m4a1-reload-right-NotAmmo");
                    //_anim.Play("m4Shoothun-Reload-Normal");
                    amoutNeeded = mag_size - ammo_in_mag;
                    if (extra_ammo >= ammo_in_mag)
                    {
                        if (amoutNeeded >= extra_ammo)
                        {
                            ammo_in_mag += extra_ammo;
                            extra_ammo -= amoutNeeded;
                        }
                        else
                        {
                            ammo_in_mag = mag_size;
                            extra_ammo -= amoutNeeded;
                        }
                    }
                }
                if (TimeFinish > .98f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle_Right);
                }
                break;

            case (int)GunState.Setup_Left:
                AnimationNameFunction("m4a1-Setup");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSetupLeft;
                    audioSource.Play();
                }
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;

                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Setup"))
                {
                    _anim.Play("m4a1-Setup");

                }
                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle_Left);
                }

                break;

            case (int)GunState.Setup_Right:
                AnimationNameFunction("m4a1-Setup");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSetupRight;
                    audioSource.Play();
                }
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;

                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Setup"))
                {
                    _anim.Play("m4a1-Setup");

                }
                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle_Right);
                }

                break;

            case (int)GunState.Drop:
                AnimationNameFunction("m4a1-Drop");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXDropLeft;
                    audioSource.Play();
                }
                float timeToDeath = currentState.normalizedTime;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4a1-Drop"))
                {
                    _anim.Play("m4a1-Drop");
                }
                if (timeToDeath > .9f)
                {
                    audioSource.Stop();
                    Destroy(gameObject);
                }
                break;

            default:
                print("No Entra a ningun lado");
                break;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            isAuto = !isAuto;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            setState((int)GunState.Drop);
        }

        //CODIGO DE DEBUG PARA PROBAR ANIMACIONES
        //if (Input.GetKeyDown(KeyCode.Q))
        //{

        //    //if (ValueIndex <= ListAnimationDebug.Count)
        //    ////{ 
        //    //    ValueIndex+=1;
        //    //   _anim.Play(ListAnimationDebug[(int)ValueIndex].ToString());
        //    //}
        //    ValueIndex += 1;
        //    _anim.Play(ListAniamtionArrayDebug[ValueIndex]);
        ////}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //    if (ValueIndex <= ListAnimationDebug.Count)
        //    //    {
        //    ValueIndex -= 1;
        //    _anim.Play(ListAniamtionArrayDebug[ValueIndex]);
        //}
    }

    IEnumerator ShootGun()
    {
        //Shoot();
        RayCastForEne();
        yield return new WaitForSeconds(fire_rate);
        _canShoot = true;
    }

    public override void Shoot()
    {
        base.Shoot();
    }

    public override void Add_ammo(DataPickUp PickUp)
    {
        base.Add_ammo(PickUp);
    }

    public override void Reload()
    {
        base.Reload();
    }
    public void Fire()
    {

        Vector3 direction = GetDirection();

        //if (Physics.Raycast(bulletTrailPreb.transform.position, direction, out RaycastHit hit, float.MaxValue, Mask))
        //{
        //    TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);

        //    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

        //    LastShootTime = Time.time;
        //}
        // this has been updated to fix a commonly reported problem that you cannot fire if you would not hit anything
        //else
        //{
        TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);

        StartCoroutine(SpawnTrail(trail, bulletTrailPreb.transform.forward * 1000, Vector3.zero, false));

        LastShootTime = Time.time;
        //}


        ammo_in_mag--;
        BulletHole();
        RayCastForEne();
        recoil_Script.RecoilFire();

    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
       
            direction += new Vector3(
                Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
                Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
                Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)

                );
       
        return direction;
    }
    private IEnumerator SpawnTrail(TrailRenderer Trail, Vector3 HitPoint, Vector3 HitNormal, bool MadeImpact)
    {
        // This has been updated from the video implementation to fix a commonly raised issue about the bullet trails
        // moving slowly when hitting something close, and not
        Vector3 startPosition = Trail.transform.position;
        float distance = Vector3.Distance(Trail.transform.position, HitPoint);
        float remainingDistance = distance;

        while (remainingDistance > 0)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, HitPoint, 1 - (remainingDistance / distance));

            remainingDistance -= BulletSpeed * Time.deltaTime;

            yield return null;
        }
        //Animator.SetBool("IsShooting", false);
        Trail.transform.position = HitPoint;
        if (MadeImpact)
        {
            Instantiate(ImpactParticleSystem, HitPoint, Quaternion.LookRotation(HitNormal));
        }

        Destroy(Trail.gameObject, Trail.time);
    }



    public void setState(int newState)
    {
        state = newState;
    }
    public override void LoadInfo()
    {
        base.LoadInfo();
    }

    public override string GetWeaponName()
    {
        return base.GetWeaponName();
    }

    public override string GetWeaponType()
    {
        return GetWeaponType();
    }

    public override int GetWeaponDamage()
    {
        return GetWeaponDamage();
    }

    public override int GetAmmo_in_Mag()
    {
        return GetAmmo_in_Mag();
    }

    public override bool GetIsShooting()
    {
        return GetIsShooting();
    }

    public override bool GetIsReload()
    {
        return GetIsReload();
    }

    public override bool GetIsCrossing()
    {
        return GetIsCrossing();
    }
    public override void Equip()
    {
        base.Equip();
        audioSource.Stop();
        setState((int)GunState.Setup_Left);
    }
    public override void Desequip()
    {
        base.Desequip();
        audioSource.Stop();
        setState((int)GunState.Desequip_Left);
    }

    public override void Drop()
    {
        base.Drop();
        audioSource.Stop();
        setState((int)GunState.Drop);

    }
    void RayCastForEne()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, Mathf.Infinity, maskEnemy))
        {
            Debug.Log("Hit an Enemy");
            marketUI.GetComponent<CHitmarket>().Hit();
            hit.collider.GetComponent<Mafioso>().TakeDamage(damage);
        }
    }
    //private void OnDrawGizmos()
    //{

    //    Gizmos.color = new Color32(255, 120, 20, 170);
    //    Gizmos.DrawRay(_spawnShooter.position, _spawnShooter.TransformDirection(Vector3.forward));
    //}
    public override void AnimationNameFunction(string nameAnimation)
    {
        base.AnimationNameFunction(nameAnimation);
    }
    IEnumerator FireMuzzle()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        MuzzleFlash.SetActive(false);
    }
    public void BulletHole()
    {
        //Ray rayOriginal = Camera.main.ScreenPointToRay(Input.mousePosition);


        RaycastHit hitInfo;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hitInfo, Mathf.Infinity))
        {
            if (hitInfo.collider.tag == "wall")
            {
                //Inantiate bullet hole
                Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                Vector3 direction = hitInfo.point - transform.position;
                //transform.rotation = Quaternion.LookRotation(direction);
            }

        }
    }
}
