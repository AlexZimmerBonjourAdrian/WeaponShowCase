using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CM4Shootgun : CArmed
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
    public int state;
    public bool isAuto;



    public Vector3 normalLocapPoition;
    public Vector3 aimIngLocalPosition;

    public float aimSmoothing = 10f;

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
    [SerializeField] protected LayerMask LayerEnemy;
    [SerializeField] private Transform _Shootposition;
    [SerializeField] private float range = 30;
    //[SerializeField] private CRecoil recoil_Script;
    //Weapon Recoil
    float timeSinceLastShot = 0f;

    float fireRate = 0.4f;
    private Animator _anim;

    [Header("DEBUG!!!!----------")]
    public List<string> ListAnimationDebug;
    [SerializeField] public string[] ListAniamtionArrayDebug;
    public uint ValueIndex = 0;


    private Transform SpawmBulletTransform;
    [SerializeField]
    private GameObject bulletTrailPreb;
    private GameObject bulletHolePreb;


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
    private float ShootDelay = 0.08f;

    public AudioSource audioSource;
    private CMuzzleController VFXMP5K;

    [SerializeField] private AudioClip SFXSetup;
    [SerializeField] private AudioClip SFXAutoShoot;
    [SerializeField] private AudioClip SFXReloadNOTAMMO;
    [SerializeField] private AudioClip SFXReload;
    [SerializeField] private AudioClip SFXSingleFire;
    [SerializeField] private AudioClip SFXDesequip;
    [SerializeField] private AudioClip SFXDrop;
    [SerializeField] private AudioClip SFXIdle;

    private CBulletHole bulletHole;

    [SerializeField]
    private GameObject MuzzleFlash;
    
    // Start is called before the first frame update
    new void Start()
    {
        transform.localPosition = new Vector3(0.0052f, -0.0193f, -0.0039f);
        _canShoot = true;
        LoadInfo();
        _anim = GetComponent<Animator>();
        //_Shootposition = GetComponentInChildren<Transform>();
        recoil_Script = GameObject.Find("CameraRecoil").GetComponent<CRecoil>();
        bulletTrailPreb = GameObject.Find("WeaponSpawnPoint");
        marketUI = GameObject.Find("UICrosshair");
        BulletTrail = GameObject.Find("HotTrail").GetComponent<TrailRenderer>();
        NameAnimation = GameObject.Find("NameAnimation").GetComponent<Text>();
        playerCamera = GameObject.Find("PlayerCam").transform;
        setState((int)GunState.Setup);
        audioSource = GetComponent<AudioSource>();
        bulletHolePreb = CManageResources.Inst.getBulletHoleStone();
        VFXMP5K = GetComponent<CMuzzleController>();
        VFXMP5K.SetRates(0);
        VFXMP5K.PlayeVisualEffect();
        isAuto = true;
        bulletHole = GetComponent<CBulletHole>();
        bulletHole.setCamera(playerCamera);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case (int)GunState.Idle:
                AnimationNameFunction("m4Shootgun-Idle");
                AnimatorStateInfo currentState = _anim.GetCurrentAnimatorStateInfo(0);
                Debug.Log("Estado actual" + state);
                float TimeFinish = currentState.normalizedTime;
                float TimeAudioclip = audioSource.time;
                float audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXIdle;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shootgun-Idle"))
                {
                    _anim.Play("m4Shootgun-Idle");
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

                if (Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate) && ammo_in_mag > 0 )
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
                    audioSource.Stop();
                    timeSinceLastShot += Time.deltaTime;
                }
                break;

            case (int)GunState.Shoot:
                AnimationNameFunction("m4Shootgun-Shoot");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
               
                
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSingleFire;
                    audioSource.Play();
                }

                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shootgun-Shoot"))
                {
                    _anim.Play("m4Shootgun-Shoot");
                    //ammo_in_mag--;
                    StartCoroutine(FireMuzzle());
                    
                    Fire();
                    timeSinceLastShot = 0f;
                }

                if (TimeFinish > .98f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle);
                }
                break;

            case (int)GunState.AutoShoot:
                AnimationNameFunction("m4Shootgun-Shoot-Auto");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                Debug.Log("Entra en AutoLoad");
                //StartCoroutine(FireMuzzle());
                //float TimeAudioclip = audioSource.time;
                //float audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                VFXMP5K.SetRates(ammo_in_mag);
                if (!audioSource.isPlaying && ammo_in_mag > 0)
                {
                    audioSource.clip = SFXAutoShoot;
                    audioSource.Play();
                   
                }

                if ((ammo_in_mag > 0) && (timeSinceLastShot >= fireRate))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shootgun-Shoot-Auto"))
                    {
                        _anim.Play("m4Shootgun-Shoot-Auto");
                    }
                    if (TimeFinish > .8f)
                    {
                        _anim.Play("m4Shootgun-Shoot-Auto");
                    }
                    Debug.Log("Entra");

                    StartCoroutine(FireMuzzle());

                    //_anim.Play("mp5k-shoot-automatic");
                    Fire();
                    timeSinceLastShot = 0f;
                    //.RecoilFire();
                    //ammo_in_mag--;

                }
                //if ((ammo_in_mag >= 0))
                //{
                //    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shootgun-Idle"))
                //    {
                //        _anim.Play("m4Shootgun-Idle");
                //    }
                //}
              

                timeSinceLastShot += Time.deltaTime;

                if (Input.GetKeyUp(KeyCode.Mouse0))
                {
                    audioSource.Stop();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle);
                }
                break;

            case (int)GunState.Desequip:
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXDesequip;
                    audioSource.Play();
                }
                AnimationNameFunction("m4Shootgun-Shoot-Auto");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);

                break;

            case (int)GunState.Reload:
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReload;
                    audioSource.Play();
                }
                AnimationNameFunction("m4Shoothun-Reload-Normal");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);

                //isReload = false;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shoothun-Reload-Normal"))
                {
                    _anim.Play("m4Shoothun-Reload-Normal");
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
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReloadNOTAMMO;
                    audioSource.Play();
                }
                AnimationNameFunction("m4Shootgun-Reload-NotAmmo");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                //if (!audioSource.isPlaying)
                //{
                //    audioSource.clip = SFXReloadNOTAMMO;
                //    audioSource.Play();
                //}
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shootgun-Reload-NotAmmo"))
                {
                    _anim.Play("m4Shootgun-Reload-NotAmmo");
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
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSetup;
                    audioSource.Play();
                }
                AnimationNameFunction("m4Shootgun-Setup");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);

                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;

                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shootgun-Setup"))
                {
                    _anim.Play("m4Shootgun-Setup");

                }
                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.Drop:
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXDrop;
                    audioSource.Play();
                }
                AnimationNameFunction("m4Shootgun-Drop");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
              
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                float timeToDeath = currentState.normalizedTime;
                
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("m4Shootgun-Drop"))
                {
                    _anim.Play("m4Shootgun-Drop");
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

        //if (input.getkeydown(keycode.g))
        //{
        //    setstate((int)gunstate.drop);
        //}
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

    //public override void Shoot()
    //{
    //    base.Shoot();
    //}

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
        setState((int)GunState.Desequip);
    }

    public override void Drop()
    {
        base.Drop();
        //audioSource.Stop();
        setState((int)GunState.Drop);
    }

    public void Fire()
    {

        ammo_in_mag--;
        BulletTrailFunction();
        RayCastForEne();
        recoil_Script.RecoilFire();
       
    }

    public void BulletTrailFunction()
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
    IEnumerator ShootGun()
    {
        //Shoot();
        RayCastForEne();
        recoil_Script.RecoilFire();
        yield return new WaitForSeconds(fire_rate);
        _canShoot = true;

    }

    public void setState(int newState)
    {
        state = newState;
    }
    public void RayCastForEne()
    {
        RaycastHit hit_4;
        RaycastHit hit_1;
        RaycastHit hit_2;
        RaycastHit hit_3;


        //GameObject muzzleInstance = Instantiate(muzzle, spawnPoint.position, spawnPoint.localRotation);
        //muzzleInstance.transform.parent = spawnPoint;

        //Bulle that goes forward;
        //if(Physics.Raycast(_Shootposition.position, _Shootposition.forward, out hit,distance),)
        //BulletTrailFunction();
        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit_4, Mathf.Infinity))
        {

            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));

            //Apply damage if you have a method that does it;


            //Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.None;
            //rb.AddForce(transform.parent.transform.forward * 500);
            
            //Debug.Log(hit.collider.gameObject.GetComponent<CMafioso>().Hearth);
            //Debug.DrawRay(transform.position, transform.forward, Color.red);
            BulletHole(playerCamera.forward);
        }
        //BulletTrailFunction();
        if (Physics.Raycast(playerCamera.position, playerCamera.forward + new Vector3(-.2f, 0f, 0f), out hit_1, Mathf.Infinity))
        {
            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit_1.normal));


            Debug.Log("Hit an Enemy");

           
            BulletHole(playerCamera.forward + new Vector3(-.2f, 0f, 0f));

            //Apply damage if you have a method that does it;

        }
        //BulletTrailFunction();
        if (Physics.Raycast(playerCamera.position, playerCamera.forward + new Vector3(.0f, .1f, 0f), out hit_2, Mathf.Infinity))
        {
            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit_2.normal));

            //Apply damage if you have a method that does it;

         
            BulletHole(playerCamera.forward + new Vector3(.0f, .1f, 0f));


        }
        //BulletTrailFunction();
        if (Physics.Raycast(playerCamera.position, playerCamera.forward + new Vector3(0f, -1f, 0f), out hit_3, Mathf.Infinity))
        {
            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit_3.normal));
      
            BulletHole(playerCamera.forward + new Vector3(0f, -1f, 0f));

        }

        if (Physics.Raycast(playerCamera.position, playerCamera.forward, out hit_4, Mathf.Infinity, maskEnemy))
        {

            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));

            //Apply damage if you have a method that does it;
            //marketUI.GetComponent<CHitmarket>().Hit();
            //Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.None;
            //rb.AddForce(transform.parent.transform.forward * 500);
            if(hit_4.collider.GetComponent<CDataEnemy>() != null)
            { 
            hit_4.collider.GetComponent<CDataEnemy>().TakeDamage(20f);
            }
            //Debug.Log(hit.collider.gameObject.GetComponent<CMafioso>().Hearth);
            //Debug.DrawRay(transform.position, transform.forward, Color.red);
            BulletHole(playerCamera.forward);


        }
        //BulletTrailFunction();
        if (Physics.Raycast(playerCamera.position, playerCamera.forward + new Vector3(-.2f, 0f, 0f), out hit_1, Mathf.Infinity, maskEnemy))
        {
            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit_1.normal));


            Debug.Log("Hit an Enemy");

            //marketUI.GetComponent<CHitmarket>().Hit();
            //Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.None;
            //rb.AddForce(transform.parent.transform.forward * 500);
            if (hit_4.collider.GetComponent<CDataEnemy>() != null)
            {
                hit_1.collider.GetComponent<CDataEnemy>().TakeDamage(20f);
            }
            //Debug.Log(hit.collider.gameObject.GetComponent<CMafioso>().Hearth);
            //Debug.DrawRay(transform.position, transform.forward, Color.red);
            BulletHole(playerCamera.forward + new Vector3(-.2f, 0f, 0f));

            //Apply damage if you have a method that does it;

        }
        //BulletTrailFunction();
        if (Physics.Raycast(playerCamera.position, playerCamera.forward + new Vector3(.0f, .1f, 0f), out hit_2, Mathf.Infinity, maskEnemy))
        {
            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit_2.normal));

            //Apply damage if you have a method that does it;

            Debug.Log("Hit an Enemy");
            // marketUI.GetComponent<CHitmarket>().Hit();
            //Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.None;
            //rb.AddForce(transform.parent.transform.forward * 500);
            if (hit_4.collider.GetComponent<CDataEnemy>() != null)
            {
                hit_2.collider.GetComponent<CDataEnemy>().TakeDamage(20f);
            }
            //Debug.Log(hit.collider.gameObject.GetComponent<CMafioso>().Hearth);
            //Debug.DrawRay(transform.position, transform.forward, Color.red);
            BulletHole(playerCamera.forward + new Vector3(.0f, .1f, 0f));


        }
        //BulletTrailFunction();
        if (Physics.Raycast(playerCamera.position, playerCamera.forward + new Vector3(0f, -1f, 0f), out hit_3, Mathf.Infinity, maskEnemy))
        {
            //Instantiate(impact, hit.point, Quaternion.LookRotation(hit_3.normal));
            Debug.Log("Hit an Enemy");
            //marketUI.GetComponent<CHitmarket>().Hit();
            //Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.None;
            //rb.AddForce(transform.parent.transform.forward * 500);
            if (hit_4.collider.GetComponent<CDataEnemy>() != null)
            {
                hit_3.collider.GetComponent<CDataEnemy>().TakeDamage(20f);
            }
            //Debug.Log(hit.collider.gameObject.GetComponent<CMafioso>().Hearth);
            //Debug.DrawRay(transform.position, transform.forward, Color.red);
            BulletHole(playerCamera.forward + new Vector3(0f, -1f, 0f));

        }
    }
    public void BulletHole(Vector3 offset)
    {
        //Ray rayOriginal = Camera.main.ScreenPointToRay(Input.mousePosition);


        RaycastHit hitInfo;

        if (Physics.Raycast(playerCamera.position, offset, out hitInfo, Mathf.Infinity))
        {
            
                //Inantiate bullet hole
                //Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));

                Vector3 direction = hitInfo.point - transform.position;
           
            //transform.rotation = Quaternion.LookRotation(direction);
           

           
            
            if(hitInfo.collider.GetComponent<CTypeObjectBulletHole>() != null)
            { 
                var bulletHole = hitInfo.collider.GetComponent<CTypeObjectBulletHole>().BulletHole;

                switch ((int)bulletHole)
                {
                    case 0:
                        bulletHolePreb = CManageResources.Inst.getBulletHoleWood();
                        GameObject BH = Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    break;

                    case 1:
                        bulletHolePreb = CManageResources.Inst.getBulletHoleSteel();
                        BH = Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    break;
                    case 2:
                        bulletHolePreb = CManageResources.Inst.getBulletHoleStone();
                        BH = Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    break;
                    case 3:
                        bulletHolePreb = bulletHolePreb = CManageResources.Inst.getBulletHoleStone();
                        BH = Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    break;
                    case 4:
                        bulletHolePreb = bulletHolePreb = CManageResources.Inst.getBulletHoleBlood();
                        BH = Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    break;

                    default:
                        BH = Instantiate(bulletHolePreb, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    break;


                }
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
