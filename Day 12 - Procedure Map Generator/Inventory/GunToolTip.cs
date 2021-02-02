using UnityEngine.UI;
using UnityEngine;

public class GunToolTip : MonoBehaviour
{
    [SerializeField] Text ItemNameText;
    [SerializeField] Text ItemTypeText;
    [SerializeField] Text ItemDescriptionText;

    private void Start()
    {
        transform.gameObject.SetActive(false);
    }

    public void ShowToolTip(ItemSO item)
    {
        ItemNameText.text = item.ItemName;
        ItemTypeText.text = item.GetItemType();
        ItemDescriptionText.text = item.GetDescription();

        transform.gameObject.SetActive(true);

    }
    public void HideToolTip()
    {
        transform.gameObject.SetActive(false);
    }
}
