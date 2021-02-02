using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RangedWeapon : MonoBehaviour
{
    public WeaponSO weaponSo;

    public GameObject bulletToFire;
    public GameObject effectMuzzle;
    public EquipmentType EquipmentType;
    [Header("Stats")]
    public int Magazine;
    public float RPM;
    [Range(1,50)]
    public float maxRange;
    [Range(0,3)]
    public float aimingSec;
    [Range(0,5)]
    public float reloadTimeSeconds;
    [Range(0,50)]
    public float Spread;
    [Range(0,100)]
    public float Mobility;
    [Range(0,100)]
    public float Ergonomic;

    public bool noReload;
    public bool autoAction;
    public bool isBow;
    SpriteRenderer sr;
    private playerController player;
    int enemyLayer;
    Vector2 barrelDir;

    #region
    bool isAiming ,isReloading, Trigger, readyToShoot;
    int curAmmo;

    public float timeBetweenShots { get { return 1 / (RPM / 60); } }
    public float Mob { get { return 1-((100 - Mobility) / 100); } }
    public float Erg { get { return ((100 - Ergonomic) / 100); } }
    #endregion
    public void Synch(WeaponSO item,Sprite sprite,GameObject bullet, GameObject EffMuz, EquipmentType wT, int m, float rpm, float mr, float aS, float Spr, float Mob, float Ergo, bool nR, bool aA, bool iB)
    {
        this.enabled = true;
        this.weaponSo = item;
        this.sr.sprite = sprite;
        this.bulletToFire = bullet;
        this.effectMuzzle = EffMuz;
        this.EquipmentType = wT;
        this.Magazine = m;
        this.RPM = rpm;
        this.maxRange = mr;
        this.aimingSec = aS;
        this.Spread = Spr;
        this.Mobility = Mob;
        this.Ergonomic = Ergo;
        this.noReload = nR;
        this.autoAction = aA;
        this.isBow = iB;

        player.Synch();
    }
    public void DeSynch()
    {
        this.weaponSo = null;
        sr.sprite = null;
        this.enabled = false;

        player.DeSynch();
    }
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

    private void OnValidate()
    {
        gameObject.name = EquipmentType.ToString() + " Arm";
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
    void ResetShot()
    {
        curAmmo--;
        readyToShoot = true;
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

        if(isReloading == false)
        {
            if(Input.GetKeyDown(KeyCode.R) || curAmmo <= 0)
            {
                isReloading = true;
                StartCoroutine(Reload());
            }
        }
    }
    void Shoot()
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(player.transform.position);
        Vector3 dir = (Input.mousePosition - sp).normalized;
        float mouseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float spread = Random.Range(-Spread, Spread);
        Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, 0, mouseAngle + spread));
        barrelDir = dir;
        GameObject BulletObject = Instantiate(bulletToFire, transform.position, bulletRotation) as GameObject;

        if (CanHit())
        {
            BulletObject.GetComponent<Collider2D>().enabled = true;
        }
        else
        {
            BulletObject.GetComponent<Collider2D>().enabled = false;
        }
        
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
        if(hit) return hit.distance;
        else return maxRange;
    }
    float RangeValue()
    {
        float TotalValue = ((maxRange - curRange()) / (maxRange)) * 100;
        return TotalValue;
    }
    bool CanHit()
    {
        float RandomNumber = Random.Range(0, 101);
        float RangeNumber = RangeValue();

        if(RandomNumber <= RangeNumber)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
