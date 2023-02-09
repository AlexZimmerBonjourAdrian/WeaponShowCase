using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class CPPK : CArmed
{

    [Header("Gun Setting")]
    public float fireRate = 0.1f;
    // Start is called before the first frame update

    [SerializeField] bool _canShoot;


    public enum GunState
    {
        Summon = 0,
        Idle = 1,
        IdleNotAmmo = 2,
        Shoot = 3,
        ShootNotAmmo = 4,
        Reload = 5,
        ReloadNotAmmo = 6,
        Desequip = 7,
        DesequipNotAmmo = 8,
        Drop = 9,
        DropNotAmmo = 10
    }
    public int state;
    //public Image muzzleFlashImage;

    public Sprite[] flashes;

    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;
    [SerializeField] int _ammoInReserve;
    public float aimSmoothing = 10f;

    [Header("Mouse setting")]
    public float mouseSensitivity = 10;
    Vector2 _currentRotation;
    public float weaponSwayAmount = 10f;
    //Weapon Recoil
    public bool randomizeRecoil;
    public Vector2 randomRecoilConstraints;
    //you only need to assign this if randomizeRecoil if off
    public Vector2[] recoilPattern;
    public bool _CanReload;
    [SerializeField] protected LayerMask LayerEnemy;
    [SerializeField]
    private GameObject bulletHolePreb;  
    //private weapon_Input _input;
    private Animator _anim;

    public float range = 100f;
    [Header("DEBUG!!!!----------")]
    public List<string> ListAnimationDebug;
    [SerializeField] public string[] ListAniamtionArrayDebug;
    public uint ValueIndex = 0;
    float timeSinceLastShot = 0f;
    new void Start()
    {
       
        transform.localPosition = new Vector3(-0.0004f, -0.0154f, -0.003f);
        _anim = GetComponent<Animator>();
        recoil_Script = GameObject.Find("CameraRecoil").GetComponent<CRecoil>();
        bulletTrailPreb = GameObject.Find("WeaponSpawnPoint");
        BulletTrail = GameObject.Find("HotTrail").GetComponent<TrailRenderer>();
        playerCamera = GameObject.Find("PlayerCam").transform;
        marketUI = GameObject.Find("UICrosshair");
        _anim.SetInteger("Ammo", ammo_in_mag);
        setState((int)GunState.Summon);
        LoadInfo();
        bulletHolePreb = CManageResources.Inst.getBulletHoleWall();
        //_canShoot = true;
        //_CanReload = false;
       
        
    }

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
    void Update()
    {
        AnimatorStateInfo currentState = _anim.GetCurrentAnimatorStateInfo(0);
        Debug.Log("Estado actual" + state);
        float TimeFinish = currentState.normalizedTime;

        switch (state)
        {
            case (int)GunState.IdleNotAmmo:
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-idle-NotAmmo"))
                {
                    _anim.Play("ppk-idle-NotAmmo");
                }
            
                if ((Input.GetKeyDown(KeyCode.R) && extra_ammo > 0) && ammo_in_mag <= 0 && ammo_in_mag != mag_size /*&& isReload == true*/)
                {
                    //iniciar la animacion de recarga que no tiene balas
                    setState((int)GunState.ReloadNotAmmo);
                }
                if (Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate))
                {
                     //setState((int)GunState.Shoot); 
                     //TODO: Ejecutar sonido de recamara vasia
                }
                else
                {
                    timeSinceLastShot += Time.deltaTime;
                }
                break;
            case (int)GunState.Idle:
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-idle-normal"))
                {
                    _anim.Play("ppk-idle-normal");
                }
                if ((Input.GetKeyDown(KeyCode.R) && extra_ammo > 0) && ammo_in_mag >= 1 && ammo_in_mag != mag_size /*&& ammo_in_mag < 1*/ /*&& isReload==true*/)
                {
                    //iniciar la animacion de recarga que tiene las valas normales
                    setState((int)GunState.Reload);
                }
                if (Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate) && ammo_in_mag >= 2)
                {
                    setState((int)GunState.Shoot);
                }
                else if (Input.GetKey(KeyCode.Mouse0) && (timeSinceLastShot >= fireRate) && ammo_in_mag <= 1)
                {
                    setState((int)GunState.ShootNotAmmo);
                }
                if(ammo_in_mag < 0)
                {
                    ammo_in_mag = 0;
                }

                else
                {
                    timeSinceLastShot += Time.deltaTime;
                }
               
                break;

            case (int)GunState.Shoot:


                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-Shoot-Normal"))
                {
                    _anim.Play("ppk-Shoot-Normal");
                    //ammo_in_mag--;
                    Fire();
                    timeSinceLastShot = 0f;
                }

                if (TimeFinish > .98f)
                {
                    setState((int)GunState.Idle);
                }
                break;

            case (int)GunState.ShootNotAmmo:
                Debug.LogWarning("timeSinceLastShot es" + timeSinceLastShot);
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;


                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("PPK-Fire-NotAmmo"))
                {
                    _anim.Play("PPK-Fire-NotAmmo");
                    //ammo_in_mag--;
                    Fire();
                    timeSinceLastShot = 0f;
                }

                if (TimeFinish > .98f)
                {
                    setState((int)GunState.IdleNotAmmo);
                }

                break;

            case (int)GunState.Desequip:

                break;

            case (int)GunState.DesequipNotAmmo:

                break;

            case (int)GunState.Reload:

                //isReload = false;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-Reload-Normal"))
                {
                    _anim.Play("ppk-Reload-Normal");
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
                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.ReloadNotAmmo:
               
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-Reload-Notammo"))
                {
                    _anim.Play("ppk-Reload-Notammo");
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
               else if (TimeFinish > .98f)
                {
                    setState((int)GunState.IdleNotAmmo);
                }
                break;

            case (int)GunState.Summon:
                currentState = _anim.GetCurrentAnimatorStateInfo(0);
                TimeFinish = currentState.normalizedTime;

                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-Summon"))
                {
                    _anim.Play("ppk-Summon");

                }
                if (TimeFinish > .9f)
                {
                    setState((int)GunState.Idle);
                }

                break;

            case (int)GunState.Drop:
                float timeToDeath = currentState.normalizedTime;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-Drop-normal"))
                {
                    _anim.Play("ppk-Drop-normal");
                }
                if (timeToDeath > .9f)
                {
                    Destroy(gameObject);
                }
                break;
            
            case (int)GunState.DropNotAmmo:
                 timeToDeath = currentState.normalizedTime;
                if (!_anim.GetCurrentAnimatorStateInfo(0).IsName("ppk-Drop-NotAmmo"))
                {
                    _anim.Play("ppk-Drop-NotAmmo");
                }
                if (timeToDeath > .9f)
                {
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

        //if (Input.GetKeyDown(KeyCode.Mouse0) && _canShoot && ammo_in_mag > 2 )
        //{
        //    StartCoroutine(Shootgun());
        //    ammo_in_mag--;
        //    recoil_Script.RecoilFire();
        //    //_canShoot = false;
        //}
        //else if (Input.GetKeyDown(KeyCode.Mouse0) && _canShoot && ammo_in_mag <= 1)
        //{
        //    StartCoroutine(ShootgunNotAmmo());
        //    ammo_in_mag--;
        //    recoil_Script.RecoilFire();
        //    //_canShoot = false;
        //}


        //else if (Input.GetKeyDown(KeyCode.R) && ammo_in_mag < mag_size && extra_ammo > 0)
        //{
        //    int amoutNeeded = mag_size - ammo_in_mag;
        //    if (amoutNeeded >= extra_ammo)
        //    {
        //        ammo_in_mag += extra_ammo;
        //        extra_ammo -= amoutNeeded;
        //    }
        //    else
        //    {
        //        ammo_in_mag = mag_size;
        //        extra_ammo -= amoutNeeded;
        //    }


        //CODIGO DE DEBUG PARA PROBAR ANIMACIONES
        //if (Input.GetKeyDown(KeyCode.Q))
        //{

        //    //if (ValueIndex <= ListAnimationDebug.Count)
        //    //{ 
        //    //    ValueIndex+=1;
        //    //   _anim.Play(ListAnimationDebug[(int)ValueIndex].ToString());
        //    //}
        //    ValueIndex += 1;
        //     _anim.Play(ListAniamtionArrayDebug[ValueIndex]);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    //    if (ValueIndex <= ListAnimationDebug.Count)
        //    //    {
        //     ValueIndex -= 1;
        //    _anim.Play(ListAniamtionArrayDebug[ValueIndex]);
        //}
        //}
    }

    public override void Add_ammo(DataPickUp PickUp)
    {
        base.Add_ammo(PickUp);
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
    }
    public override void Desequip()
    {
        base.Desequip();
    }

    public override void Drop()
    {
        base.Drop();
    }

    IEnumerator ShootGun()
    {
        // Shoot();
        RayCastForEne();
        // recoil_Script.RecoilFire();
        ammo_in_mag--;
        timeSinceLastShot = 0f;
        _canShoot = false;
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;

    }
    IEnumerator ShootgunNotAmmo()
    {
        yield return new WaitForSeconds(fire_rate);
        RayCastForEne();
        //Shoot();
        _anim.Play("PPK_Shoot_Not_Ammo");
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
        BulletHole();
        ammo_in_mag--;
        recoil_Script.RecoilFire();








        //    //lastFiredAt = Time.time;
        //    //_canShoot = false;

        //    //timeSinceLastShot = 0f;
        //    //_canShoot = true;


        //    //FUNCTION DEBUG


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
    //public void Fire()
    //{

    //    ammo_in_mag--;
    //    //RayCastForEne();
    //    //lastFiredAt = Time.time;
    //    //_canShoot = false;

    //    //timeSinceLastShot = 0f;
    //    //_canShoot = true;

    //}


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

    public void setState(int newState)
    {
        state = newState;
    }
}

