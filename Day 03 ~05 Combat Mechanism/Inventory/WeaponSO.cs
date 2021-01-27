using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum EquipType
{
    MainWeapon,
    SubWeapon,
}
[CreateAssetMenu(menuName ="Items/Weapon Items")]
public class WeaponSO : ItemSO
{
    public int accuracy;
    public int RPM;
    public float reloadSeconds;
    public int MaxAmmo;
    public EquipType type;


    public override string GetItemType()
    {
        return type.ToString();//weaponType.ToString();
    }
    public override string GetDescription()
    {
        sb.Length = 0;

        AddStat(accuracy, "Accuracy");
        AddStat(RPM, "RPM");
        AddStat(reloadSeconds, "ReloadTimeSeconds");
        AddStat(MaxAmmo, "MaxAmmo");
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
