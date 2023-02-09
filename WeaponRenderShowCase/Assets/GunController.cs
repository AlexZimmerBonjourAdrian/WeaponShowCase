using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GunController : MonoBehaviour
{
    [Header("Gun Setting")]
    public float fireRate = 0.1f;
    public int clipSize = 30;
    public int reservedAmmoCapacity = 270;
    // Start is called before the first frame update

    [SerializeField] bool _canShoot;
    [SerializeField]int _currentAmmoInClip;
    [SerializeField] int _ammoInReserve;

    //public Image muzzleFlashImage;

    public Sprite[] flashes;

    public Vector3 normalLocalPosition;
    public Vector3 aimingLocalPosition;

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

   [SerializeField] private LayerMask EnemyMask;

    [SerializeField] private int Damage = 50;

    [SerializeField] private float range=30;

   
    void Start()
    {

        _currentAmmoInClip = clipSize;
        _ammoInReserve = reservedAmmoCapacity;
        _canShoot = true;

    }
    // Update is called once per frame
    void Update()
    {
        DetermineAim();
        DetermineRotation();
        if(Input.GetMouseButton(0) && _canShoot && _currentAmmoInClip > 0)
        {
            _canShoot = false;
            _currentAmmoInClip--;
            StartCoroutine(ShootGun());
        }
        else if(Input.GetKeyDown(KeyCode.R) && _currentAmmoInClip < clipSize && _ammoInReserve > 0)
        {
            int amountNeeded = clipSize - _currentAmmoInClip;
            if(amountNeeded >= _ammoInReserve)
            {

                _currentAmmoInClip += _ammoInReserve;
                _ammoInReserve -= amountNeeded;
            }
            else 
            {
                _currentAmmoInClip = clipSize;
                _ammoInReserve -= amountNeeded;
            }
        }
    }

    void DetermineRotation()
    {
        Vector2 mouseAxis = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        mouseAxis *= mouseSensitivity;
        _currentRotation += mouseAxis;

        _currentRotation.y = Mathf.Clamp(_currentRotation.y, -90, 90);

        transform.localPosition += (Vector3)mouseAxis * weaponSwayAmount / 1000;
        
        transform.root.localRotation = Quaternion.AngleAxis(_currentRotation.x, Vector3.up);
        transform.parent.localRotation = Quaternion.AngleAxis(-_currentRotation.y, Vector3.right);
    }

    void DetermineAim()
    {
        Vector3 target = normalLocalPosition;
        if (Input.GetMouseButton(1)) target = aimingLocalPosition;

        Vector3 desiredPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * aimSmoothing);

        transform.localPosition = desiredPosition;
    }
    void DetermineRecoil()
    {
        transform.localPosition -= Vector3.forward * 0.1f;
        if(randomizeRecoil)
        {
            float xRecoil = Random.Range(-randomRecoilConstraints.x, randomRecoilConstraints.x);
            float yRecoil = Random.Range(-randomRecoilConstraints.y, randomRecoilConstraints.y);

            Vector2 recoil = new Vector2(xRecoil, yRecoil);

            _currentRotation += recoil;
        }
        else
        {
            int currentStep = clipSize + 1 - _currentAmmoInClip;
            currentStep = Mathf.Clamp(currentStep, 0, recoilPattern.Length - 1);

            _currentRotation += recoilPattern[currentStep];

        
        }
    }
    IEnumerator ShootGun()
    {
        //StartCoroutine(MuzzleFlash());
        RayCastForEne();
        DetermineRecoil();
        yield return new WaitForSeconds(fireRate);
        _canShoot = true;
    }

    //IEnumerator MuzzleFlash()
    //{
    //    muzzleFlashImage.sprite = flashes[Random.Range(0, flashes.Length)];
    //    muzzleFlashImage.color = Color.white;
    //    yield return new WaitForSeconds(0.05f);
    //    muzzleFlashImage.sprite = null;
    //    muzzleFlashImage.color = new Color(0, 0, 0, 0);

    //}

    void RayCastForEne()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit,1 << LayerMask.NameToLayer("enemy")))
        {
            try
            {

                Debug.Log("Hit an Enemy");
                //Rigidbody rb = hit.transform.GetComponent<Rigidbody>();
                //rb.constraints = RigidbodyConstraints.None;
                //rb.AddForce(transform.parent.transform.forward * 500);
                hit.collider.gameObject.GetComponent<CMafioso>().DestroyEnemy();
                // Debug.Log(hit.collider.gameObject.GetComponent<CMafioso>().Hearth);
                Debug.DrawRay(transform.position, transform.forward, Color.red);
            }
            catch
            {

            }
            
           
        }
    }
}
