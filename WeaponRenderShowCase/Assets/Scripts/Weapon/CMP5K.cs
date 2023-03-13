using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CMP5K : CArmed
{
    // Start is called before the first frame update
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
        Hoister = 7,
        Drop = 8


    }
    public Vector3 normalLocapPoition;
    public Vector3 aimIngLocalPosition;

   private CMuzzleController VFXMP5K;  

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

    [SerializeField] private float range = 30;
    //[SerializeField] private CRecoil recoil_Script;
    //Weapon Recoil

    public int state;
    public bool isAuto;
    float timeSinceLastShot = 0f;
    float fireRate = 0.08f;
    //Control de Recarga
    public AudioSource audioSource;

    [SerializeField] private AudioClip SFXSetup;
    [SerializeField] private AudioClip SFXAutoShoot;
    [SerializeField] private AudioClip SFXReloadNOTAMMO;
    [SerializeField] private AudioClip SFXReload;
    [SerializeField] private AudioClip SFXSingleFire;
    [SerializeField] private AudioClip SFXDesequip;
    [SerializeField] private AudioClip SFXDrop;
    [SerializeField] private AudioClip SFXIdle;

    //Animacoon Controller
    private Animator _anim;

    [Header("DEBUG!!!!----------")]
    public List<string> ListAnimationDebug;
    [SerializeField] public string[] ListAniamtionArrayDebug;
    public uint ValueIndex = 0;

    //[SerializeField]
    //private TrailRenderer BulletTrail;
    //[SerializeField]
    //private Renderer Renderer;
    //private bool isDisamblig = false;
    private GameObject bulletHolePreb;
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

    [SerializeField]
    private GameObject MuzzleFlash;
    // Start is called before the first frame update
   new void Start()
    {
        transform.localPosition = new Vector3(0f, -0.017f, 0f);
        _canShoot = true;
        _anim = GetComponent<Animator>();
        LoadInfo();
        marketUI = GameObject.Find("UICrosshair");
        //_Shootposition = GetComponentInChildren<Transform>();
        recoil_Script = GameObject.Find("CameraRecoil").GetComponent<CRecoil>();
        playerCamera = GameObject.Find("PlayerCam").transform;
       bulletTrailPreb = GameObject.Find("WeaponSpawnPoint");
        NameAnimation = GameObject.Find("NameAnimation").GetComponent<Text>();
        setState((int)GunState.Setup);
        audioSource = GetComponent<AudioSource>();
        bulletHolePreb = CManageResources.Inst.getBulletHoleStone();

        VFXMP5K = GetComponent<CMuzzleController>();
        VFXMP5K.SetRates(0);
        VFXMP5K.PlayeVisualEffect();

        isAuto = true;
        //CManagerSFX.Inst.AddAudioSource(audioSource);
        //_bulletTrailPreb = Instantiate(Resources.Load("SpectreTrail", typeof(GameObject))) as GameObject;

        //_bulletTrailPreb = Resources.Load("SpectreTrail", typeof(GameObject)) as GameObject;

      
        //BulletTrail = Resources.Load("Effects/SpectreTrail", typeof(GameObject)) as TrailRenderer;


    }

    // Update is called once per frame
    //void FixedUpdate()
    //{

    //    if (Input.GetKey(KeyCode.Mouse0) && _canShoot && ammo_in_mag > 0)
    //    {
    //        _canShoot = false;

    //        ammo_in_mag--;

    //        StartCoroutine(ShootGun());
    //    }

    //    else if (Input.GetKeyDown(KeyCode.R) && ammo_in_mag < mag_size && extra_ammo > 0)
    //    {
    //        int amoutNeeded = mag_size - ammo_in_mag;
    //        if (amoutNeeded >= extra_ammo)
    //        {
    //            ammo_in_mag += extra_ammo;
    //            extra_ammo -= amoutNeeded;
    //        }
    //        else
    //        {
    //            ammo_in_mag = mag_size;
    //            extra_ammo -= amoutNeeded;
    //        }
    //    }
    //}

    public void Update()
    {
        AnimatorStateInfo currentState = _anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Estado actual" + state);
        float TimeFinish = currentState.normalizedTime;
      

        switch (state)
        {
            case (int)GunState.Idle:
                AnimationNameFunction("mp5k-Idle");
                float TimeAudioclip = audioSource.time;
                float audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXIdle;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Idle"))
                {
                    _anim.Play("mp5k-Idle");
                }
                if ((Input.GetKeyDown(KeyCode.R) && extra_ammo > 0) && ammo_in_mag > 2 /*&& ammo_in_mag < 1*/ /*&& isReload==true*/)
                {
                    audioSource.Stop();
                    //iniciar la animacion de recarga que tiene las valas normales
                    setState((int)GunState.Reload);

                }
                else if ((Input.GetKeyDown(KeyCode.R) && extra_ammo > 0) && ammo_in_mag <= 1 /*&& isReload == true*/)
                {
                    audioSource.Stop();
                    //iniciar la animacion de recarga que no tiene balas
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

                //_canShoot = true;
                TimeAudioclip = audioSource.time;
                audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                AnimationNameFunction("mp5k-Shoot");
                VFXMP5K.SetRates(1);
                StartCoroutine(FireMuzzle());
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSingleFire;
                    audioSource.Play();
                
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Shoot"))
                {
                    _anim.Play("mp5k-Shoot");
                    
                }
                if (Input.GetButton("Fire1") && ammo_in_mag > 0)
                {
                   Fire();

                }
              
                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    //VFXMP5K.StopVisualEffect();
                    VFXMP5K.SetRates(0);
                    setState((int)GunState.Idle);
                }
                break;

            case (int)GunState.AutoShoot:
                AnimationNameFunction("mp5k-shoot-automatic");
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                 TimeAudioclip = audioSource.time;
                 audioNormalized = (TimeAudioclip - 0) / (TimeAudioclip - 0);
                 
                    
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXAutoShoot;
                    audioSource.Play();
                   
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-shoot-automatic"))
                {

                    _anim.Play("mp5k-shoot-automatic");

                }
                if (TimeFinish > .9f)
                {
                    //VFXMP5K.StopVisualEffect();
                    
                    _anim.Play("mp5k-shoot-automatic");
                }

                if ((ammo_in_mag > 0) && (timeSinceLastShot >= fireRate))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-shoot-automatic"))
                    {
                        _anim.Play("mp5k-shoot-automatic");
                    }
                    if (TimeFinish > .8f)
                    {
                        //VFXMP5K.StopVisualEffect();
                        _anim.Play("mp5k-shoot-automatic");
                    }
                    Debug.Log("Entra");
                    Fire();
                    StartCoroutine(FireMuzzle());
                    VFXMP5K.SetRates(ammo_in_mag);
                    timeSinceLastShot = 0f;
                }
                if ((ammo_in_mag <= 0))
                {
                    if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Idle"))
                    {
                        audioSource.Stop();
                        _anim.Play("mp5k-Idle");
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
                AnimationNameFunction("mp5k-Desequip");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXDesequip;
                    audioSource.Play();
                }
                TimeFinish = currentState.normalizedTime;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Desequip"))
                {
                    _anim.Play("mp5k-Desequip");
                }
                if (TimeFinish  > .9f)
                {
                    audioSource.Stop();
                    gameObject.SetActive(false);
                }
                break;

            case (int)GunState.Reload:
                isReload = false;
                AnimationNameFunction("mp5k-Reload");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReload;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Reload"))
                {
                    _anim.Play("mp5k-Reload");
                    int amoutNeeded = mag_size - ammo_in_mag;
                    if (!(extra_ammo <= 0))
                    {
                        _anim.Play("mp5k-Reload");
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


                }

                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.ReloadNotAmmo:
                AnimationNameFunction("mp5k-Realod-NotAmmo");
                isReload = false;
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXReloadNOTAMMO;
                    audioSource.Play();
                }
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Realod-NotAmmo"))
                {
                    _anim.Play("mp5k-Realod-NotAmmo");
                    _anim.Play("mp5k-Reload");
                    int amoutNeeded = mag_size - ammo_in_mag;
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
                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.Setup:
                AnimationNameFunction("mp5k-Equip");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXSetup;
                    audioSource.Play();
                }
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Equip"))
                {
                    _anim.Play("mp5k-Equip");

                }
                if (TimeFinish > .9f)
                {
                    audioSource.Stop();
                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.Drop:
                AnimationNameFunction("mp5k-Drop");
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = SFXDrop;
                    audioSource.Play();
                }
                float timeToDeath = currentState.normalizedTime;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("mp5k-Drop"))
                {
                    _anim.Play("mp5k-Drop");
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


        if (Input.GetKeyDown(KeyCode.G))
        {
            setState((int)GunState.Drop);
        }
        //if (Input.GetKeyDown(KeyCode.B))
        //{
        //    isAuto = !isAuto;
        //}
        //if (Input.GetKeyDown(KeyCode.Q))
        //{

        //    //if (ValueIndex <= ListAnimationDebug.Count)
        //    ////{ 
        //    //    ValueIndex+=1;
        //    //   _anim.Play(ListAnimationDebug[(int)ValueIndex].ToString());
        //    // }
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

        //if (Input.GetKeyUp(KeyCode.Mouse0))
        //{
        //    setState((int)GunState.Idle);
        //}

        //Desequip es segun que arma tengo en la siguiente o anterior casilla para poder cargarla y ejecutar la animacion
        //TODO: Consultar el array de las armas que contiene el jugador, chequear antes, chequear despues si hay un arma segun el boton asignado cambiarla
        //Chequear si el arma siguiente no esta vacio, Con un if realizar la animacion sea con "1" O "2" dependiendo de el resultado
        //Con bools es mas que suficiente

        //DEBUG FUNCTION
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    Fire();
        //}
    }

    public void setState(int newState)
    {
        state = newState;
    }
    public override void Shoot()
    {
        base.Shoot();
    }
    public override void Drop()
    {
        base.Drop();
        //audioSource.Stop();
        setState((int)GunState.Drop);
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

    public override void Equip()
    {
        base.Equip();
        setState((int)GunState.Setup);
    }
    public override void Desequip()
    {
        base.Desequip();
        setState((int)GunState.Desequip);
    }

   
    
    public override bool IsFinishAnimation(bool IsAnimation = false)
    {
        AnimatorStateInfo currentState = _anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Estado actual" + state);
        float TimeFinish = currentState.normalizedTime;
        IsAnimation = TimeFinish >= 1;
        return base.IsFinishAnimation(IsAnimation);
    }





    //IEnumerator ShootGun()
    //{
    //    // Shoot();
    //    //RayCastForEne();
    //    //recoil_Script.RecoilFire();
    //   // ammo_in_mag--;
    //    timeSinceLastShot = 0f;
    //    _canShoot = false;
    //    yield return new WaitForSeconds(fireRate);
    //    _canShoot = true;

    //}

    IEnumerator FireMuzzle()
    {
        MuzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        MuzzleFlash.SetActive(false);
    }

    public void Fire()
    {

        ammo_in_mag--;
        RayCastForEne();
        recoil_Script.RecoilFire();
       
        
      
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
            
        



        //    //lastFiredAt = Time.time;
        //    //_canShoot = false;

        //    //timeSinceLastShot = 0f;
        //    //_canShoot = true;


        //    //FUNCTION DEBUG


    }
    public void ReloadAnimationEvent()
    {
        isReload = true;
    }
    public void Fire_second()
    {
        if (LastShootTime + ShootDelay < Time.time)
        {
            // Use an object pool instead for these! To keep this tutorial focused, we'll skip implementing one.
            // For more details you can see: https://youtu.be/fsDE_mO4RZM or if using Unity 2021+: https://youtu.be/zyzqA_CPz2E

            //Animator.SetBool("IsShooting", true);
            //ShootingSystem.Play();
            Vector3 direction = GetDirection();

            if (Physics.Raycast(bulletTrailPreb.transform.position, Vector3.forward, out RaycastHit hit, float.MaxValue, Mask))
            {
                TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit.point, hit.normal, true));

                LastShootTime = Time.time;
            }
            // this has been updated to fix a commonly reported problem that you cannot fire if you would not hit anything
            else
            {
                TrailRenderer trail = Instantiate(BulletTrail, bulletTrailPreb.transform.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, playerCamera.transform.forward * 1000 , Vector3.zero, false));

                LastShootTime = Time.time;
            }
        }
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
    public override void AnimationNameFunction(string nameAnimation)
    {
        base.AnimationNameFunction(nameAnimation);
    }
}
 

