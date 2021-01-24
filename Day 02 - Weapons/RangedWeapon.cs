using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedWeapon : MonoBehaviour
{

    [Header("Addable")]
    public GameObject bulletToFire;
    public GameObject effectMuzzle;
    public SpriteRenderer sr;
    public EquipType weaponType;
    private playerController player;

    [Header("Stats")]
    public int Magazine;
    public float RPM;
    [Range(0,5)]
    public float reloadTimeSeconds;
    [Range(0,10)]
    public float Spread; // 0 = noSpread
    public bool noReload, autoAction;
    public bool isBow, isGun;
    #region Stat
    bool isAiming ,isReloading, canShoot, readyToShoot;
    int curAmmo;
    public float timeBetweenShots { get { return 1 / (RPM / 60); } }
    #endregion


    void Awake()
    {
        player = GetComponentInParent<playerController>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (noReload == true)
        {
            Magazine = 99999;
        }
        curAmmo = Magazine;
        readyToShoot = true;

    }

    void Update()
    {
        MyInput();
        ShootingBehaviour();
    }
    void ShootingBehaviour()
    {
        if(isBow)
        {

        }
        else // if is Gun.
        {
            if (!isReloading && curAmmo > 0 && readyToShoot == true)
            {
                if (canShoot == true)
                {
                    Shoot();
                    Invoke("ResetShot", timeBetweenShots);
                }
            }
        }
    }
    void MyInput()
    {
        if(autoAction)
        {
            canShoot = Input.GetMouseButton(0);
        }
        else
        {
            canShoot = Input.GetMouseButtonDown(0);
        }

        isAiming = Input.GetMouseButton(1);

        if(isReloading == false)
        {
            if(Input.GetKeyDown(KeyCode.R) || curAmmo <= 0)
            {
                isReloading = true;
                StartCoroutine(Reload());
            }
        }
    }
    void ResetShot()
    {
        curAmmo--;
        readyToShoot = true;
    }

    void Shoot()
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 dir = (Input.mousePosition - sp).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float spread = Random.Range(-Spread, Spread);
        Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, angle + spread));

        Instantiate(bulletToFire, transform.position, bulletRotation);
        readyToShoot = false;
    }

    IEnumerator Reload()
    {
        if (isReloading)
        {
            yield return new WaitForSeconds(reloadTimeSeconds);
            curAmmo = Magazine;
            isReloading = false;
        }
    }
}
