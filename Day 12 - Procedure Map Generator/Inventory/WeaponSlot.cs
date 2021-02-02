
public class WeaponSlot : ItemSlot
{
    public EquipmentType weaponType;

    protected override void OnValidate()
    {
        base.OnValidate();
        gameObject.name = weaponType.ToString() + " Slot";
    }

    public override bool CanReceiveItem(ItemSO item)
    {
        // baseSlot X
        // ItemSlot O
        if(item == null)
        {
            return true;
        }
        WeaponSO weaponItem = item as WeaponSO; // item이 weaponItem이면 형변환을 수행, 아니면 null값 대입
        return weaponItem != null && weaponItem.equipmentType == weaponType; // 만약 weaponItem이 있고 weaponType마저 알맞다면 bool값 리턴
    }


}
