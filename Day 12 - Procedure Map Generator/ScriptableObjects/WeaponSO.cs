using UnityEngine;
public enum EquipmentType
{
    MainWeapon,
    SubWeapon,
    Bag,
}
[CreateAssetMenu(menuName ="Items/Weapon Items")]
public class WeaponSO : ItemSO
{
    public GameObject bulletToFire;
    public GameObject effectMuzzle;
    public EquipmentType equipmentType;
    [Header("Stats")]
    public int Magazine;
    public float RPM;
    [Range(1, 50)]
    public float maxRange;
    [Range(0, 3)]
    public float aimingSec;
    [Range(0, 5)]
    public float reloadTimeSeconds;
    [Range(1, 50)]
    public float Spread;
    [Range(0, 100)]
    public float Mobility;
    [Range(0, 100)]
    public float Ergonomic;

    public bool noReload;
    public bool autoAction;
    public bool isBow;
    public void Equip(InventoryManager invManager,WeaponPanel weaponPanel)
    {
        
        if (weaponPanel.CheckItemType(this))
        {
            invManager.weaponScript.Synch(this,Icon, bulletToFire, effectMuzzle, equipmentType, Magazine, RPM, maxRange, aimingSec, Spread, Mobility, Ergonomic, noReload, autoAction, isBow);
        }
        
    }
    public void Unequip(InventoryManager invManager,WeaponPanel weaponPanel)
    {
        if (weaponPanel.CheckItemType(this))
        {
            invManager.weaponScript.DeSynch();
        }
    }
    
    public override string GetItemType()
    {
        return equipmentType.ToString();//weaponType.ToString();
    }
    
    public override string GetDescription()
    {
        sb.Length = 0;


        return sb.ToString();
    }

    private void AddStat(float value, string statName, bool isPercent = false)
    {
        /*
         * StringBuilder = 자주 변경되는 String용 언어
         * 예시)
         * sb.Append("Click");
         * sb.Append("Space");
         * sb.Append("Bar");
         * sb.appendLine("SpaceBar");  appendLine이 작동안될 때 sb.Append(Envrionment.Newline); 를 응용하자 *using.system 포함
         * 결과  
         * ClickSpacebar
         * Spacebar
         */
        if (value != 0)
        {
            sb.Append(statName);

            if (value > 0)
            {
                sb.Append("=");
            }

            if (!isPercent)
            {
                sb.Append(value);
                sb.Append(" ");
            }
            else
            {
                sb.Append(value * 100); // 백퍼센트 계산식 총 시험점수가 850, 총 학생점수가 680일 때 ( 680 / 850 )* 100 = 80 %
                sb.Append(" % ");
            }

        }
    }
}
