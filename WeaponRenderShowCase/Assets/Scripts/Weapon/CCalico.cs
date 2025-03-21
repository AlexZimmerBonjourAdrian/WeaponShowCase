using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CCalico : CArmed
{
    [Header("Gun Setting")]

    [SerializeField] bool _canShoot;

    public enum GunState
    {
        Idle = 0,
        Setup = 1,
        Shoot = 2,
        AutoShoot = 3,
        Reload = 4,
        ReloadNotAmmo = 5,
        Desequip = 6,
        Drop = 7,
    }
    public Vector3 normalLocapPoition;
    public Vector3 aimIngLocalPosition;

    public float aimSmoothing = 10f;
    private CMuzzleController VFXMP5K;
    [Header("Mouse setting")]
    public float mouseSensitivity = 10;
    Vector2 _currentRotation;
    public float weaponSwayAmount = 10f;
    [SerializeField] int _ammoInReserve;
    //Weapon Recoil
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    //you only need to assign this if randomizeRecoil if off
    public Vector2[] recoilPattern;

    [SerializeField] private Transform _Shootposition;
    [SerializeField] private float range = 30;
    //[SerializeField] private CRecoil recoil_Script;
    public int state;
    public bool isAuto;

    //Weapon Recoil
    private Animator _anim;
    [Header("DEBUG!!!!----------")]
    public List<string> ListAnimationDebug;
    [SerializeField] public string[] ListAniamtionArrayDebug;
    public uint ValueIndex = 0;
   
    float timeSinceLastShot = 0f;

    float fireRate = 0.08f;
    private GameObject bulletHolePreb;
    private Transform SpawmBulletTransform;
    [SerializeField]
    private GameObject bulletTrailPreb;
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

    public AudioSource audioSource;
    [SerializeField] private AudioClip SFXSetup;
    [SerializeField] private AudioClip SFXAutoShoot;
    [SerializeField] private AudioClip SFXReloadNOTAMMO;
    [SerializeField] private AudioClip SFXReload;
    [SerializeField] private AudioClip SFXSingleFire;
    [SerializeField] private AudioClip SFXDesequip;
    [SerializeField] private AudioClip SFXDrop;
    [SerializeField] private AudioClip SFXIdle;


    [SerializeField]
    private GameObject MuzzleFlash;
    // Start is called before the first frame update
    void Start()
    {
        _canShoot = true;
        transform.localPosition = new Vector3(0.0021f, -0.0222f, -0.0002f);
        LoadInfo();
        _Shootposition = GetComponentInChildren<Transform>();
        _anim = GetComponent<Animator>();
        recoil_Script = GameObject.Find("CameraRecoil").GetComponent<CRecoil>();
        NameAnimation = GameObject.Find("NameAnimation").GetComponent<Text>();
        bulletTrailPreb = GameObject.Find("WeaponSpawnPoint");
        marketUI = GameObject.Find("UICrosshair");
        BulletTrail = GameObject.Find("HotTrail").GetComponent<TrailRenderer>();
        playerCamera = GameObject.Find("PlayerCam").transform;
        audioSource = GetComponent<AudioSource>();
        bulletHolePreb = CManageResources.Inst.getBulletHoleStone();
        setState((int)GunState.Setup);
        VFXMP5K = GetComponent<CMuzzleController>();
        VFXMP5K.SetRates(0);
        VFXMP5K.PlayeVisualEffect();
        isAuto = true;
    }

  

    private void Update()
    {
        AnimatorStateInfo currentState = _anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Estado actual" + state);
        float TimeFinish = currentState.normalizedTime;

        switch (state)
        {
            case (int)GunState.Idle:
                AnimationNameFunction("Calico-Idle");
                float TimeAudioclip = audioSource.time;
                float audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXIdle;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Calico-Idle"))
                {
                    _anim.Play("Calico-Idle");
                }
                if ((Input.GetKeyDown(KeyCode.R) && extra_ammo > 0) && ammo_in_mag >= 1 && ammo_in_mag != mag_size /*&& ammo_in_mag < 1*/ /*&& isReload==true*/)
                {
                    //iniciar la animacion de recarga que tiene las valas normales
                    audioSource.Stop();
                    setState((int)GunState.Reload);

                }
                else if ((Input.GetKeyDown(KeyCode.R) && extra_ammo > 0) && ammo_in_mag <= 0 && ammo_in_mag != mag_size /*&& isReload == true*/)
                {
                    //iniciar la animacion de recarga que no tiene balas
                    audioSource.Stop();
                    setState((int)GunState.ReloadNotAmmo);
                }

                if (Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate))
                {
                    if (!isAuto)
                    {
                        audioSource.Stop();
                        setState((int)GunState.Shoot);
                    }
                    if (isAuto)
                    {
                        audioSource.Stop();
                        setState((int)GunState.AutoShoot);
                    }
                }
                else
                {
                    timeSinceLastShot += Time.deltaTime;
                }
                break;

            case (int)GunState.Shoot:
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                AnimationNameFunction("Calico-Shoot-Single");
                VFXMP5K.SetRates(1);
                StartCoroutine(FireMuzzle());
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSingleFire;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Calico-Shoot-Single"))
                {
                    _anim.Play("Calico-Shoot-Single");
                    //ammo_in_mag--;
                    Fire();
                    timeSinceLastShot = 0f;
                }

                if (TimeFinish > .98f)
                {
                    audioSource.Stop();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle);
                }
                break;

            case (int)GunState.AutoShoot:
                Debug.LogWarning("timeSinceLastShot es" + timeSinceLastShot);
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;


                AnimationNameFunction("calico-Shoot-Auto");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXAutoShoot;
                    audioSource.Play();
                }
                if ((ammo_in_mag > 0) && (timeSinceLastShot >= fireRate))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("calico-Shoot-Auto"))
                    {
                        _anim.Play("calico-Shoot-Auto");
                    }
                    if (TimeFinish > .8f)
                    {
                        _anim.Play("calico-Shoot-Auto");
                    }
                    Debug.Log("Entra");

                    //_anim.Play("mp5k-shoot-automatic");
                    Fire();
                    VFXMP5K.SetRates(30);
                    timeSinceLastShot = 0f;
                    //.RecoilFire();
                    //ammo_in_mag--;

                }
                if ((ammo_in_mag <= 0))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Calico-Idle"))
                    {
                        audioSource.Stop();
                    
                        _anim.Play("Calico-Idle");
                    }
                }
                timeSinceLastShot += Time.deltaTime;

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    audioSource.Stop();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle);
                }
                break;

            case (int)GunState.Desequip:
                AnimationNameFunction("calico-Shoot-Auto");
                break;

            case (int)GunState.Reload:

                AnimationNameFunction("calico-reload");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReload;
                    audioSource.Play();
                }
                //isReload = false;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("calico-reload"))
                {
                    _anim.Play("calico-reload");
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

                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.ReloadNotAmmo:
                AnimationNameFunction("Calico-Reload-NotAmmo");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReloadNOTAMMO;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Calico-Reload-NotAmmo"))
                {
                    _anim.Play("Calico-Reload-NotAmmo");
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
                    setState((int)GunState.Idle);
                }
                break;

            case (int)GunState.Setup:
                AnimationNameFunction("Calico-setup");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSetup;
                    audioSource.Play();
                }
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;

                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("Calico-setup"))
                {
                    _anim.Play("Calico-setup");

                }
                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.Drop:
                float timeToDeath = currentState.normalizedTime;
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXDrop;
                    audioSource.Play();
                }
                AnimationNameFunction("calico-Drop");
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("calico-Drop"))
                {
                    _anim.Play("calico-Drop");
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

        if (Input.GetKeyDown(KeyCode.X))
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
        //    //{ 
        //    //    ValueIndex+=1;
        //    //   _anim.Play(ListAnimationDebug[(int)ValueIndex].ToString());
        //    //}
        //    ValueIndex += 1;
        //    _anim.Play(ListAniamtionArrayDebug[ValueIndex]);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //    if (ValueIndex <= ListAnimationDebug.Count)
        //    //    {
        //    ValueIndex -= 1;
        //    _anim.Play(ListAniamtionArrayDebug[ValueIndex]);
        //}
    }

    public void Fire()
    {
        // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
        // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

        //Animator.SetBool("IsShooting", true);
        //ShootingSystem.Play();
        Vector3 direction = GetDirection();

        //if (Physics.Raycast(bulletTrailPreb.transform.position, Vector3.forward, out RaycastHit hit, float.MaxValue, Mask))
        //{
        //    TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);

        //    StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

        //    LastShootTime = Time.time;
        //}
        //// this has been updated to fix a commonly reported problem that you cannot fire if you would not hit anything
        //else
        //{
        TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);

        StartCoroutine(SpawnTrail(trail, playerCamera.transform.forward * 1000, Vector3.zero, false));

        LastShootTime = Time.time;

        RayCastForEne();
        ammo_in_mag--;
        recoil_Script.RecoilFire();

    }
    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        //if (AddBulletSpread)
        //{
        direction += new Vector3(
            Random.Range(-BulletSpreadVariance.x, BulletSpreadVariance.x),
            Random.Range(-BulletSpreadVariance.y, BulletSpreadVariance.y),
            Random.Range(-BulletSpreadVariance.z, BulletSpreadVariance.z)

            );
        //}
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
        setState((int)GunState.Setup);
    }
    public override void Desequip()
    {
        base.Desequip();
        audioSource.Stop();
        setState((int)GunState.Desequip);
    }

    public override void Drop()
    {
        base.Drop();
        audioSource.Stop();
        setState((int)GunState.Drop);
    }
    IEnumerator ShootGun()
    {
        //Shoot();
        RayCastForEne();
        recoil_Script.RecoilFire();
        yield return new WaitForSeconds(fire_rate);
        _canShoot = true;

    }

    public void RayCastForEne()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit, Mathf.Infinity, maskEnemy))
        {

            Debug.Log("Hit an Enemy");

            marketUI.GetComponent<CHitmarket>().Hit();
            hit.collider.GetComponent<Mafioso>().TakeDamage(damage);


        }
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

    IEnumerator FireMuzzle()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        MuzzleFlash.SetActive(false);
    }
    public override void AnimationNameFunction(string nameAnimation)
    {
        base.AnimationNameFunction(nameAnimation);
    }

}
