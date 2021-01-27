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
    int enemyLayer;

    [Header("Stats")]
    public int Magazine;
    public float RPM;
    public float maxRange;
    [Range(0,5)]
    public float reloadTimeSeconds;
    [Range(0,10)]
    public float Spread; // 0 = noSpread
    public bool noReload, autoAction;
    public bool isBow;
    Vector2 barrelDir;
    public float aimSec;

    #region Stat
    bool isAiming ,isReloading, Trigger, readyToShoot;
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
        enemyLayer = 1 << LayerMask.NameToLayer("Enemy");

        if (noReload == true || isBow == true)
        {
            Magazine = 99999;
        }
        curAmmo = Magazine;
        readyToShoot = true;

    }

    void Update()
    {
        if(isBow)
        {
            Bow();
        }
        else
        {
            Gun();
        }

        MyInput();
    }

    void Bow()
    {
        if (!isReloading && curAmmo > 0 && readyToShoot == true)
        {
            if(isAiming && Trigger)
            {
                Shoot();
                Invoke("ResetShot", timeBetweenShots);
            }
        }
    }

    void Gun()
    {
        if (!isReloading && curAmmo > 0 && readyToShoot == true)
        {
            if (Trigger == true)
            {
                Shoot();
                Invoke("ResetShot", timeBetweenShots);
            }
        }
    }
    void MyInput()
    {
        if(autoAction)
        {
            Trigger = Input.GetMouseButton(0);
        }
        else
        {
            Trigger = Input.GetMouseButtonDown(0);
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
    void Aim()
    {

    }
    void Shoot()
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 dir = (Input.mousePosition - sp).normalized;
        float mouseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float spread = Random.Range(-Spread, Spread);
        Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, mouseAngle + spread));
        barrelDir = dir;
        Instantiate(bulletToFire, transform.position, bulletRotation);
        Calculate();
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

    float curRange()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, barrelDir, maxRange, enemyLayer);
        if(hit)
        {
            return hit.distance;
        }
        else
        {
            return 0;
        }
    }
    public void Calculate()
    {
        float TotalValue = ((maxRange - curRange()) / (maxRange)) * 100;
    }
}
